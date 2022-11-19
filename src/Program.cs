using HtmlToPdfApi;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HTML to PDF lib - WkHtmlToPdf-DotNet
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddSingleton<IHtmlToPdfService, HtmlToPdfService>();
builder.Services.ConfigureHttpJsonOptions(op => op.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapPost("/convert", ([FromServices] IHtmlToPdfService pdfConverter, ConvertRequest req) =>
{
    var pdf = pdfConverter.Convert(req);
    return pdf.Any() ? Results.Ok(Convert.ToBase64String(pdf)) : Results.BadRequest();
})
.Produces<string>()
.Produces(400)
.WithDescription("Converts HTML to PDF. Returns PDF bytes enconded in Base64 string.")
.WithOpenApi();

app.Run();