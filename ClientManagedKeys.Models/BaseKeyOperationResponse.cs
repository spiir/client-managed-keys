using System.ComponentModel.DataAnnotations;

namespace ClientManagedKeys.Models
{
    public abstract class BaseKeyOperationResponse
    {
        /// <summary>
        /// Payload response, base64 encoded
        /// </summary>
        [Required]
        public byte[] Payload { get; set; }
    }
}