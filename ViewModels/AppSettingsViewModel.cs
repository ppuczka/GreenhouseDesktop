using GreenhouseDesktopApp.Models;
using System.Windows.Input;

namespace GreenhouseDesktopApp.ViewModels
{
    public class AppSettingsViewModel : ViewModelBase
    {
        private readonly AppSettingsModel _appSettings;
        private bool _isLoading;
        private string _statusMessage = string.Empty;

        public AppSettingsViewModel(AppSettingsModel appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            
            // Wire up property change notifications from the model
            _appSettings.PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
            
            // Initialize commands
            SaveCommand = new RelayCommand(async () => await SaveSettingsAsync(), () => !IsLoading);
            LoadCommand = new RelayCommand(async () => await LoadSettingsAsync(), () => !IsLoading);
            ResetCommand = new RelayCommand(async () => await ResetSettingsAsync(), () => !IsLoading);
        }

        public AppSettingsModel AppSettings => _appSettings;

        public string IotHubConnectionString
        {
            get => _appSettings.IotHubConnectionString;
            set => _appSettings.IotHubConnectionString = value;
        }

        public int RefreshIntervalSeconds
        {
            get => _appSettings.RefreshIntervalSeconds;
            set => _appSettings.RefreshIntervalSeconds = value;
        }

        public bool EnableNotifications
        {
            get => _appSettings.EnableNotifications;
            set => _appSettings.EnableNotifications = value;
        }

        public string NotificationEmail
        {
            get => _appSettings.NotificationEmail;
            set => _appSettings.NotificationEmail = value;
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            private set => SetProperty(ref _statusMessage, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand ResetCommand { get; }

        public async Task InitializeAsync()
        {
            await LoadSettingsAsync();
        }

        private async Task LoadSettingsAsync()
        {
            IsLoading = true;
            StatusMessage = "Loading settings...";
            
            try
            {
                await _appSettings.LoadSettingsAsync();
                StatusMessage = "Settings loaded successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to load settings: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SaveSettingsAsync()
        {
            IsLoading = true;
            StatusMessage = "Saving settings...";
            
            try
            {
                await _appSettings.SaveSettingsAsync();
                StatusMessage = "Settings saved successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to save settings: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ResetSettingsAsync()
        {
            IsLoading = true;
            StatusMessage = "Resetting settings...";
            
            try
            {
                await _appSettings.ResetToDefaultsAsync();
                StatusMessage = "Settings reset to defaults.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to reset settings: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    // Simple RelayCommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _executeAsync;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        public RelayCommand(Func<Task> executeAsync, Func<bool> canExecute = null!)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async void Execute(object? parameter)
        {
            if (!CanExecute(parameter))
                return;

            try
            {
                _isExecuting = true;
                CommandManager.InvalidateRequerySuggested();
                await _executeAsync();
            }
            finally
            {
                _isExecuting = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}