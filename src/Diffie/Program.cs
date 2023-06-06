using System.Dynamic;
using System.Net;
using JsonDiffer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static StorageConverter;

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


app.MapPost("/", (List<dynamic> input) => {
    if (input.Count < 2)
        throw new HttpRequestException("One of the inputs is empty. Must be a list with two objects.", null, HttpStatusCode.BadRequest);

    var firstText = input?.FirstOrDefault()?.ToString();
    var first = JToken.Parse(firstText);

    var secondText = input?.Skip(1)?.FirstOrDefault()?.ToString();
    var second = JToken.Parse(secondText);
    
    return JsonHelper.Difference(first, second, showOriginalValues:true).ToString();
});

app.MapGet("/now", () => DateTime.UtcNow);

app.Run();


public static class StorageConverter {
    public static decimal FromMB(this int input) => (decimal)(input * Math.Pow(10, 6));
}