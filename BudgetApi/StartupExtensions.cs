using BudgetApi.Shared.AppSettings;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace BudgetApi;

public static class StartupExtensions
{
    public static IServiceCollection IncludeFirebaseAuth(this IServiceCollection services, AppSettings config)
    {
        services.AddSingleton(() => {
            return new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_API_KEY"),
                AuthDomain = config.Auth.AuthDomain,
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

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = config.Auth.ValidIssuer;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.Auth.ValidIssuer,
                    ValidAudience = config.Auth.ValidAudience
                };
            });

        return services;
    }
}
