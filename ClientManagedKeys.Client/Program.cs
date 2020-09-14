using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClientManagedKeys.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            
            var configuration = LoadConfiguration();

            services.AddSingleton(configuration);

            var startup = new Startup(configuration);

            startup.ConfigureServices(services);
            
            var serviceProvider = services.BuildServiceProvider();
            await serviceProvider.GetService<App>().Run();
            await Task.Delay(1);
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }
    }
}