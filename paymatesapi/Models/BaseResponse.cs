namespace paymatesapi.Models
{
    public class Error 
    {
        public string? Message { get; set; }
    }

    public class BaseResponse<T>
    {
        public T? Data { get; set; }
        public Error? Error { get; set; }
    }
}