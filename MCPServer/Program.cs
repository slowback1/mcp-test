using MCPServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateEmptyApplicationBuilder(null);

var exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
var dbPath = Path.Combine(exeDir!, "app.db");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source=\"{dbPath}\""));

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();


var app = builder.Build();

await app.RunAsync();

