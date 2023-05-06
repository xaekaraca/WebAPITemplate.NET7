using System.Diagnostics.CodeAnalysis;
using Core.Environment;
using Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Enums;

namespace WebAPI.Result
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/error")]
        [SuppressMessage("ReSharper", "InvertIf")]
        public IActionResult Error([FromServices] IConfiguration configuration)
        {
            IServiceResult serviceResult;

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (context is not null)
            {
                serviceResult = ServiceResultFactory.CreateFromException(context.Error);
                if (serviceResult.ErrorResult?.Data != null)
                {
                    var traceId = serviceResult.ErrorResult.Data.TraceId;
                    _logger.LogError(context.Error, "{CommonErrorTitle}. TraceId: {TraceId}", traceId, ExceptionConstants.CommonErrorMessage);

                    if (!configuration.IsSensitiveEnvironment())
                    {
                        serviceResult.ErrorResult.Data.Detail = context.Error.ToString();
                    }
                }
                else
                {
                    serviceResult = ServiceResult.GlobalSystemError();
                }
            }
            else
            {
                serviceResult = ServiceResult.GlobalSystemError();
            }

            return new ObjectResult(serviceResult) { StatusCode = serviceResult.ErrorResult is not null ? (int)serviceResult.ErrorResult.Status : (int)HttpStatus.Internal };
        }
    }
}