using System.CommandLine;

namespace AzureOpenAIImageConsole.Models;

internal record class AppOptions(
    string AzureOpenAIResource,
    string AzureOpenAIKey,
    string DeleteId,
    string Prompt,
    int NumberOfImages,
    int ImageSize,
    string OutputFilePath,
    IConsole Console) : AppConsole(Console);
