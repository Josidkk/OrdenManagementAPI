namespace OrderManagementAPI.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public T Data { get; set; }

        public ApiResponse() { }

     
        public static ApiResponse<T> SuccessResponse(T data, string message = "")
            => new ApiResponse<T> { Success = true, Data = data, Message = message };

      
        public static ApiResponse<T> ErrorResponse(string message, List<string> errors)
            => new ApiResponse<T> { Success = false, Message = message, Errors = errors };
    }
}