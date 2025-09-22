using System.ComponentModel.DataAnnotations;

namespace StockPortfolioAPI.Models.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public string PortfolioName { get; set; } = string.Empty;
        public int StockId { get; set; }
        public string StockSymbol { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? Commission { get; set; }
        public decimal? Fees { get; set; }
        public decimal NetAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    public class CreateTransactionDto
    {
        [Required]
        public int PortfolioId { get; set; }
        
        [Required]
        public int StockId { get; set; }
        
        [Required]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Commission cannot be negative")]
        public decimal? Commission { get; set; } = 0;
        
        [Range(0, double.MaxValue, ErrorMessage = "Fees cannot be negative")]
        public decimal? Fees { get; set; } = 0;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public DateTime? TransactionDate { get; set; }
    }
    
    public class UpdateTransactionDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal? Quantity { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? Price { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Commission cannot be negative")]
        public decimal? Commission { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Fees cannot be negative")]
        public decimal? Fees { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public DateTime? TransactionDate { get; set; }
    }
}
