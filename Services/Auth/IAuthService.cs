using StockPortfolioAPI.Models.Auth;

namespace StockPortfolioAPI.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<User?> GetUserByUsernameAsync(string username);
        string GenerateJwtToken(User user);
    }
}