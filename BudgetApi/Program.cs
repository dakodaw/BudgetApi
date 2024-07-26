using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace BudgetApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);

            host
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                    config.AddJsonFile("appsettings.json");
                    config.AddJsonFile("appsettings.Development.json", optional: true);
                    config.AddJsonFile("appsettings.Production.json", optional: true);
                    config.AddEnvironmentVariables();
                });

            return host;
        }
    }
}
