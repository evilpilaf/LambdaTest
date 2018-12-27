using Dapper;
using LambdaCore.Adapters;
using LambdaCore.Entities;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Persistence.Adapter.Oracle.Queries
{
    internal sealed class GetStockLocationsFromVanessa : IGetStockLocationsQuery
    {
        private readonly PersistenceAdapterSettings _options;
        private readonly ISecretManagementService _secretManagementService;
        private readonly ILogger<GetStockLocationsFromVanessa> _logger;

        public GetStockLocationsFromVanessa(
            ISecretManagementService secretManagementService,
            IOptions<PersistenceAdapterSettings> adapterOptions,
            ILogger<GetStockLocationsFromVanessa> logger)
        {
            _secretManagementService = secretManagementService;
            _logger = logger;
            _options = adapterOptions.Value;
            _logger.LogDebug("Query built.");
        }

        public async Task<IEnumerable<StockLocationAddress>> GetStockLocations()
        {
            var builder = new OracleConnectionStringBuilder {
                DataSource = _options.DataSource,
                UserID = _options.UserId,
                Password = await GetDecryptedPassword(),
                LoadBalancing = _options.LoadBalancing
            };

            _logger.LogDebug("Connection string built");

            using (var cnn = new OracleConnection(builder.ConnectionString))
            {
                await cnn.OpenAsync();
                _logger.LogDebug("DB connection open");
                return await cnn.QueryAsync<StockLocationAddress>(_query);
            }
        }

        private async Task<string> GetDecryptedPassword()
        {
            _logger.LogDebug("Requesting password decryption.");
            return await _secretManagementService.DecryptString(_options.Password);
        }

        private const string _query =
            "select"
            + "  sl.STOCKLOCATIONID,"
            + "  sla.STOCKLOCATIONADDRESSID,"
            + "  sla.ADDRESSDESCRIPTION"
            + " from"
            + "   VAN_STOCKLOCATION sl"
            + "   inner join VAN_STOCKLOCATIONSTOCKCLUSTER slsc on slsc.STOCKLOCATIONID = sl.STOCKLOCATIONID"
            + "   inner join VAN_COMPANY c on c.MAINDISTRISTOCKCLUSTERID = slsc.STOCKCLUSTERID"
            + "   inner join VAN_STOCKLOCATIONADDRESS sla on sl.STOCKLOCATIONID = sla.STOCKLOCATIONID"
            + " where"
            + "   c.COMPANYID = 1 AND sla.CITY = 'Tilburg'"
            + "   order by"
            + "   sl.STOCKLOCATIONID";
    }
}
