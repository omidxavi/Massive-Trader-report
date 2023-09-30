namespace ProfitLossCalculatorWebApi.Persistence.Repositories.AssetDailyBalance;

public interface IAssetDailyBalanceRepository
{
    
    Task<List<Models.AssetDailyBalance>> GetAssetByIsin(string isin);

    Task<List<Models.AssetDailyBalance>> GetLatestData();
    
    
    void Add(List<Models.AssetDailyBalance> assetDailyBalances);

}