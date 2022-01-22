using BudgetApi.Models;
using BudgetApi.Settings.Cache;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.Settings.Services
{
    public class SettingsService: ISettingsService
    {
        public SettingsService(IConfiguration configuration, BudgetEntities db)
        {
            var settings = db.Settings.ToList();
            foreach (var setting in settings)
            {
                SettingsCache.AddSetting(setting.KeyName, setting.Value);
            }
        }

        public string GetSetting(string environmentName)
        {
            return SettingsCache.GetSetting(environmentName);
        }

        public Dictionary<string, string> GetAllSettings()
        {
            return SettingsCache.GetAllSettings();
        }
    }
}
