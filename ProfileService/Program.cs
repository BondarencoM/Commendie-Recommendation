using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Http.BatchFormatters;

namespace ProfileService
{
    public class Program
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
                .Enrich.WithProperty("ServiceOfOrigin", "profile-service")
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
