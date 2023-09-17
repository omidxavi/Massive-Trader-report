using Microsoft.AspNetCore.Mvc;
using ProfitLossCalculatorWebApi.Handler;
using ProfitLossCalculatorWebApi.Persistence.Models;
using ProfitLossCalculatorWebApi.Persistence.Repositories.AssetTransaction;

namespace ProfitLossCalculatorWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly QueryHandler _queryHandler;
    
    public DataController(QueryHandler queryHandler)
    {
        _queryHandler = queryHandler;
    }

    /// <summary>
    /// Date time format : 1402/06/01 and between only one month
    /// </summary>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    [HttpGet("InvoiceMaker")]
    public async Task<ActionResult<List<AssetTransaction>>> InvoiceMaker([FromQuery]  string? fromDate,[FromQuery] string? toDate)
    {
        var result = await _queryHandler.GetDataFromRayan(fromDate,toDate);
        return result;
    }
    [HttpGet("Aggregation")]
    public async Task<ActionResult<List<AssetTransaction>>> Aggregation()
    {
        var result = await _queryHandler.GetAggregation();
        return result;
    }

    [HttpGet("DailyReport")]
    public async Task<ActionResult<List<AssetDailyBalance>>> DailyReport([FromQuery] string? fromDate,
        [FromQuery] string? toDate)
    {
        var result = await _queryHandler.CalculateDailyBalance(fromDate, toDate);
        return result;
    }
    
}