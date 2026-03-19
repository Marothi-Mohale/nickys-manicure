using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace NickysManicurePedicure.Infrastructure;

public sealed class SystemTextJsonStreamOutputFormatter : TextOutputFormatter
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SystemTextJsonStreamOutputFormatter(JsonSerializerOptions? jsonSerializerOptions = null)
    {
        _jsonSerializerOptions = jsonSerializerOptions is null
            ? new JsonSerializerOptions(JsonSerializerDefaults.Web)
            : new JsonSerializerOptions(jsonSerializerOptions);

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
        SupportedMediaTypes.Add("application/json");
        SupportedMediaTypes.Add("text/json");
        SupportedMediaTypes.Add("application/*+json");
    }

    protected override bool CanWriteType(Type? type) => type is not null;

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        ArgumentNullException.ThrowIfNull(context);

        var objectType = context.ObjectType ?? context.Object?.GetType() ?? typeof(object);

        await JsonSerializer.SerializeAsync(
            context.HttpContext.Response.Body,
            context.Object,
            objectType,
            _jsonSerializerOptions,
            context.HttpContext.RequestAborted);
    }
}
