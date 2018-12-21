using LambdaCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Adapter;
using SecretManagement.Adapter;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;
using System.IO;

namespace AWSLambda
{
    internal static class LambdaBootstrapper
    {
        public static IConfigurationRoot GetConfiguration()
            => new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

        public static IServiceProvider GetDefaultServiceProvider()
        {
            IConfigurationRoot config = GetConfiguration();
            var log = new LoggerConfiguration()
                      .Enrich.FromLogContext()
                      .MinimumLevel.Verbose()
                      .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                      .WriteTo.Console(new JsonFormatter())
                      .CreateLogger();

            

            return new ServiceCollection()
                   .AddLogging(builder =>
                       builder.AddSerilog(logger: log, dispose: true))
                   .Configure<PersistenceAdapterSettings>(config.GetSection("Oracle"))
                   .AddScoped<UseCase>()
                   .AddSecretManagementAdapter()
                   .AddPersistenceAdapter()
                   .BuildServiceProvider();
        }
    }
}
