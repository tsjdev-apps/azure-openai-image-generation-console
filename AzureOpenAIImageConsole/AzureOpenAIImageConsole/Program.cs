using AzureOpenAIImageConsole.Models;
using System.CommandLine;

// Validate the AppOptions
_rootCommand.AddValidator(
    result =>
    {
        // Check that an Azure OpenAI Resource value is provided
        if (result.FindResultFor(_azureOpenAIResource) is null)
        {
            result.ErrorMessage +=
                $"Please provide the resource name of your Azure OpenAI service.{Environment.NewLine}";
        }

        // Check that an Azure OpenAI Key value is provided
        if (result.FindResultFor(_azureOpenAIKey) is null)
        {
            result.ErrorMessage +=
                $"Please provide the API key of your Azure OpenAI service.{Environment.NewLine}";
        }

        // Check that the --delete or -d value is provided
        if (result.FindResultFor(_deleteId) is not null)
        {
            return;
        }

        // Check that the --prompt or -p value is provided
        if (result.FindResultFor(_prompt) is null)
        {
            result.ErrorMessage +=
                $"Please provide a value for --prompt or -p.{Environment.NewLine}";
        }

        // Check that the --number or -n value is provided and in the correct range
        var numberOfImages = result.GetValueForOption(_numberOfImages);
        if (numberOfImages < 1 || numberOfImages > 5)
        {
            result.ErrorMessage +=
                $"Please provide a valid value for --numbers or -n. The value needs to be between 1 and 5.{Environment.NewLine}";
        }

        // Check that the --size or -s value is provided and in the correct range
        var imageSize = result.GetValueForOption(_imageSize);
        if (imageSize < 1 || imageSize > 3)
        {
            result.ErrorMessage +=
                $"Please provide a valid value for --size or -s. The value needs to be between 1 and 3.{Environment.NewLine}";
        }
    });

// Set the Handler for the Commmand
_rootCommand.SetHandler(
    async (context) =>
    {
        // get options
        AppOptions options = GetParsedAppOptions(
            context);

        // create httpclient
        HttpClient httpClient = CreateHttpClient(
            options.AzureOpenAIKey);

        // check if delete id is set and delete images if necessary
        if (!string.IsNullOrEmpty(options.DeleteId))
        {
            var deleteEndpoint = GetDeleteEndpoint(
                options.AzureOpenAIResource, 
                options.DeleteId);
            var deleteResponse = await httpClient.DeleteAsync(deleteEndpoint);

            if (deleteResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Images where deleted successfully.");
            }

            return;
        }

        // create request
        HttpRequestMessage requestMessage = CreateHttpRequestMessage(
            options.AzureOpenAIResource,
            options.Prompt,
            options.NumberOfImages,
            options.ImageSize);

        // make request
        HttpResponseMessage response = await httpClient
            .SendAsync(requestMessage);

        // get id
        string? id = await GetGenerationIdAsync(response);
        Console.WriteLine();
        Console.WriteLine($"Your ID: {id}");

        // waiting...
        Console.WriteLine();
        Console.WriteLine("Image(s) will be generated...");

        // check if images are available
        var checkEndpoint = GetCheckEndpoint(
            options.AzureOpenAIResource, 
            id);
        bool isFinished = false;

        do
        {
            await Task.Delay(2000);
            HttpResponseMessage checkResponse = await httpClient
                .GetAsync(checkEndpoint);
            (bool success, AzureOpenAIImageResponseItem? imageCreationResponse) = await checkResponse.GetImageCreationResponseAsync();

            isFinished = success;

            if (success && imageCreationResponse is not null)
            {
                string urls = string.Join(
                    Environment.NewLine, 
                    imageCreationResponse.Result.Data.Select(x => x.Url));
                
                await File.AppendAllTextAsync(
                    options.OutputFilePath, 
                    $"ID: {id}{Environment.NewLine}{urls}{Environment.NewLine}{Environment.NewLine}");

                Console.WriteLine();
                Console.WriteLine("Image(s) are written to file...");
            }
        } while (!isFinished);
    });

return await _rootCommand.InvokeAsync(args);