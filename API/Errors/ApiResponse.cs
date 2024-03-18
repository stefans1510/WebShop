
namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string messsage = null)
        {
            StatusCode = statusCode;
            Message = messsage ?? GetDefaultMessgeForStatusCode(statusCode);

        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        
        private string GetDefaultMessgeForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad request",
                401 => "Not authorized",
                404 => "No resource found",
                500 => "TextTextTextTextTextTextTextTextTextTextTextTextTextText",
                _ => null
            };
        }
    }
}