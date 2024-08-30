using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Models;

namespace MyBlog.Data.Repositories
{
    public class NewsRepository
    {
        private readonly BlogDbContext _dbContext;
        private readonly NoSqlDataService _noSqlDataService;

        public NewsRepository(BlogDbContext dbContext, 
            NoSqlDataService noSqlDataService)
        {
            _dbContext = dbContext;
            _noSqlDataService = noSqlDataService;
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

            var authors = _noSqlDataService.GetUserSubs(userId).AuthorIds;

            foreach (var author in authors)
            {
                var newsFromThisAuthor = GetByAuthor(author);
                newsForUser.AddRange(newsFromThisAuthor);
            }

            return newsForUser
                .OrderByDescending(news => news.TimeOfCreation)
                .ToList();
        }

        public void SetLike(int userId, int newsId)
        {
            _noSqlDataService.SetLike(newsId, userId);
        }

        public void RemoveLike(int userId, int newsId)
        {
            _noSqlDataService.RemoveLike(newsId, userId);
        }

        public int GetCountOfLikes(int newsId)
        {
            return _noSqlDataService.GetNewsLikes(newsId).UserIds.Count;
        }
    }
}
