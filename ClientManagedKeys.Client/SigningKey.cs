using System.Security.Cryptography.X509Certificates;

namespace ClientManagedKeys.Client
{
    public class SigningKey
    {
        public X509Certificate2 Certificate { get; set; }
        public string KeyId { get; set; }
    }
}