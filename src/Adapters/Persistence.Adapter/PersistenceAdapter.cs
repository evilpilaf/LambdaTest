using LambdaCore.Adapters;

using Microsoft.Extensions.DependencyInjection;

using Persistence.Adapter.Oracle.Queries;

namespace Persistence.Adapter
{
    public static class PersistenceAdapter
    {
        public static IServiceCollection AddPersistenceAdapter(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IGetStockLocationsQuery, GetStockLocationsFromVanessa>();
            return serviceCollection;
        }
    }
}
