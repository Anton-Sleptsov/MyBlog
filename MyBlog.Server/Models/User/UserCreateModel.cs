namespace MyBlog.Server.Models.User
{
    public class UserCreateModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public byte[]? Photo { get; set; }
    }
}
