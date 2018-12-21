using LambdaCore.Adapters;

using Microsoft.Extensions.DependencyInjection;

using SecretManagement.Adapter.InMemory;

namespace SecretManagement.Adapter
{
    public static class SecretManagementAdapter
    {
        public static IServiceCollection AddSecretManagementAdapter(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ISecretManagementService, InMemorySecretManagementService>();
            return serviceCollection;
        }
    }
}
