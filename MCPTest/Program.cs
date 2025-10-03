

using MCPTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();
    
    
var app = builder.Build();

await app.RunAsync(); 