using System;
using System.Linq;
using System.Threading.Tasks;
using ClientManagedKeys.Models;
using Microsoft.Extensions.Logging;

namespace ClientManagedKeys.Client
{
    public class App
    {
        // keyId, SHA1, lowercase hex of certificate thumbprint
        private const string KeyId = "0e4085c0ae94078ad8992e3fdf6ebffb707ffa8a";
        
        // various constants, used for demo purposes
        private const string ClientId = "client-9778a952-4f22-4a57-abaa-61f99e14869a";
        private const string ProviderId = "XX_DemoBank";
        private const string CorrelationId = "|9e74f0e5-efc4-41b5-86d1-3524a43bd891.bcec871c_1.";
        private const string Operation = "SupervisedLogin";
        
        private readonly ILogger<App> _logger;
        private readonly IApiClient _apiClient;

        public App(ILogger<App> logger, IApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task Run()
        {
            _logger.LogInformation("Starting sample client");

            // Testing signing vectors
            
            var signResponse = await _apiClient.Sign(KeyId, new SignOperationRequest
            {
                Algorithm = JwsAlgorithm.Rs256,
                ClientId = ClientId,
                ProviderId = ProviderId,
                CorrelationId = CorrelationId,
                Operation = Operation,
                Payload = Convert.FromBase64String("TkFHQ2xpZW50TWFuYWdlZEtleXNWZXJpZmljYXRpb24=")
            });

            var expectedSignResponse = Convert.FromBase64String(
                "fFTZOo2+Sa9xquCm7Kcy74T7OhS+DaSVh7cQVmXXVkHUXcwqiLldjKwqEOk9ux/DliZ3mMJT6xo7cc28rfWa+54hzhjEmIdkBos0ZUs+6YFezYcabzGlTmGPm6K5zLZqMGSwx2bvKThrCI0q7mv+Nc7jNctUZ2S5zei6HrCELXy2UR5zLcaZUBUAyECKl19hYzx2eilSCMy4dt2lp3QnR1b/KM/7HIgJLFdlDHDpbZNA0qwqvq3j8bzOjpXAK5W0SN/rkNRiKPT/1hXMpPEh75iR0rFyyM/oope7ccCJ3iCghVLZ8s7S5ulntjA2lPYTnfKMIJuoeKAqEuVJQxqUJA==");
            
            // Use timing safe version of .SequenceEqual for production code
            _logger.LogInformation("Sign match: {Assertion}", expectedSignResponse.SequenceEqual(signResponse));
            
            // Testing decryption vectors
            
            var decryptResponse = await _apiClient.Decrypt(KeyId, new DecryptOperationRequest
            {
                Algorithm = JweAlgorithm.RsaOaep256,
                ClientId = ClientId,
                ProviderId = ProviderId,
                CorrelationId = CorrelationId,
                Operation = Operation,
                Payload = Convert.FromBase64String("mpBi7+q9XvpDWm3VOnyU1si1lpIqEPw0lGfZrNiONpg5rhZADmoIYwf96wXyZKXggBsfxQNd54KBtZ2ZgfFvnR6ONmMDtQiohGJUA7lKShnFIgllC7sC+PgN2i+BEXTZoWEfXJz4NtYf+PlPKdfit63WGP5rvpJaRbo3/cn0JaPUOqIRwbkx6dd46dt1d+zFiu993SiTXm1LxvEw1ZFGf0fd110THskkXOcFWxJO1Yg9wtUMihrB0hOJ97Kfdt8CjOiMiIAVAIyqXCrQvLiNivVEAmorsRTu2OC832/EmfnTu+fdK5zgquFM9ujymyS3ZbcpMe96IJmZmxsu+DEm5w==")
            });

            var expectedDecryptResponse = Convert.FromBase64String("SGVsbG8gd29ybGQK");

            // Use timing safe version of .SequenceEqual for production code
            _logger.LogInformation("Decrypt match: {Assertion}", expectedDecryptResponse.SequenceEqual(decryptResponse));
            
            try
            {
                var invalidKeyResponse = await _apiClient.Sign("1234567890123456789012345678901234567890",
                    new SignOperationRequest
                    {
                        Algorithm = JwsAlgorithm.Rs256,
                        ClientId = ClientId,
                        ProviderId = ProviderId,
                        CorrelationId = CorrelationId,
                        Operation = Operation,
                        Payload = Convert.FromBase64String("TkFHQ2xpZW50TWFuYWdlZEtleXNWZXJpZmljYXRpb24=")
                    });
            }
            catch (ApiClientException exception) when (exception.Fault == ApiClientFault.NotFound)
            {
                _logger.LogInformation(exception, "Correctly caught ApiClientException");
            }

            _logger.LogInformation("Sample client completed");
        }
    }
}