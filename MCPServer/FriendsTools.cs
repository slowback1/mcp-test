using System.ComponentModel;
using System.Text.Json;
using MCPServer.Models;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;

namespace MCPServer;

[McpServerToolType]
public static class FriendsTools
{
    [McpServerTool]
    [Description("Get a list of friends")]
    public static async Task<string> GetFriends(AppDbContext context)
    {
        try
        {
            var friends = await context.Friends.ToListAsync();

            var json = JsonSerializer.Serialize(friends);

            return json;
        }
        catch (Exception E)
        {
            return E.Message;
        }
    }

    [McpServerTool]
    [Description("Add a new friend")]
    public static async Task<string> AddFriend(AppDbContext context, Friend friend)
    {
        try
        {
            context.Friends.Add(friend);
            await context.SaveChangesAsync();
            return "Friend added successfully.";
        }
        catch (Exception E)
        {
            return E.Message;
        }
    }

    [McpServerTool]
    [Description("Change an existing friend's friend level")]
    public static async Task<string> UpdateFriendLevel(AppDbContext context,
        [Description("The name of the friend to update")]
        string friendName, [Description("The level to set the friend to")] int newFriendLevel)
    {
        var friend = await context.Friends.FirstOrDefaultAsync(f => f.Name == friendName);
        if (friend == null) return $"Friend with name '{friendName}' not found.";

        friend.FriendLevel = newFriendLevel;
        await context.SaveChangesAsync();

        return $"Friend level for '{friendName}' updated to {newFriendLevel}.";
    }

    [McpServerTool]
    [Description("Delete a friend by name")]
    public static async Task<string> DeleteFriend(AppDbContext context,
        [Description("The name of the friend to delete")]
        string friendName)
    {
        var friend = await context.Friends.FirstOrDefaultAsync(f => f.Name == friendName);
        if (friend == null) return $"Friend with name '{friendName}' not found.";

        context.Friends.Remove(friend);
        await context.SaveChangesAsync();

        return $"Friend '{friendName}' deleted successfully.";
    }
}