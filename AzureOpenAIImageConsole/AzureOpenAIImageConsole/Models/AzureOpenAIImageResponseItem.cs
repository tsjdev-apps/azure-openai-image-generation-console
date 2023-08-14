using System.Text.Json.Serialization;

namespace AzureOpenAIImageConsole.Models;

internal record class AzureOpenAIImageResponseItem(
    [property: JsonPropertyName("created")] int Created,
    [property: JsonPropertyName("expires")] int Expires,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("result")] Result Result,
    [property: JsonPropertyName("status")] string Status);

internal record class Result(
    [property: JsonPropertyName("data")] Data[] Data);

internal record class Data(
    [property: JsonPropertyName("url")] string Url);
