# Use Azure OpenAI to generate images using C#

![Logo](./docs/image.png)

Microsoft enables the options to generate images using Azure OpenAI. 

This repository contains a simple console application written in .NET 7 to demonstrate how to call an Azure OpenAI service to generate an image.

## Usage

### Generate images

You can use `dotnet run` to build the project and pass directly the parameters to the console application.

```bash
dotnet run 
  --openairesource <NAME OF YOUR AZURE OPENAI RESOURCE>
  --openaikey <API KEY OF YOUR AZURE OPENAI SERVICE>
  --prompt <PROMPT TO GENERATE THE IMAGE>
  --number <NUMBER OF IMAGES TO GENERATE>
  --size <SIZE OF THE IMAGE>
  --output <PATH TO A FILE TO STORE THE LINKS TO THE GENERATED IMAGES>
```

### Delete images

The generated images are stored within an Azure Blob Storage hosted by Microsoft. If you want to delete the images, you need to call another endpoint which is included in this project aswell. You can use `dotnet run` to build the project and pass directly the parameters to the console application.

```bash
dotnet run 
  --openairesource <NAME OF YOUR AZURE OPENAI RESOURCE>
  --openaikey <API KEY OF YOUR AZURE OPENAI SERVICE>
  --delete <ID OF THE IMAGE(S) TO GENERATE>
```

## Blog Posts

If you are more interested into details, please see the following [medium.com](https://www.medium.com) posts:

- [Use Azure OpenAI to create a Copilot on your own data / Part 1](https://medium.com/medialesson/use-azure-openai-to-create-a-copilot-on-your-own-data-part-1-ba1d997298ca)

- [Use Azure OpenAI to create a Copilot on your own data in C# / Part 2](https://medium.com/medialesson/use-azure-openai-to-create-a-copilot-on-your-own-data-in-c-part-2-b7acc1922337)

- [Deploy an Azure OpenAI service with LLM deployments via Bicep](https://medium.com/medialesson/deploy-an-azure-openai-service-with-llm-deployments-via-bicep-244411472d40)

- [Use Azure OpenAI to generate images using C#](https://medium.com/medialesson/use-azure-openai-to-generate-images-using-c-c2fa32e12b72)

