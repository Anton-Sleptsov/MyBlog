using MyBlog.Data.Models;
using MyBlog.Data.Repositories;
using MyBlog.Server.Models.News;

namespace MyBlog.Server.Mappers
{
    public class NewsMapper
    {
        private readonly UserMapper _userMapper;
        private readonly NewsRepository _newsRepository;

        public NewsMapper(UserMapper userMapper, NewsRepository newsRepository)
        {
            _userMapper = userMapper;
            _newsRepository = newsRepository;
        }

        public News BuildDataModelFromCreate(NewsCreateModel news)
            => new() 
            {
                TimeOfCreation = DateTime.Now,
                Text = news.Text,
                Image = news.Image
            };

        public News BuildDataModelFromUpdate(NewsUpdateModel news)
           => new()
           {
               Id = news.Id,
               Text = news.Text,
               Image = news.Image
           };

        public NewsViewModel BuildNewsModel(News data)
            => new()
            {
                Id = data.Id,
                Author = _userMapper.BuildShortProfile(data.Author),
                Text = data.Text,
                Image = data.Image,
                TimeOfCreation = data.TimeOfCreation,
                LikesCount = _newsRepository.GetCountOfLikes(data.Id)
            };
    }
}
