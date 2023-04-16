using Azure;
using Azure.AI.TextAnalytics;

using ComputerVisionWebApi.Credentials;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(_ =>
{
    var credentials = new VisionClientCredentials(builder.Configuration["ComputerVisionKey"]!);

    return new ComputerVisionClient(credentials)
    {
        Endpoint = builder.Configuration["ComputerVisionEndpoint"]!
    };
});
builder.Services.AddSingleton(_ =>
{
    if (Uri.TryCreate(builder.Configuration["TextAnalysisEndpoint"]!, UriKind.Absolute, out var uri))
    {
        return new TextAnalyticsClient(uri, new AzureKeyCredential(builder.Configuration["TextAnalysisKey"]!));
    }

    throw new InvalidOperationException("Endpoint is invalid");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();