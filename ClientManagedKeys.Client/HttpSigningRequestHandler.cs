using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dalion.HttpMessageSigning.Signing;

namespace ClientManagedKeys.Client
{
    public class HttpSigningRequestHandler : DelegatingHandler {
        private readonly IRequestSignerFactory _requestSignerFactory;
        private readonly SigningKey _signingKey;

        public HttpSigningRequestHandler(IRequestSignerFactory requestSignerFactory, SigningKey signingKey)
        {
            _requestSignerFactory = requestSignerFactory;
            _signingKey = signingKey;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            var requestSigner = _requestSignerFactory.CreateFor(_signingKey.KeyId);
            await requestSigner.Sign(request);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}