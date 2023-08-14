using AzureOpenAIImageConsole.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

internal static partial class Program
{
    private static HttpClient CreateHttpClient(
        string azureOpenAIKey)
    {
        // create httpclient
        var httpClient = new HttpClient();

        // set accept header
        httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        // set api-key header
        httpClient.DefaultRequestHeaders.Add(
            "api-key",
            azureOpenAIKey);

        // set useragent header
        httpClient.DefaultRequestHeaders.Add(
            "x-ms-useragent",
            "AzureOpenAIImageConsole/0.0.1");

        // return httpclient
        return httpClient;
    }

    private static HttpRequestMessage CreateHttpRequestMessage(
        string azureOpenAIResource, 
        string prompt, 
        int? amount, 
        int size)
    {
        // translate size to size string
        string sizeValue = ToSizeValue(size);

        // create requestItem
        AzureOpenAIImageRequestItem requestItem = new(
            prompt, 
            amount, 
            sizeValue);

        // serialize request body
        string requestContent = JsonSerializer.Serialize(requestItem);

        // get endpoint
        string endpoint = GetRequestEndpoint(azureOpenAIResource);

        // create HttpRequestMessage
        HttpRequestMessage request = new(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(
                requestContent,
                Encoding.UTF8,
                "application/json")
        };

        // return request
        return request;
    }

    private static async Task<string?> GetGenerationIdAsync(
        HttpResponseMessage httpResponseMessage)
    {
        // validate that the request was successful
        httpResponseMessage.EnsureSuccessStatusCode();

        // read the content
        string content = await httpResponseMessage.Content
            .ReadAsStringAsync();

        // deserialize the content
        AzureOpenAIImageResponseItem? response = JsonSerializer
            .Deserialize<AzureOpenAIImageResponseItem>(content);

        // return the image creation id
        return response?.Id;
    }

    private static async Task<(bool Success, AzureOpenAIImageResponseItem? Response)> GetImageCreationResponseAsync(
        this HttpResponseMessage httpResponseMessage)
    {
        // validate that the request was successful
        httpResponseMessage.EnsureSuccessStatusCode();

        // read the content
        string content = await httpResponseMessage.Content
            .ReadAsStringAsync();

        // deserialize the content
        AzureOpenAIImageResponseItem? response = JsonSerializer
            .Deserialize<AzureOpenAIImageResponseItem>(content);

        // check if status == "succeeded"
        if (response?.Status == "succeeded")
        {
            return (true, response);
        }

        // no validate response was send
        return (false, null);
    }

    // Get the endpoint for the image request
    private static string GetRequestEndpoint(string resource)
        => $"https://{resource}.openai.azure.com/" +
            $"openai/images/generations:submit" +
            $"?api-version=2023-06-01-preview";

    // Get the endpoint for the image check
    private static string GetCheckEndpoint(string resource, string? id)
        => $"https://{resource}.openai.azure.com/" +
            $"openai/operations/images/{id}" +
            $"?api-version=2023-06-01-preview";

    // Get the endpoint for the image deletion
    private static string GetDeleteEndpoint(string resource, string? id)
        => $"https://{resource}.openai.azure.com/" +
            $"openai/operations/images/{id}" +
            $"?api-version=2023-06-01-preview";

    // Map an integer value to a concrete size string
    private static string ToSizeValue(int size)
        => size switch
        {
            1 => "256x256",
            2 => "512x512",
            3 => "1024x1024",
            _ => "512x512"
        };
}

