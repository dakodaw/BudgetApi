using Budget.DB.Budget;
using Budget.DB.BudgetingGroups;
using Budget.DB.BudgetTypes;
using Budget.DB.CustomSettings;
using Budget.DB.GiftCards;
using Budget.DB.Incomes;
using Budget.DB.Users;
using BudgetApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.DB;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BudgetEntities>(options =>
            options.UseSqlServer(configuration.GetValue<string>("BudgetDB")));

        services.AddScoped<IIncomeProvider, IncomeProvider>();
        services.AddScoped<IIncomeSourceProvider, IncomeSourceProvider>();
        services.AddScoped<IPurchaseProvider, PurchaseProvider>();
        services.AddScoped<IBudgetProvider, BudgetProvider>();
        services.AddScoped<ICustomSettingsProvider, CustomSettingsProvider>();
        services.AddScoped<IGiftCardProvider, GiftCardProvider>();
        services.AddScoped<IBudgetTypeProvider, BudgetTypeProvider>();
        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<IBudgetingGroupProvider, BudgetingGroupProvider>();
        services.AddScoped<IUserBudgetingGroupProvider, UserBudgetingGroupProvider>();
    }
}
