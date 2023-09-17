namespace ProfitLossCalculatorWebApi.Persistence.Models;

public class AssetTransaction
{
    public long Id { get; set; }
    public long AssetId { get; set; }
    public decimal TotalTransaction { get; set; }
    public decimal TotalTransactionNum { get; set; }
    public int TotalTradeNum { get; set; }
    public int BuyTradeNum { get; set; }
    public int SellTradeNum { get; set; }
    public decimal TotalFee { get; set; }
    public decimal BrokerFee { get; set; }
    public decimal Benefit { get; set; }
    public decimal NetBenefit { get; set; }
    public decimal EntraDayBuyQty { get; set; }
    public decimal EntraDaySellQty { get; set; }
    public decimal AvgBuyPrice { get; set; }
    public decimal AvgSellPrice { get; set; }
    public decimal EntraDayDebit { get; set; }
    public decimal EntraDayCredit { get; set; }
    public decimal AssetDifference { get; set; }
    public string DateTime { get; set; }
    public DateTime InsertionDateTime { get; set; }

    public  Asset Asset { get; set; }
}