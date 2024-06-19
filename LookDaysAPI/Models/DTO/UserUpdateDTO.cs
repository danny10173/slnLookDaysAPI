using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models
{
    public class UserUpdateDTO
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? fPhone { get; set; }

        public byte[]? UserPic { get; set; }  // Base64 編碼的圖片數據
    }
}
