using System.Threading.Tasks;

namespace LambdaCore.Adapters
{
    public interface ISecretManagementService
    {
        Task<string> DecryptString(string value);
    }
}
