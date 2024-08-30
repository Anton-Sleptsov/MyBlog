using LiteDB;
using MyBlog.Data.Models;

namespace MyBlog.Data
{
    public class NoSqlDataService
    {
        private readonly string _databasePath = "";

        private const string SUBS_COLLECTION = nameof(SUBS_COLLECTION);
        private const string NEWS_LIKE_COLLECTION = nameof(NEWS_LIKE_COLLECTION);

        internal UserSubs GetUserSubs(int userId)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var subs = db.GetCollection<UserSubs>(SUBS_COLLECTION);

                return subs.FindOne(s => s.UserId == userId);
            }
        }

        internal UserSubs Subscribe(int userId, int authorId)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var subs = db.GetCollection<UserSubs>(SUBS_COLLECTION);

                var subsForUser = subs.FindOne(s => s.UserId == userId);

                if (subsForUser is not null) 
                { 
                    if (!subsForUser.AuthorIds.Contains(authorId))
                    {
                        subsForUser.AuthorIds.Add(authorId);
                        subs.Update(subsForUser);
                    }
                }
                else
                {
                    var newSubsForUser = new UserSubs
                    {
                        UserId = userId,
                        AuthorIds = new List<int> { authorId }
                    };

                    subs.Insert(newSubsForUser);
                    subs.EnsureIndex(s => s.UserId);

                    subsForUser = newSubsForUser;
                }

                return subsForUser;
            }
        }

        internal UserSubs Unsubscribe(int userId, int authorId)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var subs = db.GetCollection<UserSubs>(SUBS_COLLECTION);

                var subsForUser = subs.FindOne(s => s.UserId == userId);

                if (subsForUser is not null && subsForUser.AuthorIds.Contains(authorId))
                {
                    subsForUser.AuthorIds.Remove(authorId);
                    subs.Update(subsForUser);
                }

                return subsForUser;
            }
        }

        internal NewsLikes GetNewsLikes(int newsId)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var likes = db.GetCollection<NewsLikes>(NEWS_LIKE_COLLECTION);

                return likes.FindOne(l => l.NewsId == newsId);
            }
        }

        internal NewsLikes SetLike(int newsId, int userId)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var likes = db.GetCollection<NewsLikes>(NEWS_LIKE_COLLECTION);

                var likesForNews = likes.FindOne(l => l.NewsId == newsId);

                if (likesForNews is not null)
                {
                    if (!likesForNews.UserIds.Contains(userId))
                    {
                        likesForNews.UserIds.Add(userId);
                        likes.Update(likesForNews);
                    }
                }
                else
                {
                    var newLikesForNews = new NewsLikes
                    {
                        NewsId = newsId,
                        UserIds = new List<int> { userId }
                    };

                    likes.Insert(newLikesForNews);
                    likes.EnsureIndex(l => l.NewsId);

                    likesForNews = newLikesForNews;
                }

                return likesForNews;
            }
        }

        internal NewsLikes RemoveLike(int newsId, int userId)
        {
            using (var db = new LiteDatabase(_databasePath))
            {
                var likes = db.GetCollection<NewsLikes>(NEWS_LIKE_COLLECTION);

                var likesForNews = likes.FindOne(l => l.NewsId == newsId);

                if (likesForNews is not null && likesForNews.UserIds.Contains(userId))
                {
                    likesForNews.UserIds.Remove(userId);
                    likes.Update(likesForNews);
                }

                return likesForNews;
            }
        }
    }
}
