using System.ComponentModel.DataAnnotations;

namespace StockPortfolioAPI.Models.DTOs
{
    public class PortfolioDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Owner { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalGainLoss { get; set; }
        public decimal TotalGainLossPercentage { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PortfolioStockDto>? PortfolioStocks { get; set; }
    }
    
    public class CreatePortfolioDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Owner { get; set; } = string.Empty;
    }
    
    public class UpdatePortfolioDto
    {
        [StringLength(100)]
        public string? Name { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(50)]
        public string? Owner { get; set; }
        
        public bool? IsActive { get; set; }
    }
    
    public class PortfolioStockDto
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public string StockSymbol { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GainLoss { get; set; }
        public decimal GainLossPercentage { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
