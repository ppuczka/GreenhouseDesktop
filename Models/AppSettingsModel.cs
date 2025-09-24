using GreenhouseDesktopApp.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GreenhouseDesktopApp.Models
{
    public class AppSettingsModel : INotifyPropertyChanged
    {
        private readonly IAppSettingsService _appSettingsService;
        
        private string _iotHubConnectionString = string.Empty;
        private int _refreshIntervalSeconds = 60;
        private bool _enableNotifications = true;
        private string _notificationEmail = string.Empty;

        public AppSettingsModel(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));
        }

        public string IotHubConnectionString
        {
            get => _iotHubConnectionString;
            set => SetProperty(ref _iotHubConnectionString, value);
        }

        public int RefreshIntervalSeconds
        {
            get => _refreshIntervalSeconds;
            set => SetProperty(ref _refreshIntervalSeconds, value);
        }

        public bool EnableNotifications
        {
            get => _enableNotifications;
            set => SetProperty(ref _enableNotifications, value);
        }

        public string NotificationEmail
        {
            get => _notificationEmail;
            set => SetProperty(ref _notificationEmail, value);
        }

        public async Task LoadSettingsAsync()
        {
            try
            {
                IotHubConnectionString = await _appSettingsService.GetSettingAsync("IotHubConnectionString", string.Empty);
                RefreshIntervalSeconds = await _appSettingsService.GetSettingAsync("RefreshIntervalSeconds", 60);
                EnableNotifications = await _appSettingsService.GetSettingAsync("EnableNotifications", true);
                NotificationEmail = await _appSettingsService.GetSettingAsync("NotificationEmail", string.Empty);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load application settings", ex);
            }
        }

        public async Task SaveSettingsAsync()
        {
            try
            {
                await _appSettingsService.SetSettingAsync("IotHubConnectionString", IotHubConnectionString);
                await _appSettingsService.SetSettingAsync("RefreshIntervalSeconds", RefreshIntervalSeconds);
                await _appSettingsService.SetSettingAsync("EnableNotifications", EnableNotifications);
                await _appSettingsService.SetSettingAsync("NotificationEmail", NotificationEmail);
                
                await _appSettingsService.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save application settings", ex);
            }
        }

        public async Task ResetToDefaultsAsync()
        {
            try
            {
                await _appSettingsService.ResetToDefaultsAsync();
                await LoadSettingsAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to reset settings to defaults", ex);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
