using StockPortfolioAPI.Models;
using StockPortfolioAPI.Models.DTOs;
using StockPortfolioAPI.Repositories;

namespace StockPortfolioAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockRepository _stockRepository;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IPortfolioRepository portfolioRepository,
            IStockRepository stockRepository)
        {
            _transactionRepository = transactionRepository;
            _portfolioRepository = portfolioRepository;
            _stockRepository = stockRepository;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return transactions.Select(MapToDto);
        }

        public async Task<TransactionDto?> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            return transaction != null ? MapToDto(transaction) : null;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByPortfolioIdAsync(int portfolioId)
        {
            var transactions = await _transactionRepository.GetByPortfolioIdAsync(portfolioId);
            return transactions.Select(MapToDto);
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByStockIdAsync(int stockId)
        {
            var transactions = await _transactionRepository.GetByStockIdAsync(stockId);
            return transactions.Select(MapToDto);
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = await _transactionRepository.GetByDateRangeAsync(startDate, endDate);
            return transactions.Select(MapToDto);
        }

        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto)
        {
            // Validate portfolio exists
            var portfolio = await _portfolioRepository.GetByIdAsync(createTransactionDto.PortfolioId);
            if (portfolio == null)
                throw new ArgumentException("Portfolio not found.");

            // Validate stock exists
            var stock = await _stockRepository.GetByIdAsync(createTransactionDto.StockId);
            if (stock == null)
                throw new ArgumentException("Stock not found.");

            // Parse transaction type
            if (!Enum.TryParse<TransactionType>(createTransactionDto.Type, true, out var transactionType))
                throw new ArgumentException("Invalid transaction type. Must be 'Buy' or 'Sell'.");

            // Calculate amounts
            var totalAmount = createTransactionDto.Quantity * createTransactionDto.Price;
            var commission = createTransactionDto.Commission ?? 0;
            var fees = createTransactionDto.Fees ?? 0;
            var netAmount = totalAmount + commission + fees;

            // For sell transactions, check if we have enough quantity
            if (transactionType == TransactionType.Sell)
            {
                var currentQuantity = await GetCurrentQuantityAsync(createTransactionDto.PortfolioId, createTransactionDto.StockId);
                if (currentQuantity < createTransactionDto.Quantity)
                    throw new InvalidOperationException($"Insufficient quantity. Current: {currentQuantity}, Trying to sell: {createTransactionDto.Quantity}");
            }

            var transaction = new Transaction
            {
                PortfolioId = createTransactionDto.PortfolioId,
                StockId = createTransactionDto.StockId,
                Type = transactionType,
                Quantity = createTransactionDto.Quantity,
                Price = createTransactionDto.Price,
                TotalAmount = totalAmount,
                Commission = commission,
                Fees = fees,
                NetAmount = netAmount,
                TransactionDate = createTransactionDto.TransactionDate ?? DateTime.UtcNow,
                Notes = createTransactionDto.Notes
            };

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            // Update portfolio values after transaction
            await _portfolioRepository.UpdatePortfolioValuesAsync(createTransactionDto.PortfolioId);
            await _portfolioRepository.SaveChangesAsync();

            return MapToDto(transaction);
        }

        public async Task<TransactionDto?> UpdateTransactionAsync(int id, UpdateTransactionDto updateTransactionDto)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) return null;

            // Update properties if provided
            if (updateTransactionDto.Quantity.HasValue)
                transaction.Quantity = updateTransactionDto.Quantity.Value;
            
            if (updateTransactionDto.Price.HasValue)
                transaction.Price = updateTransactionDto.Price.Value;
            
            if (updateTransactionDto.Commission.HasValue)
                transaction.Commission = updateTransactionDto.Commission.Value;
            
            if (updateTransactionDto.Fees.HasValue)
                transaction.Fees = updateTransactionDto.Fees.Value;
            
            if (updateTransactionDto.Notes != null)
                transaction.Notes = updateTransactionDto.Notes;
            
            if (updateTransactionDto.TransactionDate.HasValue)
                transaction.TransactionDate = updateTransactionDto.TransactionDate.Value;

            // Recalculate amounts
            transaction.TotalAmount = transaction.Quantity * transaction.Price;
            transaction.NetAmount = transaction.TotalAmount + (transaction.Commission ?? 0) + (transaction.Fees ?? 0);

            transaction.UpdatedAt = DateTime.UtcNow;

            await _transactionRepository.UpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            // Update portfolio values after transaction update
            await _portfolioRepository.UpdatePortfolioValuesAsync(transaction.PortfolioId);
            await _portfolioRepository.SaveChangesAsync();

            return MapToDto(transaction);
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) return false;

            // Soft delete
            transaction.IsDeleted = true;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _transactionRepository.UpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            // Update portfolio values after transaction deletion
            await _portfolioRepository.UpdatePortfolioValuesAsync(transaction.PortfolioId);
            await _portfolioRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _transactionRepository.ExistsAsync(id);
        }

        public async Task<decimal> GetCurrentQuantityAsync(int portfolioId, int stockId)
        {
            return await _transactionRepository.GetTotalQuantityByPortfolioAndStockAsync(portfolioId, stockId);
        }

        private static TransactionDto MapToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                PortfolioId = transaction.PortfolioId,
                PortfolioName = transaction.Portfolio?.Name ?? string.Empty,
                StockId = transaction.StockId,
                StockSymbol = transaction.Stock?.Symbol ?? string.Empty,
                StockName = transaction.Stock?.CompanyName ?? string.Empty,
                Type = transaction.Type.ToString(),
                Quantity = transaction.Quantity,
                Price = transaction.Price,
                TotalAmount = transaction.TotalAmount,
                Commission = transaction.Commission,
                Fees = transaction.Fees,
                NetAmount = transaction.NetAmount,
                TransactionDate = transaction.TransactionDate,
                Notes = transaction.Notes,
                CreatedAt = transaction.CreatedAt
            };
        }
    }
}
