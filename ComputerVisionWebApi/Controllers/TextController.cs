using Microsoft.AspNetCore.Mvc;
using Azure.AI.TextAnalytics;

namespace ComputerVisionWebApi.Controllers;

[ApiController]
[Route("text")]
public class TextController : ControllerBase
{
    private readonly TextAnalyticsClient _client;

    public TextController(TextAnalyticsClient client)
    {
        _client = client;
    }

    [HttpPost("language")]
    public async Task<List<dynamic>> DetectLanguage(string[] values)
    {

        var batch = new List<DetectLanguageInput>();

        foreach (var item in values)
        {
            batch.Add(new(Guid.NewGuid().ToString(), item));
        }

        var detectResults = await _client.DetectLanguageBatchAsync(batch);
        var result = new List<dynamic>();

        foreach (var item in detectResults.Value)
        {
            result.Add(new
            {
                Language = item.PrimaryLanguage.Iso6391Name,
                Confidence = item.PrimaryLanguage.ConfidenceScore * 100
            });
        }

        return result;
        //var detectResult = await _client.DetectLanguageAsync(text);

        //return new
        //{
        //    FullLanguageName = detectResult.Value.Name,
        //    ShortLanguageName = detectResult.Value.Iso6391Name,
        //    Confidence = detectResult.Value.ConfidenceScore * 100,
        //    detectResult.Value.Warnings
        //};
    }

    [HttpPost("sentiment")]
    public async Task<List<dynamic>> AnalyzeSentiment(string text)
    {
        var response = await _client.AnalyzeSentimentAsync(text);
        var result = new List<dynamic>();

        foreach (var item in response.Value.Sentences)
        {
            result.Add(new
            {
                item.Text,
                Scores = new
                {
                    item.ConfidenceScores.Positive,
                    item.ConfidenceScores.Negative,
                    item.ConfidenceScores.Neutral,
                },
                item.Opinions
            });
        }

        return result;
    }

    [HttpPost("phrases")]
    public async Task<List<string>> GetKeyPhrases(string text)
    {
        var phrasesResult = await _client.ExtractKeyPhrasesAsync(text);

        return phrasesResult.Value.ToList();
    }
}


