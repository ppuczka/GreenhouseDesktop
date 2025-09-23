using GreenhouseDesktopApp.Models;
using GreenhouseDesktopApp.Services;
using System.Windows;

namespace GreenhouseDesktopApp
{
    public partial class MainWindow : Window
    {
        private IIotHubService _controllerService;
        private string _connectionString = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        //    _controllerService = new IotHubService();
         //   _controllerService.OnAlertReceived += ControllerService_OnAlertReceived;
          //  Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    ConnectionStatus.Text = "Connecting...";
            //    ConnectionStatus.Foreground = System.Windows.Media.Brushes.Orange;
            //    await _controllerService.SendStartSignalAsync();
            //    ConnectionStatus.Text = "Connected";
            //    ConnectionStatus.Foreground = System.Windows.Media.Brushes.Green;
            //}
            //catch
            //{
            //    ConnectionStatus.Text = "Disconnected";
            //    ConnectionStatus.Foreground = System.Windows.Media.Brushes.Red;
            //}
        }

        //private void ConnectButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var dialog = new ConnectionStringDialog(_connectionString);
        //    dialog.Owner = this;
        //    if (dialog.ShowDialog() == true)
        //    {
        //        _connectionString = dialog.ConnectionString;
        //        // TODO: Pass the connection string to your controller service as needed
        //        MessageBox.Show("Connection string updated.", "Info");
        //    }
        //}

        //private async void StartButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        await _controllerService.SendStartSignalAsync();
        //        MessageBox.Show("Start signal sent successfully", "Success");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Failed to send start signal: {ex.Message}", "Error");
        //    }
        //}

        //private async void StopButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        await _controllerService.SendStopSignalAsync();
        //        MessageBox.Show("Stop signal sent successfully", "Success");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Failed to send stop signal: {ex.Message}", "Error");
        //    }
        //}
    }
}