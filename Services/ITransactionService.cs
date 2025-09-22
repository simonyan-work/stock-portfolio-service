using StockPortfolioAPI.Models.DTOs;

namespace StockPortfolioAPI.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
        Task<TransactionDto?> GetTransactionByIdAsync(int id);
        Task<IEnumerable<TransactionDto>> GetTransactionsByPortfolioIdAsync(int portfolioId);
        Task<IEnumerable<TransactionDto>> GetTransactionsByStockIdAsync(int stockId);
        Task<IEnumerable<TransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto);
        Task<TransactionDto?> UpdateTransactionAsync(int id, UpdateTransactionDto updateTransactionDto);
        Task<bool> DeleteTransactionAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetCurrentQuantityAsync(int portfolioId, int stockId);
    }
}
