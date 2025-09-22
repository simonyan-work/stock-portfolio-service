using Microsoft.IdentityModel.Tokens;
using StockPortfolioAPI.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockPortfolioAPI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly List<User> _users;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;

            _users = new List<User>
            {
                new User{Id =  1, Username = "admin", Password = "admin", Role = "Admin"},
                new User{Id = 2, Username = "user", Password = "user", Role = "User"},
                new User{Id = 3, Username = "guest", Password = "guest", Role = "Guest"}
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await GetUserByUsernameAsync(request.Username);
            if (user == null || user.Password != request.Password)
            {
                return null;
            }
        
            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration.GetValue<string>("Jwt:ExpiryInMinutes")));
            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                ExpireAt = expiresAt
            };
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            await Task.Delay(1);
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("role", user.Role),
                new Claim("username", user.Username),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration.GetValue<string>("Jwt:ExpiryInMinutes"))),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}