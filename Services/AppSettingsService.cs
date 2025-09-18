using GreenhouseDesktopApp.Interfaces;

namespace GreenhouseDesktopApp.Services
{
    class AppSettingsService : IAppSettingsService
    {
        private static string ConfigFilePath = "config.json";

        public string GetSetting(string key)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> LoadAppSettings()
        {
            throw new NotImplementedException();
        }

        public void SaveAppSettings(Dictionary<string, string> settings)
        {
            throw new NotImplementedException();
        }
    }
}
