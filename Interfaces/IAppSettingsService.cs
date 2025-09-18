using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenhouseDesktopApp.Interfaces
{
    interface IAppSettingsService
    {
        Dictionary<string, string> LoadAppSettings();

        void SaveAppSettings(Dictionary<string, string> settings);

        string GetSetting(string key);
    }
}
