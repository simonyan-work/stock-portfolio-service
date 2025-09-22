using Microsoft.AspNetCore.Mvc;
using StockPortfolioAPI.Models.DTOs;
using StockPortfolioAPI.Services;

namespace StockPortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IStockService stockService, ILogger<StocksController> logger)
        {
            _stockService = stockService;
            _logger = logger;
        }

        /// <summary>
        /// Get all stocks
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockDto>>> GetStocks()
        {
            try
            {
                var stocks = await _stockService.GetAllStocksAsync();
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all stocks");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get stock by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<StockDto>> GetStock(int id)
        {
            try
            {
                var stock = await _stockService.GetStockByIdAsync(id);
                if (stock == null)
                    return NotFound($"Stock with ID {id} not found");

                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting stock with ID {StockId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get stock by symbol
        /// </summary>
        [HttpGet("symbol/{symbol}")]
        public async Task<ActionResult<StockDto>> GetStockBySymbol(string symbol)
        {
            try
            {
                var stock = await _stockService.GetStockBySymbolAsync(symbol);
                if (stock == null)
                    return NotFound($"Stock with symbol {symbol} not found");

                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting stock with symbol {Symbol}", symbol);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Search stocks by symbol, company name, sector, or industry
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<StockDto>>> SearchStocks([FromQuery] string q)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                    return BadRequest("Search query cannot be empty");

                var stocks = await _stockService.SearchStocksAsync(q);
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching stocks with query {Query}", q);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Create a new stock
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<StockDto>> CreateStock(CreateStockDto createStockDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var stock = await _stockService.CreateStockAsync(createStockDto);
                return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, stock);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating stock");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Update an existing stock
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<StockDto>> UpdateStock(int id, UpdateStockDto updateStockDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var stock = await _stockService.UpdateStockAsync(id, updateStockDto);
                if (stock == null)
                    return NotFound($"Stock with ID {id} not found");

                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating stock with ID {StockId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Update stock price
        /// </summary>
        [HttpPut("{id}/price")]
        public async Task<ActionResult> UpdateStockPrice(int id, [FromBody] decimal newPrice)
        {
            try
            {
                if (newPrice <= 0)
                    return BadRequest("Price must be greater than 0");

                var success = await _stockService.UpdateStockPriceAsync(id, newPrice);
                if (!success)
                    return NotFound($"Stock with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating stock price for ID {StockId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Delete a stock (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStock(int id)
        {
            try
            {
                var success = await _stockService.DeleteStockAsync(id);
                if (!success)
                    return NotFound($"Stock with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting stock with ID {StockId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
