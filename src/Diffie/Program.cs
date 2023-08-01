using System.Collections;
using System.Dynamic;
using System.Net;
using JsonDiffer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

using static StorageConverter;
using Diffie;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Limit request body size
app.Use(async (context, next) =>
{
    var httpMaxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
    if (httpMaxRequestBodySizeFeature is not null) {
        httpMaxRequestBodySizeFeature.MaxRequestBodySize = (long)2.FromMB();
    } else {
        throw new Exception("Cannot set request body limit!");
    }
    
    await next(context);
});

var prototype = new DiffJob();
app.MapGet("/diff", () => prototype);

app.MapPost("/diff", (DiffJob input) => {
    if (input.Left == null || input.Right == null)
        throw new HttpRequestException("One of the inputs is empty. Must be a DiffJob object payload.", null, HttpStatusCode.BadRequest);

    var firstText = input?.Left?.ToString();
    var first = JToken.Parse(firstText);

    var secondText = input?.Right?.ToString();
    var second = JToken.Parse(secondText);
    var mode = input.DetailedOutput ? OutputMode.Detailed : OutputMode.Symbol;
    bool showInitial = (bool)input?.ShowInitialValues;

    return JsonHelper.Difference(first, second, outputMode:mode, showOriginalValues:showInitial).ToString();
});


app.MapGet("/now", () => DateTime.UtcNow);

app.Map("/", () => "Ok");

app.Run();


public static class StorageConverter {
    public static decimal FromMB(this int input) => (decimal)(input * Math.Pow(10, 6));
}