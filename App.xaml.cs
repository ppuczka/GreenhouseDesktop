using GreenhouseDesktopApp.Configuration;
using GreenhouseDesktopApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace GreenhouseDesktopApp;

public partial class App : Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Create and configure the host
        _host = ServiceConfiguration.CreateHost();
        
        // Configure additional services
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        
        // Build service provider
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Start the host
        await _host.StartAsync();

        // Show main window
        var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();

        // Configure all services using the extension method
        services.ConfigureServices();

        // Add windows
        services.AddSingleton<MainWindow>();

        // Add other services
        services.AddSingleton<IIotHubService, IotHubService>();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host != null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
        
        base.OnExit(e);
    }
}


