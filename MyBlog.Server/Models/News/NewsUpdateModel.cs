namespace MyBlog.Server.Models.News
{
    public class NewsUpdateModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public byte[]? Image { get; set; }
    }
}
