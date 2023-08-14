using AzureOpenAIImageConsole.Models;
using System.CommandLine;
using System.CommandLine.Invocation;

internal static partial class Program
{
    private static readonly Option<string> _azureOpenAIResource =
        new(name: "--openairesource",
            description: "Resource name of your Azure OpenAI service.");

    private static readonly Option<string> _azureOpenAIKey =
        new(name: "--openaikey",
            description: "API key of your Azure OpenAI service.");

    private static readonly Option<string> _deleteId =
        new(aliases: new string[] { "--delete", "-d" },
            description: "Delete the images with the corresponding id.");

    private static readonly Option<string> _prompt =
        new(aliases: new string[] { "--prompt", "-p" },
            description: "A text description of the desired image.");

    private static readonly Option<int> _numberOfImages =
        new(aliases: new string[] { "--number", "-n" },
            description: "The number of images to generate (1 to 5).");

    private static readonly Option<int> _imageSize =
        new(aliases: new string[] { "--size", "-s" },
            description: "The desired image size (1: 256x256, 2: 512x512, 3: 1024x1024).");

    private static readonly Option<string> _outputFilePath =
        new(aliases: new string[] { "--output", "-o" },
            description: "Path to a file where the image links will be stored.");


    private static readonly RootCommand _rootCommand =
    new(description: """
        Use an Azure OpenAI service to generate an amount of images from 
        a text prompt in a desired size.
        """)
    {
            _azureOpenAIResource, _azureOpenAIKey, _deleteId, _prompt,
            _numberOfImages, _imageSize, _outputFilePath
    };

    private static AppOptions GetParsedAppOptions(InvocationContext context) =>
       new(
           AzureOpenAIResource: context.ParseResult.GetValueForOption(_azureOpenAIResource) ?? "",
           AzureOpenAIKey: context.ParseResult.GetValueForOption(_azureOpenAIKey) ?? "",
           DeleteId: context.ParseResult.GetValueForOption(_deleteId) ?? "",
           Prompt: context.ParseResult.GetValueForOption(_prompt) ?? "",
           NumberOfImages: context.ParseResult.GetValueForOption(_numberOfImages),
           ImageSize: context.ParseResult.GetValueForOption(_imageSize),
           OutputFilePath: context.ParseResult.GetValueForOption(_outputFilePath) ?? "",
           Console: context.Console);
}

