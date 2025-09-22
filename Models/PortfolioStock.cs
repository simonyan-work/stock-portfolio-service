using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPortfolioAPI.Models
{
    public class PortfolioStock : BaseEntity
    {
        [Required]
        public int PortfolioId { get; set; }
        
        [Required]
        public int StockId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AveragePrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal GainLoss { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal GainLossPercentage { get; set; }
        
        public DateTime? LastUpdated { get; set; }
        
        // Navigation properties
        [ForeignKey("PortfolioId")]
        public virtual Portfolio Portfolio { get; set; } = null!;
        
        [ForeignKey("StockId")]
        public virtual Stock Stock { get; set; } = null!;
    }
}
