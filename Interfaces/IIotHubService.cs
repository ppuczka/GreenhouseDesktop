using GreenhouseDesktopApp.Models;

namespace GreenhouseDesktopApp.Services;

public interface IIotHubService
{
    Task SendStartSignalAsync();

    Task SendStopSignalAsync();

    Task SendCommandAsync(ControllerCommand command);
    
    event EventHandler<ControllerAlert> OnAlertReceived;
    
    bool IsConnected { get; }
}
