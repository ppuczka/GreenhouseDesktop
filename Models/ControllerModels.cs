namespace GreenhouseDesktopApp.Models;

public class ControllerCommand
{
    public string Command { get; set; } = string.Empty;
    public object? Payload { get; set; }
}

public class ControllerAlert
{
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
