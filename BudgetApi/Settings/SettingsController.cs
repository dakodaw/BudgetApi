using BudgetApi.Settings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApi.Settings
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        ISettingsService _settingsService;
        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        // TODO: Have general settings and user settings maybe?
        [HttpGet]
        [Route("")]
        public string GetSetting(string settingName)
        {
            return _settingsService.GetSetting(settingName);
        }
    }
}
