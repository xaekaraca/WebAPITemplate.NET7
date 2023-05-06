using WebAPI.Enums;

namespace WebAPI.Result;

public static class ServiceResultExtensions
{
    public static ApiResult ApiResult(this IServiceResult serviceResult, ApiResult.Location? location = null)
    {
        return serviceResult.IsSuccess
            ? new ApiResult((int)(serviceResult.Result?.Status ?? HttpStatus.Accepted), null, location)
            : new ApiResult((int)(serviceResult.ErrorResult?.Status ?? HttpStatus.Internal), serviceResult.ErrorResult?.Data?.Message);
    }

    public static ApiResult ApiResult<T>(this IServiceResult<T> serviceResult, ApiResult.Location? location = null) where T : class
    {
        return serviceResult.IsSuccess
            ? new ApiResult((int)(serviceResult.Result?.Status ?? HttpStatus.Accepted), serviceResult.Result?.Data, location)
            : new ApiResult((int)(serviceResult.ErrorResult?.Status ?? HttpStatus.Internal), serviceResult.ErrorResult?.Data?.Message);
    }
}