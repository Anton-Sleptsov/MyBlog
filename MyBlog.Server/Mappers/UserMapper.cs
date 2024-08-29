using MyBlog.Data.Models;
using MyBlog.Server.Models.User;

namespace MyBlog.Server.Mappers
{
    public class UserMapper
    {
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
                Password = user.Password,
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
                Photo = data.Photo
            };
    }
}
