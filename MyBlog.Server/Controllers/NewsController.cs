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

        [HttpGet]
        public IActionResult Get()
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            var newsForCurrentUser = _newsRepository
                .GetNewsForUser(currentUser.Id)
                .Select(_newsMapper.BuildNewsModel)
                .ToList();

            return Ok(newsForCurrentUser);
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

        [HttpPost("/[controller]/all")]
        public IActionResult CreateNews([FromBody] List<NewsCreateModel> newsModels)
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            var newsViewModels = new List<NewsViewModel>();
            foreach (var newsModel in newsModels)
            {
                var news = _newsMapper.BuildDataModelFromCreate(newsModel);
                var createdNews = _newsRepository.Create(news, currentUser.Id);
                var newsViewModel = _newsMapper.BuildNewsModel(createdNews);
                newsViewModels.Add(newsViewModel);
            }
            
            return Ok(newsViewModels);
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

        [HttpDelete("{newsId}")]
        public IActionResult Delete(int newsId)
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            if(!_newsRepository.Delete(newsId, currentUser.Id))
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost("setLike/{newsId}")]
        public IActionResult SetLike(int newsId)
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            _newsRepository.SetLike(currentUser.Id, newsId);
            return Ok();
        }

        [HttpPost("removeLike/{newsId}")]
        public IActionResult RemoveLike(int newsId)
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            _newsRepository.RemoveLike(currentUser.Id, newsId);
            return Ok();
        }
    }
}
