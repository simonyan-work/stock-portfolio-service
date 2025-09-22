using System.ComponentModel.DataAnnotations;

namespace StockPortfolioAPI.Models
{
    public class Stock : BaseEntity
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
        
        public DateTime? LastUpdated { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<PortfolioStock> PortfolioStocks { get; set; } = new List<PortfolioStock>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
