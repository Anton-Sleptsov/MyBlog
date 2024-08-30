namespace MyBlog.Data.Models
{
    public class UserSubs
    {
        public int UserId { get; set; }
        public List<int> AuthorIds { get; set; }
    }
}
