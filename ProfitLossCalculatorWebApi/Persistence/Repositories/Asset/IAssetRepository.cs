using ProfitLossCalculatorWebApi.Persistence.Models;

namespace ProfitLossCalculatorWebApi.Persistence.Repositories;

public interface IAssetRepository
{
    Task<List<Models.Asset>> GetAllAssets();
}