namespace PromptMapper.Core.PromptCore.Configurations;

public class AIClientOptions
{
    public AIClientOptions(string model, string apiKey, string baseUrl)
    {
        Model = model;
        ApiKey = apiKey;
        BaseUrl = baseUrl;
    }

    public string Model { get; set; }
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; }
}