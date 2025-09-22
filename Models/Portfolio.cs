using System.ComponentModel.DataAnnotations;

namespace StockPortfolioAPI.Models
{
    public class Portfolio : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Owner { get; set; } = string.Empty;
        
        public decimal TotalValue { get; set; } = 0;
        
        public decimal TotalCost { get; set; } = 0;
        
        public decimal TotalGainLoss { get; set; } = 0;
        
        public decimal TotalGainLossPercentage { get; set; } = 0;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<PortfolioStock> PortfolioStocks { get; set; } = new List<PortfolioStock>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
