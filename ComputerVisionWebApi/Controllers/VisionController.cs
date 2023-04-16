using ComputerVisionWebApi.Credentials;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Rest;

using System;
using System.Web;

namespace ComputerVisionWebApi.Controllers;

[ApiController]
[Route("vision")]
public class VisionController : ControllerBase
{
    private readonly ComputerVisionClient _client;

    public VisionController(ComputerVisionClient client)
    {
        _client = client;
    }

    [HttpGet("link/{url}")]
    public async Task<List<dynamic>> DetectByUrl(string url)
    {
        url = HttpUtility.UrlDecode(url);
        var detectResult = await _client.DetectObjectsAsync(url);

        return GenerateResult(detectResult.Objects);
    }

    [HttpGet("local/{path}")]
    public async Task<List<dynamic>> DetectFromLocal(string path)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var detectResult = await _client.DetectObjectsInStreamAsync(fileStream);

        return GenerateResult(detectResult.Objects);
    }

    private static List<dynamic> GenerateResult(IList<DetectedObject> detectedObjects)
    {
        var result = new List<dynamic>();

        foreach (var @object in detectedObjects)
        {
            result.Add(new
            {
                Description = @object.ObjectProperty,
                Confidence = @object.Confidence * 100,
                Rect = new
                {
                    @object.Rectangle.X,
                    @object.Rectangle.Y
                },
                Width = @object.Rectangle.W,
                Height = @object.Rectangle.H,
            });
        }

        return result;
    }
}


