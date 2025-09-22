using StockPortfolioAPI.Models;
using StockPortfolioAPI.Models.DTOs;
using StockPortfolioAPI.Repositories;

namespace StockPortfolioAPI.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public async Task<IEnumerable<PortfolioDto>> GetAllPortfoliosAsync()
        {
            var portfolios = await _portfolioRepository.GetAllAsync();
            return portfolios.Select(MapToDto);
        }

        public async Task<PortfolioDto?> GetPortfolioByIdAsync(int id)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(id);
            return portfolio != null ? MapToDto(portfolio) : null;
        }

        public async Task<IEnumerable<PortfolioDto>> GetPortfoliosByOwnerAsync(string owner)
        {
            var portfolios = await _portfolioRepository.GetByOwnerAsync(owner);
            return portfolios.Select(MapToDto);
        }

        public async Task<PortfolioDto> CreatePortfolioAsync(CreatePortfolioDto createPortfolioDto)
        {
            var portfolio = new Portfolio
            {
                Name = createPortfolioDto.Name,
                Description = createPortfolioDto.Description,
                Owner = createPortfolioDto.Owner
            };

            await _portfolioRepository.AddAsync(portfolio);
            await _portfolioRepository.SaveChangesAsync();

            return MapToDto(portfolio);
        }

        public async Task<PortfolioDto?> UpdatePortfolioAsync(int id, UpdatePortfolioDto updatePortfolioDto)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(id);
            if (portfolio == null) return null;

            if (!string.IsNullOrEmpty(updatePortfolioDto.Name))
                portfolio.Name = updatePortfolioDto.Name;
            
            if (updatePortfolioDto.Description != null)
                portfolio.Description = updatePortfolioDto.Description;
            
            if (!string.IsNullOrEmpty(updatePortfolioDto.Owner))
                portfolio.Owner = updatePortfolioDto.Owner;
            
            if (updatePortfolioDto.IsActive.HasValue)
                portfolio.IsActive = updatePortfolioDto.IsActive.Value;

            portfolio.UpdatedAt = DateTime.UtcNow;

            await _portfolioRepository.UpdateAsync(portfolio);
            await _portfolioRepository.SaveChangesAsync();

            return MapToDto(portfolio);
        }

        public async Task<bool> DeletePortfolioAsync(int id)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(id);
            if (portfolio == null) return false;

            // Soft delete
            portfolio.IsDeleted = true;
            portfolio.UpdatedAt = DateTime.UtcNow;

            await _portfolioRepository.UpdateAsync(portfolio);
            await _portfolioRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _portfolioRepository.ExistsAsync(id);
        }

        public async Task<PortfolioDto?> GetPortfolioWithStocksAsync(int id)
        {
            var portfolio = await _portfolioRepository.GetWithStocksAsync(id);
            return portfolio != null ? MapToDtoWithStocks(portfolio) : null;
        }

        public async Task<bool> UpdatePortfolioValuesAsync(int id)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(id);
            if (portfolio == null) return false;

            await _portfolioRepository.UpdatePortfolioValuesAsync(id);
            await _portfolioRepository.SaveChangesAsync();

            return true;
        }

        private static PortfolioDto MapToDto(Portfolio portfolio)
        {
            return new PortfolioDto
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                Description = portfolio.Description,
                Owner = portfolio.Owner,
                TotalValue = portfolio.TotalValue,
                TotalCost = portfolio.TotalCost,
                TotalGainLoss = portfolio.TotalGainLoss,
                TotalGainLossPercentage = portfolio.TotalGainLossPercentage,
                IsActive = portfolio.IsActive,
                CreatedAt = portfolio.CreatedAt
            };
        }

        private static PortfolioDto MapToDtoWithStocks(Portfolio portfolio)
        {
            var dto = MapToDto(portfolio);
            dto.PortfolioStocks = portfolio.PortfolioStocks.Select(ps => new PortfolioStockDto
            {
                Id = ps.Id,
                StockId = ps.StockId,
                StockSymbol = ps.Stock.Symbol,
                StockName = ps.Stock.CompanyName,
                Quantity = ps.Quantity,
                AveragePrice = ps.AveragePrice,
                CurrentPrice = ps.CurrentPrice,
                TotalValue = ps.TotalValue,
                TotalCost = ps.TotalCost,
                GainLoss = ps.GainLoss,
                GainLossPercentage = ps.GainLossPercentage,
                LastUpdated = ps.LastUpdated
            }).ToList();

            return dto;
        }
    }
}
