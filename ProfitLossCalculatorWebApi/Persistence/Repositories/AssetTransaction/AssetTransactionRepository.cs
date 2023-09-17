using ProfitLossCalculatorWebApi.Persistence.Models;

namespace ProfitLossCalculatorWebApi.Persistence.Repositories.AssetTransaction;

public class AssetTransactionRepository : IAssetTransactionRepository
{
    private readonly PostgresConnectionString _postgresConnectionString;

    public AssetTransactionRepository(PostgresConnectionString postgresConnectionString)
    {
        _postgresConnectionString = postgresConnectionString;
    }

    public async Task<List<Models.AssetTransaction>> GetAssetBalance(DateTime dateTime)
    {
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        var result = new List<Models.AssetTransaction>();
        var response = db.asset_transaction.Where(x => Convert.ToDateTime(x.DateTime) >= dateTime.Date);
        foreach (var asset in response)
        {
            result.Add(asset);
        }

        return (result);
    }


    public void AddToAssetTransaction(Models.AssetTransaction assetTransaction)
    {
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        db.asset_transaction.Add(assetTransaction);
        db.SaveChanges();
    }

    public void AddToAssetTransactionByList(List<Models.AssetTransaction> assetTransaction)
    {
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        db.asset_transaction.AddRange(assetTransaction);
        db.SaveChanges();
    }

    public async Task<List<Models.AssetTransaction>> GetAssetTransaction()
    {
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        var result = new List<Models.AssetTransaction>();
        var response = db.asset_transaction.OrderByDescending(x => x.DateTime
        ).Take(6).ToList();
        foreach (var data in response)
        {
            result.Add(data);
        }
        return result;
    }
    
    public async Task<List<Models.AssetTransaction>> GetAssetByAssetId()
    {
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        var response = db.asset_transaction.ToList();
        return response;
    }
}