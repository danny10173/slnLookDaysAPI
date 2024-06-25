using System.ComponentModel.DataAnnotations;

namespace ReactApp1.Server.Models
{
    public class UserConnection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ConnectionId { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string ChatRoom { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        public UserConnection() { }

        public UserConnection(string connectionId, string username, string chatRoom)
        {
            ConnectionId = connectionId;
            Username = username;
            ChatRoom = chatRoom;
        }
    }
}
