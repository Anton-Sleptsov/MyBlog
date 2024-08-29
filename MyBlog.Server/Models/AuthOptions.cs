using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyBlog.Server.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        private const string KEY = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
        public const int LIFETIME = 10;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
           return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}

