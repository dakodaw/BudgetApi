using BudgetApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.DB;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BudgetEntities>(options =>
            options.UseSqlServer(configuration.GetValue<string>("BudgetDB")));
    }
}
