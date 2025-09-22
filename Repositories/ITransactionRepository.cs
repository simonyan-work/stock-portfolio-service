using StockPortfolioAPI.Models;

namespace StockPortfolioAPI.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByPortfolioIdAsync(int portfolioId);
        Task<IEnumerable<Transaction>> GetByStockIdAsync(int stockId);
        Task<IEnumerable<Transaction>> GetByPortfolioAndStockAsync(int portfolioId, int stockId);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transaction>> GetByTypeAsync(TransactionType type);
        Task<decimal> GetTotalQuantityByPortfolioAndStockAsync(int portfolioId, int stockId);
    }
}
