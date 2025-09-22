using Microsoft.AspNetCore.Mvc;
using StockPortfolioAPI.Models.DTOs;
using StockPortfolioAPI.Services;

namespace StockPortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfoliosController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ILogger<PortfoliosController> _logger;

        public PortfoliosController(IPortfolioService portfolioService, ILogger<PortfoliosController> logger)
        {
            _portfolioService = portfolioService;
            _logger = logger;
        }

        /// <summary>
        /// Get all portfolios
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortfolioDto>>> GetPortfolios()
        {
            try
            {
                var portfolios = await _portfolioService.GetAllPortfoliosAsync();
                return Ok(portfolios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all portfolios");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get portfolio by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PortfolioDto>> GetPortfolio(int id)
        {
            try
            {
                var portfolio = await _portfolioService.GetPortfolioByIdAsync(id);
                if (portfolio == null)
                    return NotFound($"Portfolio with ID {id} not found");

                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting portfolio with ID {PortfolioId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get portfolio with stocks
        /// </summary>
        [HttpGet("{id}/stocks")]
        public async Task<ActionResult<PortfolioDto>> GetPortfolioWithStocks(int id)
        {
            try
            {
                var portfolio = await _portfolioService.GetPortfolioWithStocksAsync(id);
                if (portfolio == null)
                    return NotFound($"Portfolio with ID {id} not found");

                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting portfolio with stocks for ID {PortfolioId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get portfolios by owner
        /// </summary>
        [HttpGet("owner/{owner}")]
        public async Task<ActionResult<IEnumerable<PortfolioDto>>> GetPortfoliosByOwner(string owner)
        {
            try
            {
                var portfolios = await _portfolioService.GetPortfoliosByOwnerAsync(owner);
                return Ok(portfolios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting portfolios for owner {Owner}", owner);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Create a new portfolio
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PortfolioDto>> CreatePortfolio(CreatePortfolioDto createPortfolioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var portfolio = await _portfolioService.CreatePortfolioAsync(createPortfolioDto);
                return CreatedAtAction(nameof(GetPortfolio), new { id = portfolio.Id }, portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating portfolio");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Update an existing portfolio
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<PortfolioDto>> UpdatePortfolio(int id, UpdatePortfolioDto updatePortfolioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var portfolio = await _portfolioService.UpdatePortfolioAsync(id, updatePortfolioDto);
                if (portfolio == null)
                    return NotFound($"Portfolio with ID {id} not found");

                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating portfolio with ID {PortfolioId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Update portfolio values (recalculate)
        /// </summary>
        [HttpPut("{id}/recalculate")]
        public async Task<ActionResult> RecalculatePortfolio(int id)
        {
            try
            {
                var success = await _portfolioService.UpdatePortfolioValuesAsync(id);
                if (!success)
                    return NotFound($"Portfolio with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while recalculating portfolio with ID {PortfolioId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Delete a portfolio (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePortfolio(int id)
        {
            try
            {
                var success = await _portfolioService.DeletePortfolioAsync(id);
                if (!success)
                    return NotFound($"Portfolio with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting portfolio with ID {PortfolioId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
