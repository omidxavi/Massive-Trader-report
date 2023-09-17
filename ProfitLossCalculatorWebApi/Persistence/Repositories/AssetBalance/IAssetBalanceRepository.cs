using ProfitLossCalculatorWebApi.Persistence.Models;

namespace ProfitLossCalculatorWebApi.Persistence.Repositories.AssetBalance;

public interface IAssetBalanceRepository
{
     Task<List<Models.AssetBalance>> GetAssetBalance(DateTime dateTime);
     Task<List<Models.AssetBalance>> GetAssetBalanceById(long id);
     void AddToAssetBalance(Models.AssetBalance assetBalance);
}                        