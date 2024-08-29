using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Models;

namespace MyBlog.Data.Repositories
{
    public class NewsRepository
    {
        private readonly BlogDbContext _dbContext;

        public NewsRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<News> GetByAuthor(int userId)
        {
            return _dbContext.News
                .Include(news => news.Author)
                .Where(news => news.Author.Id == userId)
                .ToList();
        }

        public News Create(News news, int userId)
        {
            var user = _dbContext.Users.First(user => user.Id == userId);
            news.Author = user;

            _dbContext.News.Add(news);
            _dbContext.SaveChanges();
            return news;
        }

        public News? Update(News news, int userId)
        {
            var updatedNews = _dbContext.News
                .Include(n => n.Author)
                .FirstOrDefault(n => n.Id == news.Id && n.Author.Id == userId);

            if (updatedNews is null)
            {
                return null;
            }

            updatedNews.Text = news.Text;
            updatedNews.Image = news.Image;
            _dbContext.SaveChanges();

            return updatedNews;
        }

        public bool Delete(int newsId, int userId)
        {
            var deletedNews = _dbContext.News
                .Include(n => n.Author)
                .FirstOrDefault(n => n.Id == newsId && n.Author.Id == userId);

            if (deletedNews is null)
            {
                return false;
            }

            _dbContext.News.Remove(deletedNews);
            _dbContext.SaveChanges();
            return true;
        }

        public List<News> GetNewsForUser(int userId)
        {
            List<News> newsForUser = new();

            var subs = _dbContext.UserSubs
                .Where(sub => sub.From == userId)
                .ToList();

            foreach (var sub in subs)
            {
                var newsFromThisAuthor = GetByAuthor(sub.To);
                newsForUser.AddRange(newsFromThisAuthor);
            }

            return newsForUser
                .OrderByDescending(news => news.TimeOfCreation)
                .ToList();
        }

        public bool SetLike(int userId, int newsId)
        {
            if (_dbContext.NewsLikes.Any(l => l.UserId == userId && l.NewsId == newsId))
            {
                return false;
            }

            if (!_dbContext.News.Any(news => news.Id == newsId))
            {
                return false;
            }

            var like = new NewsLike
            {
                UserId = userId,
                NewsId = newsId
            };

            _dbContext.NewsLikes.Add(like);
            _dbContext.SaveChanges();
            return false;
        }

        public void RemoveLike(int userId, int newsId)
        {
            var like = _dbContext.NewsLikes.FirstOrDefault(l => l.UserId == userId && l.NewsId == newsId);
            if (like is null)
            {
                return;
            }

            _dbContext.NewsLikes.Remove(like);
            _dbContext.SaveChanges();
        }

        public int GetCountOfLikes(int newsId)
        {
            return _dbContext.NewsLikes
                .Where(like => like.NewsId == newsId)
                .Count();
        }
    }
}
