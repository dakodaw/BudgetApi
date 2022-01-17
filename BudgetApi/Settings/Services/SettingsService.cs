using Microsoft.Extensions.Configuration;

namespace BudgetApi.Settings.Services
{
    public class SettingsService: ISettingsService
    {
        IConfiguration _configuration;
        public SettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetEnvironmentName()
        {
            return _configuration.GetSection("EnvironmentName").Value;
        }
    }
}
