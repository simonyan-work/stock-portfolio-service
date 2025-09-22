using System.ComponentModel.DataAnnotations;

namespace StockPortfolioAPI.Models.DTOs
{
    public class StockDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? Sector { get; set; }
        public string? Industry { get; set; }
        public string? Exchange { get; set; }
        public string? Currency { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? MarketCap { get; set; }
        public decimal? Volume { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    public class CreateStockDto
    {
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Sector { get; set; }
        
        [StringLength(50)]
        public string? Industry { get; set; }
        
        [StringLength(10)]
        public string? Exchange { get; set; }
        
        [StringLength(10)]
        public string? Currency { get; set; } = "USD";
        
        public decimal? CurrentPrice { get; set; }
        
        public decimal? MarketCap { get; set; }
        
        public decimal? Volume { get; set; }
    }
    
    public class UpdateStockDto
    {
        [StringLength(100)]
        public string? CompanyName { get; set; }
        
        [StringLength(50)]
        public string? Sector { get; set; }
        
        [StringLength(50)]
        public string? Industry { get; set; }
        
        [StringLength(10)]
        public string? Exchange { get; set; }
        
        [StringLength(10)]
        public string? Currency { get; set; }
        
        public decimal? CurrentPrice { get; set; }
        
        public decimal? MarketCap { get; set; }
        
        public decimal? Volume { get; set; }
        
        public bool? IsActive { get; set; }
    }
}
