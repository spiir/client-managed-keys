using System.ComponentModel.DataAnnotations;

namespace ClientManagedKeys.Models
{
    public abstract class BaseKeyOperationRequest
    {
        /// <summary>
        /// Hierarchical Request-Id, see [specification](https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/HierarchicalRequestId.md).
        /// </summary>
        [Required]
        public string CorrelationId { get; set; }
        
        /// <summary>
        /// Provider ID for the request.
        /// </summary>
        [Required]
        public string ProviderId { get; set; }
        
        /// <summary>
        /// Client ID of the client performing the request.
        /// </summary>
        [Required]
        public string ClientId { get; set; }
        
        /// <summary>
        /// Payload request, base64 encoded.
        /// </summary>
        [Required]
        public byte[] Payload { get; set; }
        
        /// <summary>
        /// Operation triggering the request, for logging purposes.
        /// </summary>
        [Required]
        public string Operation { get; set; }
    }
}    