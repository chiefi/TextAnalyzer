using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Threading.Tasks;
using TextAnalyzer.Parser;
using TextAnalyzer.Printer;
using TextAnalyzer.Reader;

namespace TextAnalyzer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = AppStartup();

            var analyzer = ActivatorUtilities.CreateInstance<DocumentAnalyzer>(host.Services);
            
            await CommandLine.Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async o =>
            {
                await analyzer.AnalyzeAsync(o.Sources, o.CountForMostFrequentWords, o.CountForLongestWords);
            });
        }

        static IHost AppStartup()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();
            
            var host = Host.CreateDefaultBuilder()
                        .ConfigureServices((context, services) =>
                        {
                            services.AddScoped<IDocumentReader, DocumentReader>();
                            services.AddScoped<IDocumentParser, DocumentParser>();
                            services.AddScoped<IResultPrinter, ResultPrinter>();
                            services.AddScoped<IFilterService, FilterService>();
                        })
                        .UseSerilog()
                        .Build();

            return host;
        }
    }
}
