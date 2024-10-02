namespace MyBlog.Server.Models.User
{
    public class UserShortProfileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        public byte[]? Photo { get; set; }
    }
}
