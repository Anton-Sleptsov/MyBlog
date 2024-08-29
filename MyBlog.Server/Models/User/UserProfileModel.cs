namespace MyBlog.Server.Models.User
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public byte[]? Photo { get; set; }
    }
}
