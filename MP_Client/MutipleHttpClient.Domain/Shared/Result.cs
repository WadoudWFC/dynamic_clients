namespace MutipleHttpClient.Domain;

public record Result<T>
{
    public T? Value { get; }
    public bool IsSuccess { get; }
    public Error? Error { get; }
    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
        Error = null;
    }
    private Result(Error error)
    {
        Value = default;
        IsSuccess = false;
        Error = error;
    }
    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);
}
