using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using ClientManagedKeys.Server;
using Dalion.HttpMessageSigning;
using Dalion.HttpMessageSigning.Signing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClientManagedKeys.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var key = GetSigningKey();

            services.AddLogging(
                o => o.AddConsole()    
            );
            
            services.AddTransient<App>();
            
            services.AddSingleton(key);
            services.AddTransient<HttpSigningRequestHandler>();
            services.AddSingleton<IApiClient, ApiClient>();

            services.AddHttpMessageSigning(
                    new KeyId(key.KeyId),
                    provider => new SigningSettings
                    {
                        SignatureAlgorithm = SignatureAlgorithm.CreateForSigning(key.Certificate, HashAlgorithmName.SHA256),
                        DigestHashAlgorithm = HashAlgorithmName.SHA256,
                        EnableNonce = false, // allow for retries
                        Expires = TimeSpan.FromMinutes(1),
                        RequestTargetEscaping = RequestTargetEscaping.RFC3986
                    })
                .AddHttpClient<IApiClient, ApiClient>(o => o.BaseAddress = new Uri(Configuration["BaseUri"]))
                .AddHttpMessageHandler<HttpSigningRequestHandler>();

        }

        private static SigningKey GetSigningKey()
        {
            var pfxFile = typeof(Startup).Assembly.GetResourceAsBytes("DemoAuthKey.pfx");
            var pfxPassword = "test";
            var cert = new X509Certificate2(pfxFile, pfxPassword, X509KeyStorageFlags.Exportable);
            var keyId = cert.Thumbprint?.ToLowerInvariant();

            return new SigningKey
            {
                Certificate = cert,
                KeyId = keyId
            };
        }
    }
}