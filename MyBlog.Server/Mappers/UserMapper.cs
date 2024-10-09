using MyBlog.Data.Models;
using MyBlog.Data.Repositories;
using MyBlog.Server.Models.User;

namespace MyBlog.Server.Mappers
{
    public class UserMapper
    {
        private readonly UserRepository _userRepository;

        public UserMapper(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public User BuildDataModelFromCreate(UserCreateModel user)
            => new()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Description = user.Description,
                Photo = user.Photo
            };

        public User BuildDataModelFromUpdate(UserUpdateModel user)
            => new()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Description = user.Description,
                Photo = user.Photo
            };

        public UserProfileModel BuildProfile(User data)
            => new()
            {
                Id = data.Id,
                Name = data.Name,
                Email = data.Email,
                Description = data.Description,
                Photo = data.Photo,
                SubscribersCount = _userRepository.GetSubscribersCount(data.Id)
            };

        public UserShortProfileModel BuildShortProfile(User data)
            => new()
            {
                Id = data.Id,
                Name = data.Name,
                ShortDescription = new string(data.Description?.Take(50).ToArray()),
                Photo = data.Photo
            };
    }
}
