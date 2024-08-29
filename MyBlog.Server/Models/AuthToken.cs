namespace MyBlog.Server.Models
{
    public class AuthToken
    {
        public int Minutes { get; set; }
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}
