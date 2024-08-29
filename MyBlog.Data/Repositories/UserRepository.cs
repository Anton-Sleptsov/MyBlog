using MyBlog.Data.Models;

namespace MyBlog.Data.Repositories
{
    public class UserRepository
    {
        private readonly BlogDbContext _dbContext;

        public UserRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User Create(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public User? GetById(int id)
        {
            return _dbContext.Users
                .FirstOrDefault(user => user.Id == id);
        }

        public User? GetByLogin(string login)
        {
            return _dbContext.Users
                .FirstOrDefault(user => user.Email == login);
        }

        public User Update(User user)
        {
            var updatedUser = GetById(user.Id);

            updatedUser!.Name = user.Name;
            updatedUser.Email = user.Email;
            updatedUser.Password = user.Password;
            updatedUser.Description = user.Description;
            updatedUser.Photo = user.Photo;

            _dbContext.SaveChanges();
            return updatedUser;
        }

        public void Delete(int id)
        {
            var deletedUser = GetById(id);

            if (deletedUser is null)
            {
                return;
            }

            _dbContext.Users.Remove(deletedUser);
            _dbContext.SaveChanges();
        }

        public bool Subscribe(int userId, int authorId)
        {
            if (_dbContext.UserSubs.Any(s => s.From == userId && s.To == authorId))
            {
                return false;
            }

            if (!_dbContext.Users.Any(user => user.Id == authorId))
            {
                return false;
            }

            var sub = new UserSub
            {
                From = userId,
                To = authorId,
                Date = DateTime.UtcNow
            };

            _dbContext.UserSubs.Add(sub);
            _dbContext.SaveChanges();
            return true;
        }

        public void Unsubscribe(int userId, int authorId)
        {
            var sub = _dbContext.UserSubs.FirstOrDefault(s => s.From == userId && s.To == authorId);
            if (sub is null)
            {
                return;
            }

            _dbContext.UserSubs.Remove(sub);
            _dbContext.SaveChanges();
        }
    }
}
