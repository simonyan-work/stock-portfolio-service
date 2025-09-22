using StockPortfolioAPI.Models.DTOs;

namespace StockPortfolioAPI.Services
{
    public interface IPortfolioService
    {
        Task<IEnumerable<PortfolioDto>> GetAllPortfoliosAsync();
        Task<PortfolioDto?> GetPortfolioByIdAsync(int id);
        Task<IEnumerable<PortfolioDto>> GetPortfoliosByOwnerAsync(string owner);
        Task<PortfolioDto> CreatePortfolioAsync(CreatePortfolioDto createPortfolioDto);
        Task<PortfolioDto?> UpdatePortfolioAsync(int id, UpdatePortfolioDto updatePortfolioDto);
        Task<bool> DeletePortfolioAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<PortfolioDto?> GetPortfolioWithStocksAsync(int id);
        Task<bool> UpdatePortfolioValuesAsync(int id);
    }
}
