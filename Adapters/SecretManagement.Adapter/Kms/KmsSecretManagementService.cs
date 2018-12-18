using LambdaCore.Adapters;
using System.Threading.Tasks;

namespace SecretManagement.Adapter.Kms
{
    internal sealed class KmsSecretManagementService : ISecretManagementService
    {
        public Task<string> DecryptString(string value)
        {
            return Task.FromResult(value);
        }
    }
}
