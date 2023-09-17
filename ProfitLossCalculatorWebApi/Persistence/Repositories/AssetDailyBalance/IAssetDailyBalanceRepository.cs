namespace ProfitLossCalculatorWebApi.Persistence.Repositories.AssetDailyBalance;

public interface IAssetDailyBalanceRepository
{
    
    Task<List<Models.AssetDailyBalance>> GetAssetByIsin(string isin);
    
    
    void Add(List<Models.AssetDailyBalance> assetDailyBalances);

}