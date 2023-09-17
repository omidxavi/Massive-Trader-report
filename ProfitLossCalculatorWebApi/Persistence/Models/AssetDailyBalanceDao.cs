namespace ProfitLossCalculatorWebApi.Persistence.Models;

public class AssetDailyBalance
{
    public long Id { get; set; }

    public string AssetName { get; set; }
    public string Isin { get; set; }
    public decimal Quantity { get; set; }
    public decimal Value { get; set; }
    public string DateTime { get; set; }
    public DateTime? InsertionDateTime { get; set; }
}