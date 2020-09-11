using System;
using System.Threading.Tasks;
using ClientManagedKeys.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClientManagedKeys.Client
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly IApiClient _apiClient;

        public App(ILogger<App> logger, IApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task Run()
        {
            _logger.LogInformation("Hello world");

            var response = await _apiClient.Sign("0e4085c0ae94078ad8992e3fdf6ebffb707ffa8a", new SignOperationRequest
            {
                Algorithm = JwsAlgorithm.Rs256,
                ClientId = "client-9778a952-4f22-4a57-abaa-61f99e14869a",
                ProviderId = "XX_DemoBank",
                CorrelationId = "|9e74f0e5-efc4-41b5-86d1-3524a43bd891.bcec871c_1.",
                Operation = "SupervisedLogin",
                Payload = Convert.FromBase64String("TkFHQ2xpZW50TWFuYWdlZEtleXNWZXJpZmljYXRpb24=")
            });

        }
    }
}