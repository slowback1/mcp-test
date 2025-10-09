using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;

var builder = Kernel.CreateBuilder();

var filePath = File.ReadAllText("serverPath.txt").Trim();

if (string.IsNullOrEmpty(filePath))
    throw new InvalidOperationException("The server path is not specified in serverPath.txt");

builder.Services.AddOpenAIChatCompletion(
    "llama3.1",
    apiKey: null,
    endpoint: new Uri("http://localhost:11434/v1")
);
var kernel = builder.Build();

var command = "dotnet";
string[] arguments = ["run", "--project", filePath, "--no-build"];

var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
    Name = "Friends DB Server",
    Command = command,
    Arguments = arguments
});

await using var mcpClient = await McpClient.CreateAsync(clientTransport);

var tools = await mcpClient.ListToolsAsync();
foreach (var tool in tools) Console.WriteLine($"Connected to server with tools: {tool.Name}");

kernel.Plugins.AddFromFunctions("McpTools", tools.Select(t => t.AsKernelFunction()));

Console.WriteLine("Chat with the MCP server (type 'exit' to quit):");
var history = new ChatHistory();
history.AddSystemMessage("You are a helpful assistant that answers questions about the Friends database.");

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

while (true)
{
    Console.WriteLine("User:");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    history.AddUserMessage(input);

    var settings = new OpenAIPromptExecutionSettings
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };

    var result = await chatCompletionService.GetChatMessageContentAsync(history, settings, kernel);

    Console.WriteLine($"AI-Chan: {result.Content}");
    history.AddMessage(result.Role, result.Content ?? string.Empty);
}