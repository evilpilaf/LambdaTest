using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;

using LambdaCore.Adapters;

using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace SecretManagement.Adapter.Kms
{
    internal sealed class KmsSecretManagementService : ISecretManagementService
    {
        private readonly AmazonKeyManagementServiceClient _kmsClient;
        private readonly ILogger<KmsSecretManagementService> _logger;

        public KmsSecretManagementService(
            AmazonKeyManagementServiceClient kmsClient,
            ILogger<KmsSecretManagementService> logger)
        {
            _kmsClient = kmsClient;
            _logger = logger;
            _logger.LogDebug("KMS secret management service built");
        }

        public async Task<string> DecryptString(string value)
        {
            _logger.LogDebug("Decrypting value...");
            string decryptedString;
            using (var stream = new MemoryStream(Convert.FromBase64String(value)))
            {
                var decryptRequest = new DecryptRequest {
                    CiphertextBlob = stream
                };

                _logger.LogDebug("Decrypt request built");

                DecryptResponse response = await _kmsClient.DecryptAsync(decryptRequest);

                _logger.LogDebug("Decryption response received.");

                using (var reader = new StreamReader(response.Plaintext))
                {
                    decryptedString = await reader.ReadToEndAsync();
                }
                _logger.LogDebug("Decryption response read. Value decrypted.");
            }
            return decryptedString;
        }
    }
}