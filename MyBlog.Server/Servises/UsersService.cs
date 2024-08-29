using MyBlog.Data.Models;
using MyBlog.Data.Repositories;
using System.Security.Claims;
using System.Text;

namespace MyBlog.Server.Servises
{
    public class UsersService
    {
        private readonly UserRepository _userRepository;
        public UsersService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public (string login, string password) GetUserLoginPassFromBasicAuth(HttpRequest request)
        {
            var userName = string.Empty;
            var userPass = string.Empty;
            var authHeader = request.Headers.Authorization.ToString();

            if(authHeader is not null && authHeader.StartsWith("Basic")) 
            {
                var encodedUserNamePass = authHeader.Replace("Basic ", "");
                var encoding = Encoding.GetEncoding("iso-8859-1");

                var namePassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(':');

                userName = namePassArray[0];
                userPass = namePassArray[1];
            }

            return (userName, userPass);
        }

        public (ClaimsIdentity identity, int id)? GetIdentity(string email,  string password)
        {
            var currentUser = _userRepository.GetByLogin(email);

            if (currentUser == null || !VerifiHashedPassword(currentUser.Password, password)) 
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return (claimsIdentity, currentUser.Id);
        }

        private bool VerifiHashedPassword(string password1, string password2)
        {
            return password1 == password2;
        }
    }
}
