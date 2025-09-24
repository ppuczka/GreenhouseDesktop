using GreenhouseDesktopApp.Interfaces;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;

namespace GreenhouseDesktopApp.Services
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly string _configFilePath;
        private readonly ConcurrentDictionary<string, object> _settingsCache = new();
        private readonly SemaphoreSlim _fileLock = new(1, 1);
        private DateTime _lastLoadTime = DateTime.MinValue;
        private bool _isDirty = false;

        public AppSettingsService(string configFilePath = "config.json")
        {
            _configFilePath = configFilePath ?? throw new ArgumentNullException(nameof(configFilePath));
        }

        public async Task<T> GetSettingAsync<T>(string key, T defaultValue = default!)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

            await EnsureLoadedAsync();

            if (_settingsCache.TryGetValue(key, out var value))
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    // If conversion fails, return default value
                    return defaultValue;
                }
            }

            return defaultValue;
        }

        public async Task SetSettingAsync<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

            await EnsureLoadedAsync();

            _settingsCache.AddOrUpdate(key, value!, (k, v) => value!);
            _isDirty = true;
        }

        public async Task<bool> RemoveSettingAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            await EnsureLoadedAsync();

            var removed = _settingsCache.TryRemove(key, out _);
            if (removed)
                _isDirty = true;

            return removed;
        }

        public bool HasSetting(string key)
        {
            return !string.IsNullOrWhiteSpace(key) && _settingsCache.ContainsKey(key);
        }

        public async Task SaveAsync()
        {
            if (!_isDirty)
                return;

            await _fileLock.WaitAsync();
            try
            {
                var settingsDict = _settingsCache.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                var json = JsonSerializer.Serialize(settingsDict, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(_configFilePath, json);
                _isDirty = false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save settings to {_configFilePath}", ex);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task LoadAsync()
        {
            await LoadAsync(_configFilePath);
        }

        public async Task LoadAsync(string filePath)
        {
            await _fileLock.WaitAsync();
            try
            {
                _settingsCache.Clear();

                if (!File.Exists(filePath))
                {
                    await LoadDefaultSettingsAsync();
                    return;
                }

                var json = await File.ReadAllTextAsync(filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    await LoadDefaultSettingsAsync();
                    return;
                }

                var settings = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                if (settings != null)
                {
                    foreach (var kvp in settings)
                    {
                        _settingsCache.TryAdd(kvp.Key, kvp.Value);
                    }
                }

                _lastLoadTime = DateTime.UtcNow;
                _isDirty = false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load settings from {filePath}", ex);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task ResetToDefaultsAsync()
        {
            _settingsCache.Clear();
            await LoadDefaultSettingsAsync();
            _isDirty = true;
            await SaveAsync();
        }

        private async Task EnsureLoadedAsync()
        {
            if (_lastLoadTime == DateTime.MinValue)
            {
                await LoadAsync();
            }
        }

        private async Task LoadDefaultSettingsAsync()
        {
            var defaultSettings = new Dictionary<string, object>
            {
                { "IotHubConnectionString", string.Empty },
                { "RefreshIntervalSeconds", 60 },
                { "EnableNotifications", true },
                { "NotificationEmail", string.Empty }
            };

            foreach (var kvp in defaultSettings)
            {
                _settingsCache.TryAdd(kvp.Key, kvp.Value);
            }

            await Task.CompletedTask; 
        }
    }
}
