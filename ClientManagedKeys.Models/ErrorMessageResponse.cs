namespace ClientManagedKeys.Models
{
    public class ErrorMessageResponse
    {
        public ErrorMessageResponse(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}