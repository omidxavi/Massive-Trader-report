using Algorithm.Mdp.DataProvider.Instruments;
using Algorithm.Mdp.DataProvider.TechnicalInfo;
using Algorithm.Mdp.Models.Dto;
using ProfitLossCalculatorWebApi.Dto;
using ProfitLossCalculatorWebApi.Model;
using ProfitLossCalculatorWebApi.Services.Urls;

namespace ProfitLossCalculatorWebApi.Services;

public class MarketDataService : IMarketDataService
{
    private readonly Client _client;
    private readonly ICandlestickDataProvider _icaCandlestickDataProvider;
    public MarketDataService(HttpClient client)
    {
        _client = new Client(client);
    }



    public async Task<List<GetRayanDataResponse>> GetTransactionByNationalCode(string dsCode, string fromDate,
        string toDate,string nationalCode,  string size)
    {

        var url = new GenerateUrl().GenerateRayanUrl(dsCode, fromDate, toDate,nationalCode, size);
        var response = _client.GetDataAsync(url);

        var transactions = new List<GetRayanDataResponse>();
        foreach (var transaction in response)
        {
            transactions.Add(new GetRayanDataResponse()
            {
                BranchId = transaction.BranchId,
                BranchName = transaction.BranchName,
                CustomerId = transaction.CustomerId,
                DbsAccountNumber = transaction.DbsAccountNumber,
                CustomerFullName = transaction.CustomerFullName,
                TransactionDate = transaction.TransactionDate,
                IsAPurchase = transaction.IsAPurchase,
                Isin = transaction.InsMaxLcode,
                Amount = transaction.Amount,
                Quantity = transaction.Quantity,
                CbranchName = transaction.CbranchName,
                BrokerFee = transaction.Interest,
                BourseOrganizationFee = transaction.BourseCo+transaction.BourseOrg+transaction.DepositCo+transaction.ItManagement,
                Total = new Transactions().Total
            });
        }

        return transactions;
    }

    public async Task<List<GetRayanDataDetailResponse>> GetTransactionDetailsByNationalCode(string dsCode, string fromDate, string toDate, string nationalCode,
        string size)
    {
        var url = new GenerateUrl().GenerateDetailRayanUrl(dsCode, fromDate, toDate, nationalCode, size);
        var response = _client.GetDataDetailAsync(url);

        var transactions = new List<GetRayanDataDetailResponse>();
        foreach (var transaction in response)
        {
            transactions.Add(new GetRayanDataDetailResponse()
            {
                BranchId = transaction.BranchId,
                BranchName = transaction.BranchName,
                CustomerId = transaction.CustomerId,
                DbsAccountNumber = transaction.DbsAccountNumber,
                CustomerFullName = transaction.CustomerFullName,
                TransactionDate = transaction.TransactionDate,
                IsAPurchase = transaction.IsAPurchase,
                Isin = transaction.InsMaxLcode,
                price = transaction.Price,
                Quantity = transaction.Quantity,
                CbranchName = transaction.CbranchName,
                BrokerFee = transaction.Interest,
                BourseOrganizationFee = transaction.BourseCo+transaction.BourseOrg+transaction.DepositCo+transaction.ItManagement,
                Total = new Transactions().Total
            });
            
        }

        return transactions;
    }
    
}