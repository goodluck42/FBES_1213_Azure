using Microsoft.Rest;

namespace ComputerVisionWebApi.Credentials;

public class VisionClientCredentials : ServiceClientCredentials
{
    private readonly string _key;

    public VisionClientCredentials(string key)
    {
        _key = key;
    }

    public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("ocp-apim-subscription-key", _key);

        return base.ProcessHttpRequestAsync(request, cancellationToken);
    }
}