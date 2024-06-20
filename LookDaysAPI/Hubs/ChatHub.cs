using LookDaysAPI.DataService;
using Microsoft.AspNetCore.SignalR;
using ReactApp1.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApp1.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static int chatRoomCounter = 1;
        private readonly SharedDb _shared;

        public ChatHub(SharedDb shared)
        {
            _shared = shared;
        }

        public async Task JoinChat(UserConnection conn)
        {
            conn.ChatRoom = GenerateChatRoom();
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);

            _shared.connections[Context.ConnectionId] = conn;

            await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "客服", $"{conn.Username} has joined {conn.ChatRoom}");
        }

        public async Task JoinSpecificChatRoom(UserConnection conn)
        {
            if (conn.ChatRoom == null || conn.ChatRoom == "")
            {
                conn.ChatRoom = GenerateChatRoom();
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);
            _shared.connections[Context.ConnectionId] = conn;

            var history = _shared.GetMessagesForChatRoom(conn.ChatRoom);
            await Clients.Caller.SendAsync("ReceiveChatHistory", history);

            await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "客服", $"{conn.Username} has joined {conn.ChatRoom}");
        }

        public async Task LeaveSpecificChatRoom(UserConnection conn)
        {
            if (_shared.connections.TryRemove(Context.ConnectionId, out _))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, conn.ChatRoom);
                await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "客服", $"{conn.Username} has left {conn.ChatRoom}");
            }
        }

        public Task<List<UserConnection>> GetActiveChatRooms()
        {
            var activeRooms = _shared.connections.Values
                .GroupBy(c => c.ChatRoom)
                .Select(g => new UserConnection { ChatRoom = g.Key, Username = g.First().Username })
                .ToList();
            return Task.FromResult(activeRooms);
        }


        public async Task SendMessage(string msg)
        {
            if (_shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
            {
                var message = new Message
                {
                    Username = conn.Username,
                    Content = msg,
                    Timestamp = DateTime.Now
                };

                _shared.AddMessageToChatHistory(conn.ChatRoom, message);
                await Clients.Group(conn.ChatRoom).SendAsync("ReceiveSpecificMessage", conn.Username, msg);

                // 向所有客服發送新消息通知
                await Clients.Group("CustomerService").SendAsync("NotifyNewMessage", conn.ChatRoom, message);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "客服", "Error: Connection not found.");
            }
        }

        public async Task<List<Message>> GetChatHistory(string chatRoom)
        {
            var history = _shared.GetMessagesForChatRoom(chatRoom);
            return await Task.FromResult(history);
        }

        private string GenerateChatRoom()
        {
            return $"ChatRoom_{chatRoomCounter++}";
        }

        public override async Task OnConnectedAsync()
        {
            // 當客服連接時將其加入到客服組
            await Groups.AddToGroupAsync(Context.ConnectionId, "CustomerService");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // 當客服斷開連接時將其從客服組移除
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "CustomerService");

            if (_shared.connections.TryRemove(Context.ConnectionId, out UserConnection conn))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, conn.ChatRoom);
                await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "客服", $"{conn.Username} has left {conn.ChatRoom}");
            }

            await base.OnDisconnectedAsync(exception);
        }


    }
}
