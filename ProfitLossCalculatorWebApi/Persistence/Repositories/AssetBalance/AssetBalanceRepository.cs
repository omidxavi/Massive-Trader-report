using ProfitLossCalculatorWebApi.Persistence.Models;

namespace ProfitLossCalculatorWebApi.Persistence.Repositories.AssetBalance;

public class AssetBalanceRepository : IAssetBalanceRepository
{
    
    private readonly PostgresConnectionString _postgresConnectionString;

    public AssetBalanceRepository(PostgresConnectionString postgresConnectionString)
    {
        _postgresConnectionString = postgresConnectionString;
    }
    public async Task<List<Models.AssetBalance>> GetAssetBalance(DateTime dateTime)
    {
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        var result = new List<Models.AssetBalance>();
        var response = db.asset_balance.Where(x => x.DateTime.Date >= dateTime.Date);
        foreach (var res in response)
        {
            result.Add(res);
        }
        return result;
    }

    public async Task<List<Models.AssetBalance>> GetAssetBalanceById(long id)
    {
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        var result = new List<Models.AssetBalance>();
        var response = db.asset_balance.Where(x => x.Asset.Id == id);
        foreach (var res in response)
        {
            result.Add(res);
        }
        return result;
    }

    public void AddToAssetBalance(Models.AssetBalance assetBalance)
    {
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        db.AddAsync(assetBalance);
    }
}