namespace ProfitLossCalculatorWebApi.Persistence.Models;

public  class AssetBalance
{
    public long Id { get; set; }
    public long AssetId { get; set; }
    public decimal Qty { get; set; }
    public decimal Price { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime InsertionDateTime { get; set; }

    public Asset Asset { get; set; }
}