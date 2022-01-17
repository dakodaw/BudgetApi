using BudgetApi.Settings.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApi.Settings
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        ISettingsService _settingsService;
        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        [Route("")]
        public string GetEnvironmentName()
        {
            return _settingsService.GetEnvironmentName();
        }
    }
}
