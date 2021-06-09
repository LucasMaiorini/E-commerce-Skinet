namespace API.Errors
{
    /// <summary>
    /// This class returns a custom error to exceptions.
    /// The class extends from ApiResponse, but this one includes the details, i.e., the STACKTRACE. 
    /// </summary>
    public class ApiException : ApiResponse
    {  
        //It's necessary a middleware (in Middleware Folder) to make it works when an exception is called.
        public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            //Details is the StackTrace
            Details = details;
        }

        public string Details { get; set; }
    }
}