namespace ProfitLossCalculatorWebApi.Persistence.Repositories.AssetDailyBalance;

public class AssetDailyBalanceRepository : IAssetDailyBalanceRepository
{
    
    
    private readonly PostgresConnectionString _postgresConnectionString;

    public AssetDailyBalanceRepository(PostgresConnectionString postgresConnectionString)
    {
        _postgresConnectionString = postgresConnectionString;
    }
    
    
    public async Task<List<Models.AssetDailyBalance>> GetAssetByIsin(string isin)
    {
        
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        var result = new List<Models.AssetDailyBalance>();
        var response =db.asset_daily_balance.OrderByDescending(x=>x.InsertionDateTime).Where(x => x.Isin == isin
        ).ToList();
        foreach (var data in response)
        {
            result.Add(data);
        }
        return result;
        
    }

    public void Add(List<Models.AssetDailyBalance> assetDailyBalances)
    {
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        db.asset_daily_balance.AddRange(assetDailyBalances);
        db.SaveChanges();
    }
}