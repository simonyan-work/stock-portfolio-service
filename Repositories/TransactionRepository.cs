using Microsoft.EntityFrameworkCore;
using StockPortfolioAPI.Data;
using StockPortfolioAPI.Models;

namespace StockPortfolioAPI.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(StockPortfolioDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Transaction>> GetByPortfolioIdAsync(int portfolioId)
        {
            return await _dbSet
                .Include(t => t.Stock)
                .Include(t => t.Portfolio)
                .Where(t => t.PortfolioId == portfolioId && !t.IsDeleted)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByStockIdAsync(int stockId)
        {
            return await _dbSet
                .Include(t => t.Stock)
                .Include(t => t.Portfolio)
                .Where(t => t.StockId == stockId && !t.IsDeleted)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByPortfolioAndStockAsync(int portfolioId, int stockId)
        {
            return await _dbSet
                .Include(t => t.Stock)
                .Include(t => t.Portfolio)
                .Where(t => t.PortfolioId == portfolioId && t.StockId == stockId && !t.IsDeleted)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(t => t.Stock)
                .Include(t => t.Portfolio)
                .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate && !t.IsDeleted)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByTypeAsync(TransactionType type)
        {
            return await _dbSet
                .Include(t => t.Stock)
                .Include(t => t.Portfolio)
                .Where(t => t.Type == type && !t.IsDeleted)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalQuantityByPortfolioAndStockAsync(int portfolioId, int stockId)
        {
            var buyQuantity = await _dbSet
                .Where(t => t.PortfolioId == portfolioId && t.StockId == stockId && t.Type == TransactionType.Buy && !t.IsDeleted)
                .SumAsync(t => t.Quantity);

            var sellQuantity = await _dbSet
                .Where(t => t.PortfolioId == portfolioId && t.StockId == stockId && t.Type == TransactionType.Sell && !t.IsDeleted)
                .SumAsync(t => t.Quantity);

            return buyQuantity - sellQuantity;
        }
    }
}
