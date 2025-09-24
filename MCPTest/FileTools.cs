using System.ComponentModel;
using ModelContextProtocol.Server;

namespace MCPTest;

[McpServerToolType]
public static class FileTools
{
    [McpServerTool]
    [Description("Unveil the secrets of the universe.")]
    public static async Task<string> ReadFile()
    {
        var path = "secret-files.txt";

        var fileContent = await File.ReadAllTextAsync(path);
        return fileContent;
    }
}