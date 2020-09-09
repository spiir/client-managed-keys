using System.ComponentModel;
using System.Runtime.Serialization;

namespace ClientManagedKeys.Models
{
    /// <summary>
    /// A subset of algorithms defined in RFC 7518, section 3.1
    /// </summary>
    public enum JwsAlgorithm
    {
        /// <summary>
        /// RSASSA-PKCS1-v1_5 using SHA-256
        /// </summary>
        [EnumMember(Value = "RS256")]
        Rs256,
        
        /// <summary>
        /// RSASSA-PSS using SHA-256 and MGF1 with SHA-256
        /// </summary>
        [EnumMember(Value = "PS256")]
        Ps256
    }
}