using System.ComponentModel;
using System.Runtime.Serialization;

namespace ClientManagedKeys.Models
{
    // 
    /// <summary>
    /// A subset of algorithms defined in RFC 7518, section 4.1
    /// </summary>
    public enum JweAlgorithm
    {
        /// <summary>
        /// RSAES-PKCS1-v1_5
        /// </summary>
        [EnumMember(Value = "RSA1_5")]
        Rsa15,

        /// <summary>
        /// RSAES OAEP using default parameters
        /// </summary>
        [EnumMember(Value = "RSA-OAEP")]
        RsaOaep,
        
        /// <summary>
        /// RSAES OAEP using SHA-256 and MGF1
        /// </summary>
        [EnumMember(Value = "RSA-OAEP-256")]
        RsaOaep256
    }
}