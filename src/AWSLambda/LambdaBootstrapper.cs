using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                            .WriteTo.Console(new JsonFormatter(), LogEventLevel.Debug)
                            .CreateLogger();
            return new ServiceCollection()
                   .AddLogging(builder =>
                       builder.AddLambdaLogger().AddSerilog(logger: log, dispose: true))
                   .Configure<PersistenceAdapterSettings>(config.GetSection("Oracle"))
                   .AddSecretManagementAdapter()
                   .AddPersistenceAdapter()
                   .BuildServiceProvider();
        }
    }
}
