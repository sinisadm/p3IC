using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dropbox.API
{
    public class Program
    {
        //    public static void Main(string[] args)
        //    {
        //        CreateHostBuilder(args).Build().Run();
        //    }

        //    public static IHostBuilder CreateHostBuilder(string[] args) =>
        //        Host.CreateDefaultBuilder(args)
        //            .ConfigureWebHostDefaults(webBuilder =>
        //            {
        //                webBuilder.UseStartup<Startup>();
        //            });
        //}

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Verbose()
                        .Enrich.FromLogContext()
                        .CreateLogger();
            try
            {
                Log.Information("Application Starting.");
                var host = CreateHostBuilder(args).Build();
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
                {
                    await host.MigrateDatabase();
                }

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseShutdownTimeout(TimeSpan.FromSeconds(15));
                    webBuilder.ConfigureKestrel(options =>
                    {
                        var grpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT");
                    // Setup a HTTP/2 endpoint without TLS.
                    if (!string.IsNullOrWhiteSpace(grpcPort))
                        {
                            options.ListenLocalhost(Convert.ToInt32(grpcPort), o => o.Protocols = HttpProtocols.Http2);
                        }

                        var regexUrl = new Regex(@"^(?<proto>\w+)://[^/]+?(?<port>:\d+)?/?", RegexOptions.None);
                        Match m = regexUrl.Match(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? Environment.GetEnvironmentVariable("DOTNET_URLS"));
                        options.ListenAnyIP(m.Success ? Convert.ToInt32(m.Result("${port}").Replace(":", string.Empty)) : 5000, o => o.Protocols = HttpProtocols.Http1);
                    });
                });
        }
    }
}
