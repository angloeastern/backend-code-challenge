namespace AEBackend.Controllers.Utils;
public class ApiResult
{
  public ApiResult(bool isSuccess, ApiError error)
  {
    IsSuccess = isSuccess;
    Error = error;
  }

  public bool IsSuccess { get; set; }
  public ApiError Error { get; set; }

  public static ApiResult Success() => new(true, ApiError.None);

  public static ApiResult Failure(ApiError error) => new(false, error);

  public static ApiResult<T> Success<T>(T data) => new(true, ApiError.None, data);

  public static ApiResult<T> Failure<T>(ApiError error) => new(false, error, default);
}

public class ApiResult<T> : ApiResult
{
  public T? Data { get; set; }

  public ApiResult(bool isSuccess, ApiError error, T? data) : base(isSuccess, error)
  {
    Data = data;
  }
}