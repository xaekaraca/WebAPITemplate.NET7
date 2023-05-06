namespace WebAPI.Result;

/// <summary>
/// Represents the result of a service operation.
/// </summary>
public interface IServiceResult
{
    public bool IsSuccess { get; }

    public Result? Result { get; }

    public ErrorResult? ErrorResult { get; }
}

/// <summary>
/// Represents the result of a service operation.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IServiceResult<T> : IServiceResult where T : class
{
    public new Result<T>? Result { get; }
}