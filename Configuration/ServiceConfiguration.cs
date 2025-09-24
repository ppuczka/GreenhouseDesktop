using GreenhouseDesktopApp.Interfaces;
using GreenhouseDesktopApp.Models;
using GreenhouseDesktopApp.Services;
using GreenhouseDesktopApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GreenhouseDesktopApp.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Register services
            services.AddSingleton<IAppSettingsService>(provider => 
                new AppSettingsService("config.json"));

            // Register models
            services.AddSingleton<AppSettingsModel>();

            // Register view models
            services.AddTransient<AppSettingsViewModel>();

            return services;
        }

        public static IHost CreateHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.ConfigureServices();
                })
                .Build();
        }
    }
}