﻿namespace MyBlog.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public byte[]? Photo { get; set; }
        public virtual List<News>? News { get; set; }
    }
}
