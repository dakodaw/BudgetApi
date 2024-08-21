using BudgetApi.BudgetGroups.Services;
using BudgetApi.Budgeting.Services;
using BudgetApi.BudgetTypes;
using BudgetApi.CopyTo.Services;
using BudgetApi.GiftCards.Services;
using BudgetApi.Incomes.Services;
using BudgetApi.Purchases.Services;
using BudgetApi.Settings.Services;
using BudgetApi.Shared;
using BudgetApi.Shared.AppSettings;
using BudgetApi.Shared.Authorization;
using BudgetApi.Users.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace BudgetApi;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        //var logger = app.Services.GetService<ILogger<Program>>();
        //logger.Log(LogLevel.Information, env.EnvironmentName);
        //Configuration = configuration;
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddControllers();

        var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
        services.AddSingleton<AppSettings>(appSettings);

        services.IncludeFirebaseAuth(appSettings);
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddControllers();
        services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "BudgetApi", Version = "v1" });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

        Budget.DB.Startup.ConfigureServices(services, Configuration);
        
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<IBudgetTypeService, BudgetTypeService>();
        services.AddScoped<IBudgetCopyToService, BudgetCopyToService>();
        services.AddScoped<IGiftCardService, GiftCardService>();
        services.AddScoped<IIncomeService, IncomeService>();
        services.AddScoped<IIncomeSourceService, IncomeSourceService>();
        services.AddScoped<IPurchasesService, PurchasesService>();
        services.AddScoped<ISettingsService, SettingsService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IGroupsService, GroupsService>();
        services.AddScoped<IBudgetAuthorizationService, BudgetAuthorizationService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppSettings appSettings)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "BudgetApi v1"));

        app.UseHttpsRedirection();

        app.UseCors(builder =>
        {
            builder
                //.WithOrigins(appSettings.AllowedHosts)
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
