using Microsoft.EntityFrameworkCore;
using StockPortfolioAPI.Data;
using StockPortfolioAPI.Models;

namespace StockPortfolioAPI.Repositories
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public StockRepository(StockPortfolioDbContext context) : base(context)
        {
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Symbol.ToLower() == symbol.ToLower());
        }

        public async Task<IEnumerable<Stock>> GetActiveStocksAsync()
        {
            return await _dbSet
                .Where(s => s.IsActive && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Stock>> SearchStocksAsync(string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();
            return await _dbSet
                .Where(s => s.IsActive && !s.IsDeleted && 
                    (s.Symbol.ToLower().Contains(lowerSearchTerm) ||
                     s.CompanyName.ToLower().Contains(lowerSearchTerm) ||
                     (s.Sector != null && s.Sector.ToLower().Contains(lowerSearchTerm)) ||
                     (s.Industry != null && s.Industry.ToLower().Contains(lowerSearchTerm))))
                .ToListAsync();
        }

        public async Task UpdateStockPriceAsync(int stockId, decimal newPrice)
        {
            var stock = await GetByIdAsync(stockId);
            if (stock != null)
            {
                stock.CurrentPrice = newPrice;
                stock.LastUpdated = DateTime.UtcNow;
                await UpdateAsync(stock);
            }
        }
    }
}
