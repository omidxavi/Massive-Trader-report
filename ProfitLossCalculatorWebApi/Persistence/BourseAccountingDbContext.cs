using Microsoft.EntityFrameworkCore;
using ProfitLossCalculatorWebApi.Persistence.Models;

namespace ProfitLossCalculatorWebApi.Persistence;

public class BourseAccountingDbContext : DbContext
{
    private readonly string _postgresConnectionString;

    public DbSet<Asset> asset { get; set; }
    public DbSet<AssetBalance> asset_balance { get; set; }
    public DbSet<AssetTransaction> asset_transaction { get; set; }
    
    public BourseAccountingDbContext(PostgresConnectionString postgresConnectionString)
    {
        _postgresConnectionString = postgresConnectionString.Value;
    }
    
    public BourseAccountingDbContext(string connectionString)
    {
        _postgresConnectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_postgresConnectionString)
            .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Asset

        {
            
            modelBuilder.Entity<AssetBalance>(en =>
            {
                en.ToTable("asset_balance");
            });
            
            modelBuilder.Entity<AssetTransaction>(en =>
            {
                en.ToTable("asset_transaction");


            });
            modelBuilder.Entity<Asset>(en =>
            {
                en.ToTable("asset");

                en.HasMany(a => a.Balances)
                    .WithOne(b => b.Asset)
                    .HasForeignKey(a => a.AssetId);
                
                en.HasMany(a => a.Transactions)
                    .WithOne(b => b.Asset)
                    .HasForeignKey(a => a.AssetId);
                
            });
            
            
        }

        #endregion
/*
        #region AssetBalance
        {
            modelBuilder.Entity<AssetDao>()
                .HasMany<AssetBalanceDao>()
                .WithOne()
                .HasForeignKey(e => e.AssetId)
                .IsRequired();
        }
        

        #endregion

        #region AssetTransaction

        {
            modelBuilder.Entity<AssetDao>()
                .HasMany<AssetTransactionDao>()
                .WithOne()
                .HasForeignKey(e => e.AssetId)
                .IsRequired();
        }

        #endregion
*/
    }
}