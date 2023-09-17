namespace ProfitLossCalculatorWebApi.Services.Urls;

public class GenerateUrl
{

    public string GenerateRayanUrl(string dsCode,string fromDate, string toDate ,string nationalCode,  string size)
    {
        return 
            $"http://rayan-gw.sntinfra.local/api/v1/transactions/aggregated?dsCode={dsCode}&startDate={fromDate}&endDate={toDate}&nationalCode={nationalCode}&size={size}";
    }
    
    public string GenerateDetailRayanUrl(string dsCode,string fromDate, string toDate ,string nationalCode,  string size)
    {
        return 
            $"http://rayan-gw.sntinfra.local/api/v1/transactions/separated?dsCode={dsCode}&startDate={fromDate}&endDate={toDate}&nationalCode={nationalCode}&size={size}";
    }
    
}