using Microsoft.AspNetCore.SignalR;
using LookDaysAPI.DataService;
using ReactApp1.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using LookDaysAPI.Models;

namespace ReactApp1.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static int chatRoomCounter = 1;
        private readonly SharedDb _shared;
        private readonly LookdaysContext _context;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(SharedDb shared, LookdaysContext context, ILogger<ChatHub> logger)
        {
            _shared = shared;
            _context = context;
            _logger = logger;
        }

        public async Task JoinChat(UserConnection conn)
        {
            if (conn.UserId == 0)
            {
                // 如果 UserId 為 0 或未設置，這裡可以處理錯誤或給予默認值
                await Clients.Caller.SendAsync("ReceiveMessage", "customerservice", "Error: UserId is required.");
                return;
            }

            conn.ChatRoom = GenerateChatRoom();
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);
            _shared.connections[Context.ConnectionId] = conn;
            _logger.LogInformation($"User {conn.Username} joined {conn.ChatRoom}");
            await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "customerservice", $"{conn.Username} has joined {conn.ChatRoom}");
        }


        public async Task JoinSpecificChatRoom(UserConnection conn)
        {
            _logger.LogInformation($"JoinSpecificChatRoom called with UserId: {conn.UserId}, Username: {conn.Username}");

            if (conn.UserId == 0)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "customerservice", "Error: UserId is required.");
                _logger.LogWarning("UserId is required but was not provided.");
                return;
            }

            _logger.LogInformation($"Adding user {conn.Username} to chat room {conn.ChatRoom}");
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);
            _shared.connections[Context.ConnectionId] = conn;

            var history = _shared.GetMessagesForChatRoom(conn.ChatRoom);
            await Clients.Caller.SendAsync("ReceiveChatHistory", history);
            _logger.LogInformation($"User {conn.Username} (UserId: {conn.UserId}) joined specific chat room {conn.ChatRoom}");
            await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "customerservice", $"{conn.Username} has joined {conn.ChatRoom}");
        }









        public async Task LeaveSpecificChatRoom(UserConnection conn)
        {
            if (_shared.connections.TryRemove(Context.ConnectionId, out _))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, conn.ChatRoom);
                _logger.LogInformation($"User {conn.Username} left {conn.ChatRoom}");
                await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "customerservice", $"{conn.Username} has left {conn.ChatRoom}");
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
            try
            {
                if (_shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
                {
                    // 檢查 UserId 是否存在於 User 表中
                    var user = await _context.Users.FindAsync(conn.UserId);
                    if (user == null)
                    {
                        await Clients.Caller.SendAsync("ReceiveMessage", "customerservice", "Error: User not found.");
                        return;
                    }

                    var message = new ChatMessage
                    {
                        UserId = conn.UserId,
                        Username = conn.Username,
                        ChatRoom = conn.ChatRoom,
                        ChatContent = msg,
                        Timestamp = DateTime.Now
                    };

                    // 保存消息到数据库
                    _context.ChatMessage.Add(message);
                    await _context.SaveChangesAsync();

                    // 保存消息到內存
                    _shared.AddMessageToChatHistory(conn.ChatRoom, new Message
                    {
                        Username = conn.Username,
                        ChatContent = msg,
                        Timestamp = DateTime.Now
                    });

                    // 发送消息到群组
                    await Clients.Group(conn.ChatRoom).SendAsync("ReceiveSpecificMessage", conn.Username, msg);

                    // 向所有客服发送新消息通知
                    await Clients.Group("CustomerService").SendAsync("NotifyNewMessage", conn.ChatRoom, message);
                }
                else
                {
                    await Clients.Caller.SendAsync("ReceiveMessage", "customerservice", "Error: Connection not found.");
                }
            }
            catch (Exception ex)
            {
                // 记录异常信息
                _logger.LogError(ex, "Error sending message");
                await Clients.Caller.SendAsync("ReceiveMessage", "customerservice", $"Error: {ex.Message}");
            }
        }



        public Task<List<Message>> GetChatHistory(string chatRoom)
        {
            var history = _shared.GetMessagesForChatRoom(chatRoom);
            return Task.FromResult(history);
        }

        private string GenerateChatRoom()
        {
            return $"ChatRoom_{chatRoomCounter++}";
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "CustomerService");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "CustomerService");

            if (_shared.connections.TryRemove(Context.ConnectionId, out UserConnection conn))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, conn.ChatRoom);
                await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "customerservice", $"{conn.Username} has left {conn.ChatRoom}");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
