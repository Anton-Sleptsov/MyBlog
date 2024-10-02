using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Data.Repositories;
using MyBlog.Server.Mappers;

namespace MyBlog.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly UserMapper _userMapper;

        public UsersController(UserRepository userRepository, UserMapper userMapper)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetUsersByName(string name)
        {
            var users = _userRepository
                .GetByName(name)
                .Select(_userMapper.BuildShortProfile)
                .ToList();

            return Ok(users);
        }

        [HttpGet("{userId}")]
        [AllowAnonymous]
        public IActionResult Get(int userId)
        {
            var userData = _userRepository.GetById(userId);
            if(userData is null)
            {
                return NotFound();
            }

            var user = _userMapper.BuildProfile(userData);
                
            return Ok(user);
        }

        [HttpPost("subscribe/{userId}")]
        public IActionResult Subscribe(int userId)
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            if(currentUser.Id == userId)
            {
                return BadRequest();
            }

            _userRepository.Subscribe(currentUser.Id, userId);
            return Ok();
        }

        [HttpPost("unsubscribe/{userId}")]
        public IActionResult Unsubscribe(int userId)
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            if (currentUser.Id == userId)
            {
                return BadRequest();
            }

            _userRepository.Unsubscribe(currentUser.Id, userId);
            return Ok();
        }
    }
}
