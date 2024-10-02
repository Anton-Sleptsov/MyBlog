using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.Data.Repositories;
using MyBlog.Server.Mappers;
using MyBlog.Server.Models.User;
using MyBlog.Server.Models;
using MyBlog.Server.Servises;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MyBlog.Data.Models;

namespace MyBlog.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly UserRepository _userRepository;
        private readonly UserMapper _userMapper;

        public AccountController(UsersService usersService, 
            UserRepository userRepository, 
            UserMapper userMapper)
        {
            _usersService = usersService;
            _userRepository = userRepository;
            _userMapper = userMapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<UserProfileModel> Create([FromBody] UserCreateModel user)
        {
            var newUser = _userMapper.BuildDataModelFromCreate(user);
            var createdUser = _userRepository.Create(newUser);
            var createdProfile = _userMapper.BuildProfile(createdUser);

            return Ok(createdProfile);
        }

        [HttpPost("/[controller]/all")]
        public ActionResult<List<UserProfileModel>> CreateUsers([FromBody] List<UserCreateModel> users)
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }
            else if (currentUser.Id != 1)
            {
                return BadRequest();
            }

            var createdProfiles = new List<UserProfileModel>();

            foreach (var user in users)
            {              
                var newUser = _userMapper.BuildDataModelFromCreate(user);
                var createdUser = _userRepository.Create(newUser);
                var createdProfile = _userMapper.BuildProfile(createdUser);
                createdProfiles.Add(createdProfile);
            }

            return Ok(createdProfiles);
        }


        [HttpGet]
        public ActionResult<UserProfileModel> Get()
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }

            var user = _userMapper.BuildProfile(currentUser);
            return user;
        }

        [HttpPatch]
        public ActionResult<UserProfileModel> Update([FromBody] UserUpdateModel user)
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is null)
            {
                return NotFound();
            }
            else if (currentUser.Id != user.Id)
            {
                return BadRequest();
            }

            var data = _userMapper.BuildDataModelFromUpdate(user);
            var userAfterUpdate = _userRepository.Update(data);
            var userProfile = _userMapper.BuildProfile(userAfterUpdate);

            return Ok(userProfile);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            var currentUserEmail = HttpContext.User.Identity!.Name;
            var currentUser = _userRepository.GetByLogin(currentUserEmail);
            if (currentUser is  null)
            {
                return NotFound();
            }

            _userRepository.Delete(currentUser.Id);
            
            return NoContent();
        }

        [HttpPost("/token")]
        [AllowAnonymous]
        public ActionResult<AuthToken> GetToken()
        {
            var (login, password) = _usersService.GetUserLoginPassFromBasicAuth(Request);
            (ClaimsIdentity claims, int id)? identity = _usersService.GetIdentity(login, password);

            if (identity is null)
            {
                return NotFound("Login or password is not correct");
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity?.claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var tokenModel = new AuthToken
            {
                AccessToken = encodedJwt,
                Minutes = AuthOptions.LIFETIME,
                UserName = login,
                UserId = identity.Value.id
            };

            return Ok(tokenModel);
        }
    }
}
