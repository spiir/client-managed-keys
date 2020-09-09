using System;
using System.Diagnostics.CodeAnalysis;
using ClientManagedKeys.Models;
using ClientManagedKeys.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Annotations;

namespace ClientManagedKeys.Server.Controllers
{
    [ApiController]
    [Route("/v1")]
    [Authorize]
    public class KeyOperationController : ControllerBase
    {
        private readonly ILogger<KeyOperationController> _logger;
        private readonly IKeyService _keyService;
        private const string KeyIdMask = "{keyId:regex(^[[0-9a-fA-F]]{{40}}$)}";

        public KeyOperationController(ILogger<KeyOperationController> logger,
            IKeyService keyService)
        {
            _logger = logger;
            _keyService = keyService;
        }

        /// <summary>
        /// Sign operation
        /// </summary>
        /// <param name="keyId">SHA1 thumbprint of the certificate, lowercase hexadecimal.</param>
        [HttpPost(KeyIdMask + "/sign")]
        [SwaggerOperation("sign", Tags = new []{"Key operations"})]
        [ProducesResponseType(typeof(SignOperationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessageResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorMessageResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorMessageResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorMessageResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorMessageResponse), StatusCodes.Status429TooManyRequests)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<SignOperationResponse> Sign([FromRoute] string keyId, [FromBody] SignOperationRequest request)
        {
            if (request == null)
                return BadRequest(new ErrorMessageResponse("Could not parse body"));

            if (!_keyService.HasKey(keyId))
                return NotFound();

            byte[] payload;
            
            try
            {
                payload = _keyService.Sign(keyId, request.Payload, request.Algorithm);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Signing threw exception, returning bad request");
                return BadRequest();
            }

            
            return Ok(new SignOperationResponse
            {
                Payload = payload
            });
        }

        /// <summary>
        /// Decrypt operation
        /// </summary>
        /// <param name="keyId">SHA1 thumbprint of the certificate, lowercase hexadecimal.</param>
        [HttpPost(KeyIdMask + "/decrypt")]
        [SwaggerOperation("decrypt", Tags = new []{"Key operations"})]
        [ProducesResponseType(typeof(DecryptOperationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessageResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorMessageResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorMessageResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorMessageResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorMessageResponse), StatusCodes.Status429TooManyRequests)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<DecryptOperationResponse> Decrypt([FromRoute] string keyId, [FromBody] DecryptOperationRequest request)
        {
            if (request == null)
                return BadRequest(new ErrorMessageResponse("Could not parse body"));

            if (!_keyService.HasKey(keyId))
                return NotFound();

            byte[] payload;
            
            try
            {
                payload = _keyService.Decrypt(keyId, request.Payload, request.Algorithm);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Decryption threw exception, returning bad request");
                return BadRequest();
            }

            return Ok(new DecryptOperationResponse
            {
                Payload = payload
            });
        }
    }
}