using BudgetApi.Budgeting.Services;
using BudgetApi.BudgetTypes;
using BudgetApi.CopyTo.Services;
using BudgetApi.GiftCards.Services;
using BudgetApi.Incomes.Services;
using BudgetApi.Models;
using BudgetApi.Purchases.Services;
using BudgetApi.Settings.Services;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BudgetApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BudgetApi", Version = "v1" });
            });
            services.AddDbContext<BudgetEntities>(options =>
                options.UseSqlServer(Configuration.GetValue<string>("BudgetDB")));
            
            services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<IBudgetTypeService, BudgetTypeService>();
            services.AddScoped<IBudgetCopyToService, BudgetCopyToService>();
            services.AddScoped<IGiftCardService, GiftCardService>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IPurchasesService, PurchasesService>();
            services.AddScoped<ISettingsService, SettingsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "BudgetApi v1"));

            app.UseCors(builder =>
            {
                builder
                .WithOrigins("https://localhost:4200", "http://localhost:8080")
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
