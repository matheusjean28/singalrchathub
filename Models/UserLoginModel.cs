namespace UserLoginModel
{
    public class UserLogin
    {
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<object> MyChats { get; internal set; }
    }
}
