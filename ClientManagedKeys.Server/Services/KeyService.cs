using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using ClientManagedKeys.Models;

namespace ClientManagedKeys.Server.Services
{
    public class KeyService : IKeyService
    {
        private readonly Dictionary<string, RSA> _keys;

        public KeyService(IKeyProvider keyProvider)
        {
            _keys = keyProvider.GetKeys();
        }
        
        public bool HasKey(string keyId)
        {
            return _keys.ContainsKey(keyId);
        }

        public byte[] Sign(string keyId, byte[] payload, JwsAlgorithm algorithm)
        {
            HashAlgorithmName hashAlgorithm;
            RSASignaturePadding padding;

            switch (algorithm)
            {
                case JwsAlgorithm.Rs256:
                    hashAlgorithm = HashAlgorithmName.SHA256;
                    padding = RSASignaturePadding.Pkcs1;
                    break;

                case JwsAlgorithm.Ps256:
                    hashAlgorithm = HashAlgorithmName.SHA256;
                    padding = RSASignaturePadding.Pss;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
            }
            
            return _keys[keyId].SignHash(payload, hashAlgorithm, padding);
        }

        public byte[] Decrypt(string keyId, byte[] payload, JweAlgorithm algorithm)
        {
            RSAEncryptionPadding padding;

            switch (algorithm)
            {
                case JweAlgorithm.Rsa15:
                    padding = RSAEncryptionPadding.Pkcs1;
                    break;
                case JweAlgorithm.RsaOaep:
                    padding = RSAEncryptionPadding.OaepSHA1;
                    break;
                case JweAlgorithm.RsaOaep256:
                    padding = RSAEncryptionPadding.OaepSHA256;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
            }
            return _keys[keyId].Decrypt(payload, padding);
        }
    }
}