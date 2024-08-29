namespace MyBlog.Data.Models
{
    public class News
    {
        public int Id { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public virtual User Author { get; set; }
        public string Text { get; set; }
        public byte[]? Image { get; set; }
    }
}
