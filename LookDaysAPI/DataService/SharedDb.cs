using ReactApp1.Server.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LookDaysAPI.DataService
{
    public class SharedDb
    {
        private readonly ConcurrentDictionary<string, UserConnection> _connections = new();
        private readonly ConcurrentDictionary<string, List<Message>> _chatHistories = new();

        public ConcurrentDictionary<string, UserConnection> connections => _connections;

        public void AddMessageToChatHistory(string chatRoom, Message message)
        {
            if (!_chatHistories.ContainsKey(chatRoom))
            {
                _chatHistories[chatRoom] = new List<Message>();
            }
            _chatHistories[chatRoom].Add(message);
        }

        public List<Message> GetMessagesForChatRoom(string chatRoom)
        {
            if (_chatHistories.ContainsKey(chatRoom))
            {
                return _chatHistories[chatRoom];
            }
            return new List<Message>();
        }
    }
}
