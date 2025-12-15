namespace AEChallenge.Application.DTOs.Response
{
    public class ApiResponseResult<T>(bool isSuccess, string message, T? data, string status)
    {

        public bool IsSuccess { get; } = isSuccess;
        public string Status { get; } = status;
        public string Message { get; } = message;
        public T? Data { get; } = data;



        public static ApiResponseResult<T> Success(T data, string message = "Success")
        {
            return new ApiResponseResult<T>(true, message, data, "Success");
        }

        public static ApiResponseResult<T> Created(T data, string message = "Created successfully")
        {
            return new ApiResponseResult<T>(true, message, data, "Created");
        }



        public static ApiResponseResult<T> Failure(string message, string status = "Error")
        {

            return new ApiResponseResult<T>(false, message, default, status);
        }

        public static ApiResponseResult<T> NotFound(string message = "Resource not found")
        {
            return new ApiResponseResult<T>(false, message, default, "NotFound");
        }

        public static ApiResponseResult<T> Invalid(string message = "Validation failed")
        {
            return new ApiResponseResult<T>(false, message, default, "BadRequest");
        }
    }
}
