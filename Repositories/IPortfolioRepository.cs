using StockPortfolioAPI.Models;

namespace StockPortfolioAPI.Repositories
{
    public interface IPortfolioRepository : IRepository<Portfolio>
    {
        Task<IEnumerable<Portfolio>> GetByOwnerAsync(string owner);
        Task<Portfolio?> GetWithStocksAsync(int id);
        Task<Portfolio?> GetWithTransactionsAsync(int id);
        Task<Portfolio?> GetWithStocksAndTransactionsAsync(int id);
        Task UpdatePortfolioValuesAsync(int portfolioId);
    }
}
