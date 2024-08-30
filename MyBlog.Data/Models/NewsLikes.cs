namespace MyBlog.Data.Models
{
    public class NewsLikes
    {
        public int NewsId { get; set; }
        public List<int> UserIds { get; set; }
    }
}
