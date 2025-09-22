using StockPortfolioAPI.Models;
using StockPortfolioAPI.Models.DTOs;
using StockPortfolioAPI.Repositories;

namespace StockPortfolioAPI.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<IEnumerable<StockDto>> GetAllStocksAsync()
        {
            var stocks = await _stockRepository.GetAllAsync();
            return stocks.Select(MapToDto);
        }

        public async Task<StockDto?> GetStockByIdAsync(int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            return stock != null ? MapToDto(stock) : null;
        }

        public async Task<StockDto?> GetStockBySymbolAsync(string symbol)
        {
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            return stock != null ? MapToDto(stock) : null;
        }

        public async Task<IEnumerable<StockDto>> SearchStocksAsync(string searchTerm)
        {
            var stocks = await _stockRepository.SearchStocksAsync(searchTerm);
            return stocks.Select(MapToDto);
        }

        public async Task<StockDto> CreateStockAsync(CreateStockDto createStockDto)
        {
            // Check if stock with symbol already exists
            var existingStock = await _stockRepository.GetBySymbolAsync(createStockDto.Symbol);
            if (existingStock != null)
            {
                throw new InvalidOperationException($"Stock with symbol '{createStockDto.Symbol}' already exists.");
            }

            var stock = new Stock
            {
                Symbol = createStockDto.Symbol.ToUpper(),
                CompanyName = createStockDto.CompanyName,
                Sector = createStockDto.Sector,
                Industry = createStockDto.Industry,
                Exchange = createStockDto.Exchange,
                Currency = createStockDto.Currency ?? "USD",
                CurrentPrice = createStockDto.CurrentPrice,
                MarketCap = createStockDto.MarketCap,
                Volume = createStockDto.Volume,
                LastUpdated = createStockDto.CurrentPrice.HasValue ? DateTime.UtcNow : null
            };

            await _stockRepository.AddAsync(stock);
            await _stockRepository.SaveChangesAsync();

            return MapToDto(stock);
        }

        public async Task<StockDto?> UpdateStockAsync(int id, UpdateStockDto updateStockDto)
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null) return null;

            // Update properties if provided
            if (!string.IsNullOrEmpty(updateStockDto.CompanyName))
                stock.CompanyName = updateStockDto.CompanyName;
            
            if (updateStockDto.Sector != null)
                stock.Sector = updateStockDto.Sector;
            
            if (updateStockDto.Industry != null)
                stock.Industry = updateStockDto.Industry;
            
            if (updateStockDto.Exchange != null)
                stock.Exchange = updateStockDto.Exchange;
            
            if (updateStockDto.Currency != null)
                stock.Currency = updateStockDto.Currency;
            
            if (updateStockDto.CurrentPrice.HasValue)
            {
                stock.CurrentPrice = updateStockDto.CurrentPrice.Value;
                stock.LastUpdated = DateTime.UtcNow;
            }
            
            if (updateStockDto.MarketCap.HasValue)
                stock.MarketCap = updateStockDto.MarketCap.Value;
            
            if (updateStockDto.Volume.HasValue)
                stock.Volume = updateStockDto.Volume.Value;
            
            if (updateStockDto.IsActive.HasValue)
                stock.IsActive = updateStockDto.IsActive.Value;

            stock.UpdatedAt = DateTime.UtcNow;

            await _stockRepository.UpdateAsync(stock);
            await _stockRepository.SaveChangesAsync();

            return MapToDto(stock);
        }

        public async Task<bool> DeleteStockAsync(int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null) return false;

            // Soft delete
            stock.IsDeleted = true;
            stock.UpdatedAt = DateTime.UtcNow;

            await _stockRepository.UpdateAsync(stock);
            await _stockRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateStockPriceAsync(int id, decimal newPrice)
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null) return false;

            await _stockRepository.UpdateStockPriceAsync(id, newPrice);
            await _stockRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _stockRepository.ExistsAsync(id);
        }

        private static StockDto MapToDto(Stock stock)
        {
            return new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Sector = stock.Sector,
                Industry = stock.Industry,
                Exchange = stock.Exchange,
                Currency = stock.Currency,
                CurrentPrice = stock.CurrentPrice,
                MarketCap = stock.MarketCap,
                Volume = stock.Volume,
                LastUpdated = stock.LastUpdated,
                IsActive = stock.IsActive,
                CreatedAt = stock.CreatedAt
            };
        }
    }
}
