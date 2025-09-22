using Microsoft.EntityFrameworkCore;
using StockPortfolioAPI.Data;
using StockPortfolioAPI.Models;

namespace StockPortfolioAPI.Repositories
{
    public class PortfolioRepository : Repository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(StockPortfolioDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Portfolio>> GetByOwnerAsync(string owner)
        {
            return await _dbSet
                .Where(p => p.Owner == owner && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Portfolio?> GetWithStocksAsync(int id)
        {
            return await _dbSet
                .Include(p => p.PortfolioStocks)
                    .ThenInclude(ps => ps.Stock)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<Portfolio?> GetWithTransactionsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Transactions)
                    .ThenInclude(t => t.Stock)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<Portfolio?> GetWithStocksAndTransactionsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.PortfolioStocks)
                    .ThenInclude(ps => ps.Stock)
                .Include(p => p.Transactions)
                    .ThenInclude(t => t.Stock)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task UpdatePortfolioValuesAsync(int portfolioId)
        {
            var portfolio = await GetWithStocksAsync(portfolioId);
            if (portfolio == null) return;

            decimal totalValue = 0;
            decimal totalCost = 0;

            foreach (var portfolioStock in portfolio.PortfolioStocks)
            {
                portfolioStock.TotalValue = portfolioStock.Quantity * portfolioStock.CurrentPrice;
                portfolioStock.TotalCost = portfolioStock.Quantity * portfolioStock.AveragePrice;
                portfolioStock.GainLoss = portfolioStock.TotalValue - portfolioStock.TotalCost;
                portfolioStock.GainLossPercentage = portfolioStock.TotalCost > 0 
                    ? (portfolioStock.GainLoss / portfolioStock.TotalCost) * 100 
                    : 0;
                portfolioStock.LastUpdated = DateTime.UtcNow;

                totalValue += portfolioStock.TotalValue;
                totalCost += portfolioStock.TotalCost;
            }

            portfolio.TotalValue = totalValue;
            portfolio.TotalCost = totalCost;
            portfolio.TotalGainLoss = totalValue - totalCost;
            portfolio.TotalGainLossPercentage = totalCost > 0 
                ? (portfolio.TotalGainLoss / totalCost) * 100 
                : 0;

            await UpdateAsync(portfolio);
        }
    }
}
