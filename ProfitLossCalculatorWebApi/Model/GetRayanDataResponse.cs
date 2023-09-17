namespace ProfitLossCalculatorWebApi.Model;

public class GetRayanDataResponse
{
    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public int CustomerId { get; set; }
    public string DbsAccountNumber { get; set; }
    public string CustomerFullName { get; set; }
    public string TransactionDate { get; set; }
    public int IsAPurchase { get; set; }
    public string Isin { get; set; }
    public decimal Amount { get; set; }
    public decimal Quantity { get; set; }
    public string CbranchName { get; set; }

    public int BrokerFee { get; set; }
    public int BourseOrganizationFee { get; set; }
    public int Total { get; set; }
    
}