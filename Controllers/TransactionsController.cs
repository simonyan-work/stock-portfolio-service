using Microsoft.AspNetCore.Mvc;
using StockPortfolioAPI.Models.DTOs;
using StockPortfolioAPI.Services;

namespace StockPortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        /// <summary>
        /// Get all transactions
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionsAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all transactions");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get transaction by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                if (transaction == null)
                    return NotFound($"Transaction with ID {id} not found");

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting transaction with ID {TransactionId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get transactions by portfolio ID
        /// </summary>
        [HttpGet("portfolio/{portfolioId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByPortfolio(int portfolioId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByPortfolioIdAsync(portfolioId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting transactions for portfolio {PortfolioId}", portfolioId);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get transactions by stock ID
        /// </summary>
        [HttpGet("stock/{stockId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByStock(int stockId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByStockIdAsync(stockId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting transactions for stock {StockId}", stockId);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get transactions by date range
        /// </summary>
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest("Start date cannot be greater than end date");

                var transactions = await _transactionService.GetTransactionsByDateRangeAsync(startDate, endDate);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting transactions for date range {StartDate} to {EndDate}", startDate, endDate);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get current quantity for a stock in a portfolio
        /// </summary>
        [HttpGet("quantity/{portfolioId}/{stockId}")]
        public async Task<ActionResult<decimal>> GetCurrentQuantity(int portfolioId, int stockId)
        {
            try
            {
                var quantity = await _transactionService.GetCurrentQuantityAsync(portfolioId, stockId);
                return Ok(quantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting current quantity for portfolio {PortfolioId} and stock {StockId}", portfolioId, stockId);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Create a new transaction (buy or sell)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateTransaction(CreateTransactionDto createTransactionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var transaction = await _transactionService.CreateTransactionAsync(createTransactionDto);
                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating transaction");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Update an existing transaction
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionDto>> UpdateTransaction(int id, UpdateTransactionDto updateTransactionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var transaction = await _transactionService.UpdateTransactionAsync(id, updateTransactionDto);
                if (transaction == null)
                    return NotFound($"Transaction with ID {id} not found");

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating transaction with ID {TransactionId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Delete a transaction (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            try
            {
                var success = await _transactionService.DeleteTransactionAsync(id);
                if (!success)
                    return NotFound($"Transaction with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting transaction with ID {TransactionId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
