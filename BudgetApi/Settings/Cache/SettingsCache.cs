using System.Collections.Generic;

namespace BudgetApi.Settings.Cache
{
    public static class SettingsCache
    {
        private static Dictionary<string, string> Value { get; set; } = new();
        
        public static string GetSetting(string key)
        {
            return Value.GetValueOrDefault(key);
        }

        public static void AddSetting(string key, string value)
        {
            Value.TryAdd(key, value);
        }

        public static Dictionary<string, string> GetAllSettings()
        {
            return Value;
        }
    }
}
