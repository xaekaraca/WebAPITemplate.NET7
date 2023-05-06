namespace WebAPI.Result;

public interface IServiceResult
{
    public bool IsSuccess { get; }

    public Result? Result { get; }

    public ErrorResult? ErrorResult { get; }
}

public interface IServiceResult<T> : IServiceResult where T : class
{
    public new Result<T>? Result { get; }
}