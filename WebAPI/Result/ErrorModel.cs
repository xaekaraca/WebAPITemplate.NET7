using System.Diagnostics.CodeAnalysis;

namespace WebAPI.Result;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ErrorModel
{
    public readonly string TraceId = Guid.NewGuid().ToString("N");

    public string Code { get; set; } = null!;

    public string? Message { get; set; }

    public string? Detail { get; set; }
}