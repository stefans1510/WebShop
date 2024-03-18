
namespace API.Errors
{
    public class ApiValidationErrorrResponse : ApiResponse
    {
        public ApiValidationErrorrResponse() : base(400)
        {
            
        }

        public IEnumerable<String> Errors { get; set; }
    }
}