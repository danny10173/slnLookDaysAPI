using LookDaysAPI.Models;
using LookDaysAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace LookDaysAPI.DataAccess
{
    public class UserRepository
    {
        private readonly LookdaysContext _context;
        public UserRepository(LookdaysContext context)
        {
            this. _context = context;
        }

        public async Task<string> AddNewUser(LoginDTO signup)
        {
            string validationRes = SignUpValidation(signup);
            if (validationRes != string.Empty) { return "註冊資料格式錯誤"; }
            bool isUserExist = _context.Users.Any(m => m.Username == signup.Username);
            bool isEmailExist = _context.Users.Any(m => m.Email == signup.Email);

            if (!isUserExist && !isEmailExist)
            {
                User user = new User
                {
                    Username = signup.Username,
                    Email = signup.Email,
                    Password = signup.Password
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return "註冊成功";
            }
            else
            {
                return "使用者已存在";
            }
        }

        public async Task<Boolean> IsMemberExist(string username)
        {
            User? foundUser = await _context.Users.FirstOrDefaultAsync(m => m.Username == username);

            if (foundUser != null)
            {
                return true;
            }

            return false;
        }

        public async Task<User?> GetUserbyUsername(string username)
        {
            User? foundUser = await _context.Users.FirstOrDefaultAsync(m => m.Username == username);

            return foundUser;
        }

        public async Task<User?> AuthUser(LoginDTO signup)
        {
            if (!SignInPropsValidation(signup)) return null;

            User? foundUser = await _context.Users.FirstOrDefaultAsync(m => m.Username == signup.Username);

            if (foundUser != null)
            {
                if (signup.Password == foundUser.Password!)
                {
                    return foundUser;
                }

                return null;
            }

            return null;
        }

        private string SignUpValidation(LoginDTO user)
        {
            if (user.Username.Length < 8 || user.Username.Length > 24)
            {
                return "帳號長度必須在8至24之間";
            }

            if (!IsValidEmail(user.Email))
            {
                return "請輸入正確的Email格式";
            }

            if (user.Password.Length < 8 || user.Password.Length > 24)
            {
                return "密碼長度必須在8至24之間";
            }

            if (!IsValidPassword(user.Password))
            {
                return "密碼必須包含大小寫英文字母及數字";
            }

            return string.Empty;
        }


        public bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }
        public bool IsValidPassword(string password)
        {
            string pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[A-Za-z\\d]+$";
            return Regex.IsMatch(password, pattern);
        }
        private bool SignInPropsValidation(LoginDTO signup)
        {
            return !string.IsNullOrEmpty(signup.Username) && !string.IsNullOrEmpty(signup.Password);
        }
    }
}
