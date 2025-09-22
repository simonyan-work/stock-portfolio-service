using Microsoft.EntityFrameworkCore;
using StockPortfolioAPI.Models;

namespace StockPortfolioAPI.Data
{
    public class StockPortfolioDbContext : DbContext
    {
        public StockPortfolioDbContext(DbContextOptions<StockPortfolioDbContext> options) : base(options)
        {
        }
        
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PortfolioStock> PortfolioStocks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Stock entity
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Symbol).IsUnique();
                entity.Property(e => e.Symbol).IsRequired().HasMaxLength(10);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Sector).HasMaxLength(50);
                entity.Property(e => e.Industry).HasMaxLength(50);
                entity.Property(e => e.Exchange).HasMaxLength(10);
                entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("USD");
                entity.Property(e => e.CurrentPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MarketCap).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Volume).HasColumnType("decimal(18,2)");
            });
            
            // Configure Portfolio entity
            modelBuilder.Entity<Portfolio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Owner).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TotalValue).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.TotalCost).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.TotalGainLoss).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.TotalGainLossPercentage).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            });
            
            // Configure PortfolioStock entity
            modelBuilder.Entity<PortfolioStock>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.AveragePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CurrentPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalValue).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.GainLoss).HasColumnType("decimal(18,2)");
                entity.Property(e => e.GainLossPercentage).HasColumnType("decimal(18,2)");
                
                // Configure relationships
                entity.HasOne(e => e.Portfolio)
                    .WithMany(p => p.PortfolioStocks)
                    .HasForeignKey(e => e.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Stock)
                    .WithMany(s => s.PortfolioStocks)
                    .HasForeignKey(e => e.StockId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                // Unique constraint on PortfolioId and StockId
                entity.HasIndex(e => new { e.PortfolioId, e.StockId }).IsUnique();
            });
            
            // Configure Transaction entity
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Commission).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Fees).HasColumnType("decimal(18,2)");
                entity.Property(e => e.NetAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Type).HasConversion<string>();
                
                // Configure relationships
                entity.HasOne(e => e.Portfolio)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(e => e.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Stock)
                    .WithMany(s => s.Transactions)
                    .HasForeignKey(e => e.StockId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configure soft delete
            modelBuilder.Entity<Stock>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Portfolio>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PortfolioStock>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Transaction>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
