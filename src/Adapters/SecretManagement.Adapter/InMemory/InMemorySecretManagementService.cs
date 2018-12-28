using System.Threading.Tasks;

using LambdaCore.Adapters;

namespace SecretManagement.Adapter.InMemory
{
    internal sealed class InMemorySecretManagementService : ISecretManagementService
    {
        public Task<string> DecryptString(string value)
        {
            return Task.FromResult(value);
        }
    }
}