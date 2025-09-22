using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPortfolioAPI.Models
{
    public enum TransactionType
    {
        Buy = 1,
        Sell = 2
    }
    
    public class Transaction : BaseEntity
    {
        [Required]
        public int PortfolioId { get; set; }
        
        [Required]
        public int StockId { get; set; }
        
        [Required]
        public TransactionType Type { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Commission { get; set; } = 0;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Fees { get; set; } = 0;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }
        
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        // Navigation properties
        [ForeignKey("PortfolioId")]
        public virtual Portfolio Portfolio { get; set; } = null!;
        
        [ForeignKey("StockId")]
        public virtual Stock Stock { get; set; } = null!;
    }
}
