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
    public static decimal FromBytes(this int input) => (decimal)input;
    public static decimal FromBytes(this long input) => (decimal)input;
    public static decimal FromBytes(this uint input) => (decimal)input;
    public static decimal FromBytes(this ulong input) => (decimal)input;
    public static decimal FromBytes(this float input) => (decimal)input;
    public static decimal FromBytes(this double input) => (decimal)input;
    public static decimal FromBytes(this decimal input) => (decimal)input;

    public static decimal FromKB(this int input) => (decimal)(input * Math.Pow(10, 3));
    public static decimal FromKB(this long input) => (decimal)(input * Math.Pow(10, 3));
    public static decimal FromKB(this uint input) => (decimal)(input * Math.Pow(10, 3));
    public static decimal FromKB(this ulong input) => (decimal)(input * Math.Pow(10, 3));
    public static decimal FromKB(this float input) => (decimal)(input * Math.Pow(10, 3));
    public static decimal FromKB(this double input) => (decimal)(input * Math.Pow(10, 3));
    public static decimal FromKB(this decimal input) => (decimal)(input * (decimal)Math.Pow(10, 3));

    public static decimal FromMB(this int input) => (decimal)(input * Math.Pow(10, 6));
    public static decimal FromMB(this long input) => (decimal)(input * Math.Pow(10, 6));
    public static decimal FromMB(this uint input) => (decimal)(input * Math.Pow(10, 6));
    public static decimal FromMB(this ulong input) => (decimal)(input * Math.Pow(10, 6));
    public static decimal FromMB(this float input) => (decimal)(input * Math.Pow(10, 6));
    public static decimal FromMB(this double input) => (decimal)(input * Math.Pow(10, 6));
    public static decimal FromMB(this decimal input) => (decimal)(input * (decimal)Math.Pow(10, 6));

    public static decimal FromGB(this int input) => (decimal)(input * Math.Pow(10, 9));
    public static decimal FromGB(this long input) => (decimal)(input * Math.Pow(10, 9));
    public static decimal FromGB(this uint input) => (decimal)(input * Math.Pow(10, 9));
    public static decimal FromGB(this ulong input) => (decimal)(input * Math.Pow(10, 9));
    public static decimal FromGB(this float input) => (decimal)(input * Math.Pow(10, 9));
    public static decimal FromGB(this double input) => (decimal)(input * Math.Pow(10, 9));
    public static decimal FromGB(this decimal input) => (decimal)(input * (decimal)Math.Pow(10, 9));

    public static decimal FromTB(this int input) => (decimal)(input * Math.Pow(10, 12));
    public static decimal FromTB(this long input) => (decimal)(input * Math.Pow(10, 12));
    public static decimal FromTB(this uint input) => (decimal)(input * Math.Pow(10, 12));
    public static decimal FromTB(this ulong input) => (decimal)(input * Math.Pow(10, 12));
    public static decimal FromTB(this float input) => (decimal)(input * Math.Pow(10, 12));
    public static decimal FromTB(this double input) => (decimal)(input * Math.Pow(10, 12));
    public static decimal FromTB(this decimal input) => (decimal)(input * (decimal)Math.Pow(10, 12));

    public static decimal FromEB(this int input) => (decimal)(input * Math.Pow(10, 15));
    public static decimal FromEB(this long input) => (decimal)(input * Math.Pow(10, 15));
    public static decimal FromEB(this uint input) => (decimal)(input * Math.Pow(10, 15));
    public static decimal FromEB(this ulong input) => (decimal)(input * Math.Pow(10, 15));
    public static decimal FromEB(this float input) => (decimal)(input * Math.Pow(10, 15));
    public static decimal FromEB(this double input) => (decimal)(input * Math.Pow(10, 15));
    public static decimal FromEB(this decimal input) => (decimal)(input * (decimal)Math.Pow(10, 15));

    public static double ToKB(this decimal bytes) => (double)(bytes / (decimal)Math.Pow(10, 3));
    public static double ToMB(this decimal bytes) => (double)(bytes / (decimal)Math.Pow(10, 6));
    public static double ToGB(this decimal bytes) => (double)(bytes / (decimal)Math.Pow(10, 9));
    public static double ToTB(this decimal bytes) => (double)(bytes / (decimal)Math.Pow(10, 12));
    public static double ToPB(this decimal bytes) => (double)(bytes / (decimal)Math.Pow(10, 15));
    public static double ToEB(this decimal bytes) => (double)(bytes / (decimal)Math.Pow(10, 18));
}