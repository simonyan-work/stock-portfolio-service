using StockPortfolioAPI.Models;

namespace StockPortfolioAPI.Repositories
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<IEnumerable<Stock>> GetActiveStocksAsync();
        Task<IEnumerable<Stock>> SearchStocksAsync(string searchTerm);
        Task UpdateStockPriceAsync(int stockId, decimal newPrice);
    }
}
