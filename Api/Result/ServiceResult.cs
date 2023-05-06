using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.Enums;

namespace WebAPI.Result;

public sealed class ServiceResult : IServiceResult
{
    public bool IsSuccess { get; private init; }

    public Result? Result { get; private init; }

    public ErrorResult? ErrorResult { get; private init; }

    public static IServiceResult Ok()
        => new ServiceResult { IsSuccess = true, Result = Result.Ok() };

    public static IServiceResult Created()
        => new ServiceResult { IsSuccess = true, Result = Result.Created() };

    public static IServiceResult Accepted()
        => new ServiceResult { IsSuccess = true, Result = Result.Accepted() };

    public static IServiceResult NoContent()
        => new ServiceResult { IsSuccess = true, Result = Result.NoContent() };

    public static IServiceResult SystemError(ErrorModel data)
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.SystemError(data) };

    public static IServiceResult GlobalSystemError()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.GlobalSystemError() };

    public static IServiceResult Unauthorized()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.UnauthorizedError() };

    public static IServiceResult BusinessError(ErrorModel data)
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.BusinessError(data) };

    public static IServiceResult GlobalBusinessError()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.GlobalBusinessError() };

    public static IServiceResult NotFound()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.NotFoundError() };

    public static IServiceResult AlreadyExistsError()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.AlreadyExistsError() };

    public static IServiceResult ForbiddenError()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.ForbiddenError() };
}

[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
public sealed class ServiceResult<T> : IServiceResult<T> where T : class
{
    [BindNever, Newtonsoft.Json.JsonIgnore, JsonIgnore]
    Result? IServiceResult.Result { get; }

    public bool IsSuccess { get; private init; }

    public Result<T>? Result { get; private init; }

    public ErrorResult? ErrorResult => null;

    public static IServiceResult<T> Ok(T? data)
        => new ServiceResult<T> { IsSuccess = true, Result = Result<T>.Ok(data) };

    public static IServiceResult<T> Created(T? data)
        => new ServiceResult<T> { IsSuccess = true, Result = Result<T>.Created(data) };

    public static IServiceResult<T> Accepted(T? data)
        => new ServiceResult<T> { IsSuccess = true, Result = Result<T>.Accepted(data) };

    public static IServiceResult<T> NoContent(T? data)
        => new ServiceResult<T> { IsSuccess = true, Result = Result<T>.NoContent(data) };
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public sealed class Result
{
    public HttpStatus Status { get; private init; }

    internal static Result Ok()
        => new Result { Status = HttpStatus.Ok };

    internal static Result Created()
        => new Result { Status = HttpStatus.Created };

    internal static Result Accepted()
        => new Result { Status = HttpStatus.Accepted };

    internal static Result NoContent()
        => new Result { Status = HttpStatus.NoContent };
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public sealed class Result<T> where T : class
{
    public HttpStatus Status { get; private init; }

    public T? Data { get; private init; }

    internal static Result<T> Ok(T? data)
        => new Result<T> { Status = HttpStatus.Ok, Data = data };

    internal static Result<T> Created(T? data)
        => new Result<T> { Status = HttpStatus.Created, Data = data };

    internal static Result<T> Accepted(T? data)
        => new Result<T> { Status = HttpStatus.Accepted, Data = data };

    internal static Result<T> NoContent(T? data)
        => new Result<T> { Status = HttpStatus.NoContent, Data = data };
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public sealed class ErrorResult
{
    public HttpStatus Status { get; private init; }

    public ErrorModel? Data { get; private init; }

    internal static ErrorResult SystemError(ErrorModel? data)
        => new ErrorResult { Status = HttpStatus.Internal, Data = data };

    internal static ErrorResult GlobalSystemError()
        => SystemError(new ErrorModel { Code = ErrorCodes.SystemInternalServerError, Message = ExceptionConstants.CommonErrorMessage });

    internal static ErrorResult UnauthorizedError()
        => new ErrorResult { Status = HttpStatus.Unauthorized, Data = new ErrorModel { Code = ErrorCodes.UnauthorizedError } };

    internal static ErrorResult BusinessError(ErrorModel? data)
        => new ErrorResult { Status = HttpStatus.BadRequest, Data = data };

    internal static ErrorResult GlobalBusinessError()
        => BusinessError(new ErrorModel { Code = ErrorCodes.BusinessInternalServerError, Message = ExceptionConstants.CommonErrorMessage });

    internal static ErrorResult NotFoundError()
        => new ErrorResult { Status = HttpStatus.NotFound, Data = new ErrorModel { Code = ErrorCodes.NotFoundError } };

    internal static ErrorResult AlreadyExistsError()
        => new ErrorResult { Status = HttpStatus.BadRequest, Data = new ErrorModel { Code = ErrorCodes.AlreadyExistsError } };

    internal static ErrorResult ForbiddenError()
        => new ErrorResult { Status = HttpStatus.Forbidden, Data = new ErrorModel { Code = ErrorCodes.ForbiddenError } };
}

public static class ServiceResultFactory
{
    public static IServiceResult CreateFromException(System.Exception exception)
    {
        return exception switch
        {
            ForbiddenException => ServiceResult.ForbiddenError(),
            NotFoundException => ServiceResult.NotFound(),
            AlreadyExistsException => ServiceResult.AlreadyExistsError(),
            OperationalException OperationalException => ServiceResult.SystemError(new ErrorModel { Code = OperationalException.Code }),
            BusinessException BusinessException => ServiceResult.BusinessError(new ErrorModel { Code = BusinessException.Code }),
            _ => ServiceResult.GlobalSystemError()
        };
    }
}

public static class ServiceResultHelper
{
    public static UnauthorizedResult Unauthorized(string message, ILogger logger)
    {
        logger.LogError("{Message}", message);
        return new UnauthorizedResult();
    }
}