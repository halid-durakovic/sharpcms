using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace sharpcms.startup
{
    public class Host
    {
        public static void Run<TStartup>(string url, string[] args) where TStartup : AbstractStartup
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            if (args.Any())
            {
                var suggestedSubDirectory = args.First();

                currentDirectory = Path.Combine(currentDirectory, suggestedSubDirectory);
            }

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(url)
                .UseContentRoot(currentDirectory)
                .UseIISIntegration()
                .UseStartup(typeof(TStartup))
                .Build();

            host.Run();
        }
    }
}