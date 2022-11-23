using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Http.BatchFormatters;

namespace RecommendationService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.DurableHttpUsingFileSizeRolledBuffers(
                    requestUri: "http://commendie-elk-vm.westeurope.cloudapp.azure.com:5602/",
                    batchFormatter: new ArrayBatchFormatter(),
                    textFormatter: new ElasticsearchJsonFormatter(),
                    bufferBaseFileName: "Logs-Buffer/Buffer"
                )
                .WriteTo.Console()
                .Enrich.WithProperty("ServiceOfOrigin", "recommendation-service")
                .Enrich.FromLogContext()
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
