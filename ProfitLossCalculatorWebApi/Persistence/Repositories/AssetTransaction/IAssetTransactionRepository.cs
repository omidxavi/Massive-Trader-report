using ProfitLossCalculatorWebApi.Persistence.Models;

namespace ProfitLossCalculatorWebApi.Persistence.Repositories.AssetTransaction;

public interface IAssetTransactionRepository
{
    //Task<List<AssetTransactionDao>> GetAssetTransaction(DateTime dateTime);
    //Task<List<AssetTransactionDao>> GetAssetTransactionById(long id);
    void AddToAssetTransaction(Models.AssetTransaction assetTransaction);
    void AddToAssetTransactionByList(List<Models.AssetTransaction> assetTransaction);
    
    Task<List<Models.AssetTransaction>> GetAssetTransaction();
    Task<List<Models.AssetTransaction>> GetAssetByAssetId();

}