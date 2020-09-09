*Client Managed Keys reference server implementation*
 
**Response status codes**

Implementations MUST use HTTP status codes as the primary means of communicating success / failure.

*Successful codes*
- 200 OK
 - Returns payload

*Failure codes*
- 400 Bad Request
 - Missing required properties from request
 - Malformed payload
 - Invalid payload for operation
- 401 Unauthorized
 - Invalid or missing API-key provided
- 403 Forbidden 
 - Operation rejected, eg. due to audit checks of correlation IDs
- 404 Not Found
 - Invalid keyId
 - KeyId not known to server
- 429 Too Many Requests 
 - Rate limit reached, clients should retry

Implementations SHOULD populate the `message` property of the error message when possible.
 
**Operational requirements**
 
All servers implementing this specification MUST NOT reject signing requests for the specific payload:
 
`TkFHQ2xpZW50TWFuYWdlZEtleXNWZXJpZmljYXRpb24=` (in ASCII `NAGClientManagedKeysVerification`)