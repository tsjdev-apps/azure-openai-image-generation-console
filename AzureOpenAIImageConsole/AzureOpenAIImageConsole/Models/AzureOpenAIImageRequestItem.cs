using System.Text.Json.Serialization;

namespace AzureOpenAIImageConsole.Models;

internal record class AzureOpenAIImageRequestItem(
    [property: JsonPropertyName("prompt")] string Prompt,
    [property: JsonPropertyName("n")] int? AmountOfImages,
    [property: JsonPropertyName("size")] string? ImageSize);
