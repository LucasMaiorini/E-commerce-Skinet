namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            // ?? stands for Null Coalescing operator. It means that if the message is null,
            // execute the method that follows
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request issue :(",
                401 => "An authorization issue :(",
                404 => "A not found resource issue :(",
                500 => "An internal server error issue :(",
                _ => null
            };
        }
    }
}