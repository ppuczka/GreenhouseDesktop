using GreenhouseDesktopApp.Components;
using GreenhouseDesktopApp.Models;
using GreenhouseDesktopApp.Services;
using GreenhouseDesktopApp.ViewModels;
using System.Windows;

namespace GreenhouseDesktopApp
{
    public partial class MainWindow : Window
    {
        private readonly IIotHubService _controllerService;
        private readonly AppSettingsModel _appSettings;
        private readonly AppSettingsViewModel _appSettingsViewModel;
        private string _connectionString = string.Empty;

        public MainWindow(IIotHubService controllerService, AppSettingsModel appSettings, AppSettingsViewModel appSettingsViewModel)
        {
            InitializeComponent();
            
            _controllerService = controllerService ?? throw new ArgumentNullException(nameof(controllerService));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _appSettingsViewModel = appSettingsViewModel ?? throw new ArgumentNullException(nameof(appSettingsViewModel));
            
            DataContext = _appSettingsViewModel;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Initialize settings
                await _appSettingsViewModel.InitializeAsync();
                
                // Update connection string from settings
                _connectionString = _appSettings.IotHubConnectionString;
                
                // Attempt to start service if connection string is available
                if (!string.IsNullOrEmpty(_connectionString))
                {
                    await _controllerService.SendStartSignalAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize application: {ex.Message}", "Initialization Error");
            }
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ConnectionStringDialog(_connectionString);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                _connectionString = dialog.ConnectionString;
                
                // Update settings model
                _appSettings.IotHubConnectionString = _connectionString;
                await _appSettings.SaveSettingsAsync();
                
                MessageBox.Show("Connection string updated and saved.", "Success");
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _controllerService.SendStartSignalAsync();
                MessageBox.Show("Start signal sent successfully", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send start signal: {ex.Message}", "Error");
            }
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _controllerService.SendStopSignalAsync();
                MessageBox.Show("Stop signal sent successfully", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send stop signal: {ex.Message}", "Error");
            }
        }
    }
}