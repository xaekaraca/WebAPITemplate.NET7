using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Net.Http.Headers;
using Refit;

namespace WebAPI.Result;

public class ApiResult : ObjectResult
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Location
    {
        public string Action { get; }
        public string Controller { get; }
        public object? RouteValues { get; }

        public Location(string action, string controller, object? routeValues)
        {
            Action = action;
            Controller = controller;
            RouteValues = routeValues;
        }
    }

    private readonly string? _locationPath;

    private readonly Location? _location;

    private ApiResult(object? value, HttpHeaders headers, int statusCode, bool isSuccess) : base(value)
    {
        if (isSuccess && headers.TryGetValues(HeaderNames.Location, out var locationValues))
        {
            _locationPath = locationValues.FirstOrDefault();
        }

        StatusCode = statusCode;
    }

    public ApiResult(IApiResponse<string> refitResponse) : this(
        refitResponse.IsSuccessStatusCode ? refitResponse.Content : refitResponse.Error?.Content,
        refitResponse.Headers, (int)refitResponse.StatusCode, refitResponse.IsSuccessStatusCode)
    {
    }

    public ApiResult(IApiResponse<object> refitResponse) : this(
        refitResponse.IsSuccessStatusCode ? refitResponse.Content : refitResponse.Error?.Content,
        refitResponse.Headers, (int)refitResponse.StatusCode, refitResponse.IsSuccessStatusCode)
    {
        if (refitResponse.IsSuccessStatusCode)
        {
            DeclaredType = refitResponse.Content?.GetType();
        }
    }

    public ApiResult(IApiResponse refitResponse, Location? location = null) : this(
        refitResponse.IsSuccessStatusCode ? null : refitResponse.Error?.Content,
        refitResponse.Headers, (int)refitResponse.StatusCode, refitResponse.IsSuccessStatusCode)
    {
        _location = location;
    }

    public ApiResult(HttpStatusCode httpStatusCode, object? value = null, Location? location = null) : base(value)
    {
        StatusCode = (int)httpStatusCode;
        _location = location;
    }

    public ApiResult(int statusCode, object? value = null, Location? location = null) : base(value)
    {
        StatusCode = statusCode;
        _location = location;
    }

    public override void OnFormatting(ActionContext context)
    {
        if (!string.IsNullOrWhiteSpace(_locationPath))
        {
            context.HttpContext.Response.Headers.Location = _locationPath;
        }
        else if (_location != null)
        {
            var urlHelperFactory = context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = urlHelperFactory.GetUrlHelper(context);
            var locationPath = urlHelper.Action(_location.Action, _location.Controller, _location.RouteValues) ?? string.Empty;
            context.HttpContext.Response.Headers.Location = locationPath;
        }

        base.OnFormatting(context);
    }
}