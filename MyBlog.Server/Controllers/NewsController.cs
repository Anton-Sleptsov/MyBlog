using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Data.Repositories;
using MyBlog.Server.Mappers;
using MyBlog.Server.Models.News;

namespace MyBlog.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class NewsController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly NewsRepository _newsRepository;
        private readonly NewsMapper _newsMapper;

        public NewsController(UserRepository userRepository,
            NewsMapper newsMapper,
            NewsRepository newsRepository)
        {
            _userRepository = userRepository;
            _newsMapper = newsMapper;
            _newsRepository = newsRepository;
        }

        [HttpGet("{userId}")]
        [AllowAnonymous]
        public IActionResult GetByAuthor(int userId)
        {
            var allNewsByAuthor = _newsRepository
                .GetByAuthor(userId)
                .Select(_newsMapper.BuildNewsModel)
                .ToList();

            return Ok(allNewsByAuthor);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NewsCreateModel newsModel) 
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            var news = _newsMapper.BuildDataModelFromCreate(newsModel);
            var createdNews = _newsRepository.Create(news, currentUser.Id);
            var newsViewModel = _newsMapper.BuildNewsModel(createdNews);

            return Ok(newsViewModel);
        }

        [HttpPatch]
        public IActionResult Update([FromBody] NewsUpdateModel newsModel)
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            var news = _newsMapper.BuildDataModelFromUpdate(newsModel);
            var updatedNews = _newsRepository.Update(news, currentUser.Id);

            if (updatedNews is null)
            {
                return BadRequest();
            }

            var newsViewModel = _newsMapper.BuildNewsModel(updatedNews);

            return Ok(newsViewModel);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public IActionResult Delete(int id)
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            if(!_newsRepository.Delete(id, currentUser.Id))
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
