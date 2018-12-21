using LambdaCore.Adapters;
using Microsoft.Extensions.DependencyInjection;
using SecretManagement.Adapter.InMemory;

#if !DEBUG
using Amazon.KeyManagementService;
using SecretManagement.Adapter.Kms;
#endif

namespace SecretManagement.Adapter
{
    public static class SecretManagementAdapter
    {
        public static IServiceCollection AddSecretManagementAdapter(this IServiceCollection serviceCollection)
        {
#if DEBUG
            serviceCollection.AddScoped<ISecretManagementService, InMemorySecretManagementService>();
#else
            serviceCollection.AddScoped<ISecretManagementService, KmsSecretManagementService>();
#endif

            return serviceCollection;
        }
    }
}