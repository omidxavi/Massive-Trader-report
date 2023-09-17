using ProfitLossCalculatorWebApi.Model;

namespace ProfitLossCalculatorWebApi.Services;

public interface IMarketDataService
{
    Task<List<GetRayanDataResponse>> GetTransactionByNationalCode(string dsCode, string fromDate, string toDate,
        string nationalCode, string size);
    
    Task<List<GetRayanDataDetailResponse>> GetTransactionDetailsByNationalCode(string dsCode, string fromDate, string toDate,
        string nationalCode, string size);
}