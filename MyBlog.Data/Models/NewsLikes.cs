namespace MyBlog.Data.Models
{
    public class NewsLikes
    {
        public int Id { get; set; }
        public List<int> UserIds { get; set; }
    }
}
