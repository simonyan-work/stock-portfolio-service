using StockPortfolioAPI.Models;
using StockPortfolioAPI.Models.DTOs;

namespace StockPortfolioAPI.Services
{
    public interface IStockService
    {
        Task<IEnumerable<StockDto>> GetAllStocksAsync();
        Task<StockDto?> GetStockByIdAsync(int id);
        Task<StockDto?> GetStockBySymbolAsync(string symbol);
        Task<IEnumerable<StockDto>> SearchStocksAsync(string searchTerm);
        Task<StockDto> CreateStockAsync(CreateStockDto createStockDto);
        Task<StockDto?> UpdateStockAsync(int id, UpdateStockDto updateStockDto);
        Task<bool> DeleteStockAsync(int id);
        Task<bool> UpdateStockPriceAsync(int id, decimal newPrice);
        Task<bool> ExistsAsync(int id);
    }
}
