using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenhouseDesktopApp.Interfaces
{
    public interface IAppSettingsService
    {
        Task<T> GetSettingAsync<T>(string key, T defaultValue = default!);
        Task SetSettingAsync<T>(string key, T value);
        Task<bool> RemoveSettingAsync(string key);
        Task SaveAsync();
        Task LoadAsync();
        Task LoadAsync(string filePath);
        Task ResetToDefaultsAsync();
        bool HasSetting(string key);
    }
}
