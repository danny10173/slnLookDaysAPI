namespace LookDaysAPI.Models.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; } = null;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
