using MyBlog.Data.Models;

namespace MyBlog.Data.Repositories
{
    public class UserRepository
    {
        private readonly BlogDbContext _dbContext;
        private readonly NoSqlDataService _noSqlDataService;

        public UserRepository(BlogDbContext dbContext, 
            NoSqlDataService noSqlDataService)
        {
            _dbContext = dbContext;
            _noSqlDataService = noSqlDataService;
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

        public List<User> GetByName(string name)
        {
            return _dbContext.Users
                .Where(user => user.Name.ToLower().Contains(name.ToLower()))
                .ToList();
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

        public void Subscribe(int userId, int authorId)
        {
            _noSqlDataService.Subscribe(userId, authorId);
        }

        public void Unsubscribe(int userId, int authorId)
        {
            _noSqlDataService.Unsubscribe(userId, authorId);
        }

        public int GetSubscribersCount(int userId)
        {
            return _noSqlDataService.GetSubscribersCount(userId);
        }
    }
}
