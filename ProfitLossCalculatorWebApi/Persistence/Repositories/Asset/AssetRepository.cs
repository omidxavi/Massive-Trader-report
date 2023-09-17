using ProfitLossCalculatorWebApi.Persistence.Models;

namespace ProfitLossCalculatorWebApi.Persistence.Repositories.Asset;

public class AssetRepository : IAssetRepository
{

    private readonly PostgresConnectionString _postgresConnectionString;

    public AssetRepository(PostgresConnectionString postgresConnectionString)
    {
        _postgresConnectionString = postgresConnectionString;
    }
    public async Task<List<Models.Asset>> GetAllAssets()
    {
        using var db = new BourseAccountingDbContext(_postgresConnectionString);
        var result = new List<Models.Asset>();
        var response = db.asset;
        foreach (var asset in response)
        {
            result.Add(asset);
        }
        return result;
    }
}