namespace MyBlog.Data.Models
{
    public class UserSubs
    {
        public int Id { get; set; }
        public List<int> AuthorIds { get; set; }
    }
}
