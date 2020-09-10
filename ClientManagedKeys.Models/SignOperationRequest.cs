using System.ComponentModel.DataAnnotations;

namespace ClientManagedKeys.Models
{
    public class SignOperationRequest : BaseKeyOperationRequest
    {
        /// <summary>
        /// Algorithm for performing the signature. All algorithms MUST be supported. See [RFC 7518](https://www.rfc-editor.org/rfc/rfc7518.txt), section 3.1.
        /// </summary>
        [Required]
        public JwsAlgorithm Algorithm { get; set; }
        
        /// <summary>
        /// Hashed bytes for the signature, output from the hash algorithm underlying the signature scheme, base64 encoded.
        /// </summary>
        [Required]
        public override byte[] Payload { get; set; }
    }
}