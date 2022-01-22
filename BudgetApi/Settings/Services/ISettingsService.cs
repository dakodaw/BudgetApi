
using System.Collections.Generic;

namespace BudgetApi.Settings.Services
{
    public interface ISettingsService
    {
        string GetSetting(string settingName);
        Dictionary<string, string> GetAllSettings();
    }
}
