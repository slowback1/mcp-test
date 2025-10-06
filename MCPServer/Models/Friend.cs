using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCPServer.Models;

[Table("Friends")]
public class Friend
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Pronouns { get; set; } = string.Empty;

    [Column("friend_level")]
    public int FriendLevel { get; set; } = 1;
}