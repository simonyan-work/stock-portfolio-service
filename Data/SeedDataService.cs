using Microsoft.EntityFrameworkCore;
using StockPortfolioAPI.Models;

namespace StockPortfolioAPI.Data
{
    public static class SeedDataService
    {
        public static async Task SeedAsync(StockPortfolioDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if data already exists
            if (await context.Stocks.AnyAsync())
                return;

            // Seed Stocks
            var stocks = new List<Stock>
            {
                new Stock
                {
                    Symbol = "AAPL",
                    CompanyName = "Apple Inc.",
                    Sector = "Technology",
                    Industry = "Consumer Electronics",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    CurrentPrice = 175.50m,
                    MarketCap = 2800000000000m,
                    Volume = 50000000,
                    LastUpdated = DateTime.UtcNow,
                    IsActive = true
                },
                new Stock
                {
                    Symbol = "MSFT",
                    CompanyName = "Microsoft Corporation",
                    Sector = "Technology",
                    Industry = "Software",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    CurrentPrice = 350.25m,
                    MarketCap = 2600000000000m,
                    Volume = 30000000,
                    LastUpdated = DateTime.UtcNow,
                    IsActive = true
                },
                new Stock
                {
                    Symbol = "GOOGL",
                    CompanyName = "Alphabet Inc.",
                    Sector = "Technology",
                    Industry = "Internet Services",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    CurrentPrice = 140.75m,
                    MarketCap = 1800000000000m,
                    Volume = 25000000,
                    LastUpdated = DateTime.UtcNow,
                    IsActive = true
                },
                new Stock
                {
                    Symbol = "TSLA",
                    CompanyName = "Tesla, Inc.",
                    Sector = "Consumer Discretionary",
                    Industry = "Electric Vehicles",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    CurrentPrice = 250.00m,
                    MarketCap = 800000000000m,
                    Volume = 40000000,
                    LastUpdated = DateTime.UtcNow,
                    IsActive = true
                },
                new Stock
                {
                    Symbol = "AMZN",
                    CompanyName = "Amazon.com, Inc.",
                    Sector = "Consumer Discretionary",
                    Industry = "E-commerce",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    CurrentPrice = 145.30m,
                    MarketCap = 1500000000000m,
                    Volume = 35000000,
                    LastUpdated = DateTime.UtcNow,
                    IsActive = true
                },
                new Stock
                {
                    Symbol = "NVDA",
                    CompanyName = "NVIDIA Corporation",
                    Sector = "Technology",
                    Industry = "Semiconductors",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    CurrentPrice = 450.80m,
                    MarketCap = 1100000000000m,
                    Volume = 60000000,
                    LastUpdated = DateTime.UtcNow,
                    IsActive = true
                }
            };

            await context.Stocks.AddRangeAsync(stocks);
            await context.SaveChangesAsync();

            // Seed Portfolios
            var portfolios = new List<Portfolio>
            {
                new Portfolio
                {
                    Name = "Tech Growth Portfolio",
                    Description = "A diversified portfolio focused on technology growth stocks",
                    Owner = "John Doe",
                    IsActive = true
                },
                new Portfolio
                {
                    Name = "Conservative Portfolio",
                    Description = "A conservative portfolio with stable dividend stocks",
                    Owner = "Jane Smith",
                    IsActive = true
                },
                new Portfolio
                {
                    Name = "ESG Portfolio",
                    Description = "Environmental, Social, and Governance focused investments",
                    Owner = "John Doe",
                    IsActive = true
                }
            };

            await context.Portfolios.AddRangeAsync(portfolios);
            await context.SaveChangesAsync();

            // Seed some sample transactions
            var transactions = new List<Transaction>
            {
                // Tech Growth Portfolio transactions
                new Transaction
                {
                    PortfolioId = 1,
                    StockId = 1, // AAPL
                    Type = TransactionType.Buy,
                    Quantity = 100,
                    Price = 170.00m,
                    TotalAmount = 17000.00m,
                    Commission = 9.99m,
                    Fees = 0.00m,
                    NetAmount = 17009.99m,
                    TransactionDate = DateTime.UtcNow.AddDays(-30),
                    Notes = "Initial purchase of Apple stock"
                },
                new Transaction
                {
                    PortfolioId = 1,
                    StockId = 2, // MSFT
                    Type = TransactionType.Buy,
                    Quantity = 50,
                    Price = 340.00m,
                    TotalAmount = 17000.00m,
                    Commission = 9.99m,
                    Fees = 0.00m,
                    NetAmount = 17009.99m,
                    TransactionDate = DateTime.UtcNow.AddDays(-25),
                    Notes = "Microsoft stock purchase"
                },
                new Transaction
                {
                    PortfolioId = 1,
                    StockId = 6, // NVDA
                    Type = TransactionType.Buy,
                    Quantity = 25,
                    Price = 420.00m,
                    TotalAmount = 10500.00m,
                    Commission = 9.99m,
                    Fees = 0.00m,
                    NetAmount = 10509.99m,
                    TransactionDate = DateTime.UtcNow.AddDays(-20),
                    Notes = "NVIDIA stock purchase"
                },
                // Conservative Portfolio transactions
                new Transaction
                {
                    PortfolioId = 2,
                    StockId = 1, // AAPL
                    Type = TransactionType.Buy,
                    Quantity = 50,
                    Price = 165.00m,
                    TotalAmount = 8250.00m,
                    Commission = 9.99m,
                    Fees = 0.00m,
                    NetAmount = 8259.99m,
                    TransactionDate = DateTime.UtcNow.AddDays(-15),
                    Notes = "Conservative Apple position"
                },
                new Transaction
                {
                    PortfolioId = 2,
                    StockId = 2, // MSFT
                    Type = TransactionType.Buy,
                    Quantity = 30,
                    Price = 345.00m,
                    TotalAmount = 10350.00m,
                    Commission = 9.99m,
                    Fees = 0.00m,
                    NetAmount = 10359.99m,
                    TransactionDate = DateTime.UtcNow.AddDays(-10),
                    Notes = "Conservative Microsoft position"
                }
            };

            await context.Transactions.AddRangeAsync(transactions);
            await context.SaveChangesAsync();

            // Create PortfolioStock entries based on transactions
            var portfolioStocks = new List<PortfolioStock>();

            // Group transactions by portfolio and stock to calculate holdings
            var holdings = transactions
                .GroupBy(t => new { t.PortfolioId, t.StockId })
                .Select(g => new
                {
                    PortfolioId = g.Key.PortfolioId,
                    StockId = g.Key.StockId,
                    Quantity = g.Sum(t => t.Type == TransactionType.Buy ? t.Quantity : -t.Quantity),
                    TotalCost = g.Where(t => t.Type == TransactionType.Buy).Sum(t => t.NetAmount),
                    AveragePrice = g.Where(t => t.Type == TransactionType.Buy).Sum(t => t.NetAmount) / 
                                  g.Where(t => t.Type == TransactionType.Buy).Sum(t => t.Quantity)
                })
                .Where(h => h.Quantity > 0);

            foreach (var holding in holdings)
            {
                var stock = stocks.First(s => s.Id == holding.StockId);
                var portfolioStock = new PortfolioStock
                {
                    PortfolioId = holding.PortfolioId,
                    StockId = holding.StockId,
                    Quantity = holding.Quantity,
                    AveragePrice = holding.AveragePrice,
                    CurrentPrice = stock.CurrentPrice ?? 0,
                    TotalValue = holding.Quantity * (stock.CurrentPrice ?? 0),
                    TotalCost = holding.TotalCost,
                    GainLoss = (holding.Quantity * (stock.CurrentPrice ?? 0)) - holding.TotalCost,
                    GainLossPercentage = holding.TotalCost > 0 ? 
                        (((holding.Quantity * (stock.CurrentPrice ?? 0)) - holding.TotalCost) / holding.TotalCost) * 100 : 0,
                    LastUpdated = DateTime.UtcNow
                };

                portfolioStocks.Add(portfolioStock);
            }

            await context.PortfolioStocks.AddRangeAsync(portfolioStocks);
            await context.SaveChangesAsync();

            // Update portfolio totals
            foreach (var portfolio in portfolios)
            {
                var portfolioStocksForPortfolio = portfolioStocks.Where(ps => ps.PortfolioId == portfolio.Id).ToList();
                
                portfolio.TotalValue = portfolioStocksForPortfolio.Sum(ps => ps.TotalValue);
                portfolio.TotalCost = portfolioStocksForPortfolio.Sum(ps => ps.TotalCost);
                portfolio.TotalGainLoss = portfolio.TotalValue - portfolio.TotalCost;
                portfolio.TotalGainLossPercentage = portfolio.TotalCost > 0 ? 
                    (portfolio.TotalGainLoss / portfolio.TotalCost) * 100 : 0;
            }

            await context.SaveChangesAsync();
        }
    }
}
