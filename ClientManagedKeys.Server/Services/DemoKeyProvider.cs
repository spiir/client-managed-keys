using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ClientManagedKeys.Server
{
    public class DemoKeyProvider : IKeyProvider
    {
        private readonly string _keyId;
        private readonly RSA _rsa;

        public DemoKeyProvider()
        {
            var pfxBytes = typeof(DemoKeyProvider).Assembly.GetResourceAsBytes("DemoKey.pfx");
            var password = "test";
            
            var cert = new X509Certificate2(pfxBytes, password);
            
            _keyId = cert.Thumbprint.ToLowerInvariant();
            _rsa = (RSA)cert.PrivateKey;
        }

        public Dictionary<string, RSA> GetKeys()
        {
            return new Dictionary<string, RSA>
            {
                [_keyId] = _rsa
            };
        }
        
    }
}