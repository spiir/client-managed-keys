using ClientManagedKeys.Models;

namespace ClientManagedKeys.Server.Services
{
    public interface IKeyService
    {
        bool HasKey(string keyId);
        byte[] Sign(string keyId, byte[] payload, JwsAlgorithm algorithm);
        byte[] Decrypt(string keyId, byte[] payload, JweAlgorithm algorithm);
    }
}