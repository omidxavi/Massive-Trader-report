namespace ProfitLossCalculatorWebApi.Persistence.Models;

public class Asset
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string Isin { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<AssetBalance> Balances { get; set; }
    public virtual ICollection<AssetTransaction> Transactions { get; set; }
}