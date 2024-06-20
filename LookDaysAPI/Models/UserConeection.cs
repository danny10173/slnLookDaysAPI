namespace ReactApp1.Server.Models
{
    public class UserConnection
    {
        public string ConnectionId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string ChatRoom { get; set; } = string.Empty;

        public UserConnection() { }

        public UserConnection(string connectionId, string username, string chatRoom)
        {
            ConnectionId = connectionId;
            Username = username;
            ChatRoom = chatRoom;
        }
    }
}
