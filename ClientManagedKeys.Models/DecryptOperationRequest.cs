using System.ComponentModel.DataAnnotations;

namespace ClientManagedKeys.Models
{
    public class DecryptOperationRequest : BaseKeyOperationRequest
    {
        /// <summary>
        /// Algorithm for performing the decryption. sAll algorithms MUST be supported. See [RFC 7518](https://www.rfc-editor.org/rfc/rfc7518.txt), section 4.1.
        /// </summary>
        [Required]
        public JweAlgorithm Algorithm { get; set; }
        
        /// <summary>
        /// Payload bytes for the decryption request, base64 encoded.
        /// </summary>
        [Required]
        public override byte[] Payload { get; set; }
    }
}