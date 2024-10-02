using MyBlog.Server.Models.User;

namespace MyBlog.Server.Models.News
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public UserShortProfileModel Author { get; set; }
        public string Text { get; set; }
        public byte[]? Image { get; set; }
        public int LikesCount { get; set; }
    }
}
