using System.Collections.Generic;
using System.Security.Cryptography;

namespace ClientManagedKeys.Server
{
    public interface IKeyProvider
    {
        Dictionary<string, RSA> GetKeys();
    }
}