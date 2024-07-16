using BudgetApi.Budgeting.Services;
using BudgetApi.BudgetTypes;
using BudgetApi.CopyTo.Services;
using BudgetApi.GiftCards.Services;
using BudgetApi.Incomes.Services;
using BudgetApi.Models;
using BudgetApi.Purchases.Services;
using BudgetApi.Settings.Services;
using BudgetApi.Shared;
using BudgetApi.Shared.AppSettings;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BudgetApi.Shared.Authentication;
using Firebase.Auth.Providers;
using Firebase.Auth;

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
            IncludeFirebaseAuth(services, Configuration);
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

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton<AppSettings>(appSettings);

            services.AddDbContext<BudgetEntities>(options =>
                options.UseSqlServer(Configuration.GetValue<string>("BudgetDB")));

            services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<IBudgetTypeService, BudgetTypeService>();
            services.AddScoped<IBudgetCopyToService, BudgetCopyToService>();
            services.AddScoped<IGiftCardService, GiftCardService>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IIncomeSourceService, IncomeSourceService>();
            services.AddScoped<IPurchasesService, PurchasesService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        private static void IncludeFirebaseAuth(IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(() => {
                return new FirebaseAuthClient(new FirebaseAuthConfig
                {
                    ApiKey = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_API_KEY"),
                    AuthDomain = config["Jwt:Firebase:AuthDomain"],
                    Providers = new FirebaseAuthProvider[]
                    {
                        new GoogleProvider().AddScopes("email"),
                        //new FacebookProvider(),
                        //new TwitterProvider(),
                        //new GithubProvider(),
                        //new MicrosoftProvider(),
                        new EmailProvider()
                    }
                });
            });

            services.AddSingleton(() => {
                var googleCredentialsFilePath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

                return FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(googleCredentialsFilePath)
                });
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Authority = config["Jwt:Firebase:ValidIssuer"];
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Firebase:ValidIssuer"],
                        ValidAudience = config["Jwt:Firebase:ValidAudience"]
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppSettings appSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                //.WithOrigins("http://localhost:4200", "https://localhost:4200")
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "BudgetApi v1"));

            app.UseCors(builder =>
            {
                builder
                .WithOrigins(appSettings.AllowedHosts)
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
