using Algorithm.Mdp.DataProvider.TechnicalInfo;
using Algorithm.Mdp.Models.Dto;
using ProfitLossCalculatorWebApi.Persistence.Models;
using ProfitLossCalculatorWebApi.Persistence.Repositories;
using ProfitLossCalculatorWebApi.Persistence.Repositories.AssetDailyBalance;
using ProfitLossCalculatorWebApi.Persistence.Repositories.AssetTransaction;
using ProfitLossCalculatorWebApi.Services;

namespace ProfitLossCalculatorWebApi.Handler;

public class QueryHandler
{
    private readonly IMarketDataService _marketDataService;
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetTransactionRepository _assetTransactionRepository;
    private readonly IAssetDailyBalanceRepository _assetDailyBalanceRepository;
    private readonly ICandlestickDataProvider _candlestickDataProvider;
    private readonly ReportManager _reportManager;

    public QueryHandler(IMarketDataService marketDataService, ReportManager reportManager,
        IAssetRepository assetRepository, IAssetTransactionRepository assetTransactionRepository,
        IAssetDailyBalanceRepository assetDailyBalanceRepository,
        ICandlestickDataProvider candlestickDataProvider)
    {
        _marketDataService = marketDataService;
        _reportManager = reportManager;
        _assetRepository = assetRepository;
        _assetTransactionRepository = assetTransactionRepository;
        _assetDailyBalanceRepository = assetDailyBalanceRepository;
        _candlestickDataProvider = candlestickDataProvider;
    }

    public async Task<List<AssetTransaction>> GetDataFromRayan(string fromDate, string toDate)
    {
        var datalist1 = _marketDataService.GetTransactionByNationalCode(GeneralOptions.GeneralOption1.DsCode, fromDate,
            toDate
            , GeneralOptions.GeneralOption1.NationalCode, GeneralOptions.GeneralOption1.Size).Result;

        var datalist2 = _marketDataService.GetTransactionByNationalCode(GeneralOptions.GeneralOption2.DsCode2, fromDate,
            toDate
            , GeneralOptions.GeneralOption2.NationalCode2, GeneralOptions.GeneralOption2.Size2).Result;


        var dataListDetail1 = _marketDataService.GetTransactionDetailsByNationalCode(
            GeneralOptions.GeneralOption1.DsCode,
            fromDate,
            toDate
            , GeneralOptions.GeneralOption1.NationalCode, GeneralOptions.GeneralOption1.Size).Result;

        var dataListDetail2 = _marketDataService.GetTransactionDetailsByNationalCode(
            GeneralOptions.GeneralOption2.DsCode2,
            fromDate,
            toDate
            , GeneralOptions.GeneralOption2.NationalCode2, GeneralOptions.GeneralOption2.Size2).Result;


        var transactions = new List<AssetTransaction>();

        var assets = await _assetRepository.GetAllAssets();
        foreach (var asset in assets)
        {
            var rayan1 = datalist1.Where(x => x.Isin == asset.Isin).ToList(); //&& x.BranchId==2097321063
            var rayan2 = datalist2.Where(x => x.Isin == asset.Isin).ToList(); //&& x.BranchId==2097321063

            var rayanDetail1 = dataListDetail1.Where(x => x.Isin == asset.Isin).ToList(); //&& x.BranchId==2097321063
            var rayanDetail2 = dataListDetail2.Where(x => x.Isin == asset.Isin).ToList(); // && x.BranchId==2097321063
            if (rayanDetail1.Count == 0 && rayanDetail2.Count == 0)
                continue;

            var assetTransaction = new AssetTransaction()
            {
                TotalTransaction = rayan1.Sum(d => d.Amount) + rayan2.Sum(d => d.Amount),
                TotalTransactionNum = rayanDetail1.Count + rayanDetail2.Count,
                TotalTradeNum = rayanDetail1.Count + rayanDetail2.Count,
                BuyTradeNum =
                    rayanDetail1.Count(x => x.IsAPurchase == 0) + rayanDetail2.Count(x => x.IsAPurchase == 0),
                SellTradeNum =
                    rayanDetail1.Count(x => x.IsAPurchase == 1) + rayanDetail2.Count(x => x.IsAPurchase == 1),
                TotalFee = rayan1.Sum(x => x.BrokerFee + x.BourseOrganizationFee) +
                           rayan2.Sum(x => x.BrokerFee + x.BourseOrganizationFee),
                BrokerFee = rayan1.Sum(x => x.BrokerFee) +
                            rayan2.Sum(x => x.BrokerFee), //_reportManager.BrokerFee(rayan),
                Benefit = _reportManager.Benefit(rayan1) + _reportManager.Benefit(rayan2),
                NetBenefit = _reportManager.NetBenefit(rayan1) + _reportManager.NetBenefit(rayan2),
                EntraDayBuyQty =
                    rayan1.Where(x => x.IsAPurchase == 0).Sum(x => x.Quantity)
                    + rayan2.Where(x => x.IsAPurchase == 0).Sum(x => x.Quantity),
                EntraDaySellQty = rayan1.Where(x => x.IsAPurchase == 1).Sum(x => x.Quantity)
                                  + rayan2.Where(x => x.IsAPurchase == 1).Sum(x => x.Quantity),
                AvgBuyPrice = (rayan1.Where(x => x.IsAPurchase == 0).Sum(x => x.Amount) +
                               rayan2.Where(x => x.IsAPurchase == 1).Sum(x => x.Amount)) /
                    (rayan1.Where(x => x.IsAPurchase == 0)
                        .Sum(x =>
                            x.Quantity) + rayan2.Where(x => x.IsAPurchase == 0)
                        .Sum(x =>
                            x.Quantity)) == 0
                        ? 1
                        : (rayan1.Where(x => x.IsAPurchase == 0)
                            .Sum(x =>
                                x.Quantity) + rayan2.Where(x => x.IsAPurchase == 0)
                            .Sum(x =>
                                x.Quantity)),


                AvgSellPrice = (rayan1.Where(x => x.IsAPurchase == 1).Sum(x => x.Amount) +
                rayan2.Where(x => x.IsAPurchase == 1).Sum(x => x.Amount)) /
                (rayan1.Where(x => x.IsAPurchase == 1).Sum(x => x.Quantity) +
                rayan2.Where(x => x.IsAPurchase == 1).Sum(x => x.Quantity)) == 0 ? 1 :
                    (rayan1.Where(x => x.IsAPurchase == 1).Sum(x => x.Quantity) +
                     rayan2.Where(x => x.IsAPurchase == 1).Sum(x => x.Quantity)),
                EntraDayCredit = rayan1.Where(x => x.IsAPurchase == 0)
                    .Sum(x => x.Amount - x.BrokerFee -
                              x.BourseOrganizationFee) + rayan2.Where(x => x.IsAPurchase == 0)
                    .Sum(x => x.Amount - x.BrokerFee -
                              x.BourseOrganizationFee),
                EntraDayDebit = (rayan1.Where(x => x.IsAPurchase == 1)
                                    .Sum(x => x.Amount + x.BrokerFee + x.BourseOrganizationFee)) * -1
                                + (rayan2.Where(x => x.IsAPurchase == 1)
                                    .Sum(x => x.Amount + x.BrokerFee + x.BourseOrganizationFee)) * -1,
                DateTime = rayan1.Last().TransactionDate,
                InsertionDateTime = DateTime.UtcNow,
                AssetDifference = 0,
                AssetId = asset.Id,
            };
            transactions.Add(assetTransaction);
        }

        try
        {
            _assetTransactionRepository.AddToAssetTransactionByList(transactions);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        return transactions;
    }

    public async Task<List<AssetTransaction>> GetAggregation()
    {
        var result = new List<AssetTransaction>();


        var response = await _assetTransactionRepository.GetAssetByAssetId();
        var totalSell = response.Sum(x => x.SellTradeNum);
        var totalBuy = response.Sum(x => x.BuyTradeNum);
        var total = response.Sum(x => x.TotalTradeNum);
        var totalTransaction = response.Sum(x => x.TotalTransaction);
        var totalBenefit = response.Sum(x => x.Benefit);
        var totalBrokerBenefit = response.Sum(x => x.BrokerFee);
        var totalNetBenefit = response.Sum(x => x.NetBenefit);

        result.Add(new AssetTransaction
        {
            SellTradeNum = totalSell,
            BuyTradeNum = totalBuy,
            TotalTradeNum = total,
            TotalTransaction = totalTransaction,
            Benefit = totalBenefit,
            BrokerFee = totalBrokerBenefit,
            NetBenefit = totalNetBenefit,
            InsertionDateTime = DateTime.Now
        });


        return result;
    }

    public async Task<List<AssetDailyBalance>> CalculateDailyBalance(string fromDate, string toDate)
    {
        var datalist1 = _marketDataService.GetTransactionByNationalCode(GeneralOptions.GeneralOption1.DsCode, fromDate,
            toDate
            , GeneralOptions.GeneralOption1.NationalCode, GeneralOptions.GeneralOption1.Size).Result;

        var datalist2 = _marketDataService.GetTransactionByNationalCode(GeneralOptions.GeneralOption2.DsCode2, fromDate,
            toDate
            , GeneralOptions.GeneralOption2.NationalCode2, GeneralOptions.GeneralOption2.Size2).Result;

        var rayan1 = datalist1.Where(x => x.BranchId == 2097321063).ToList(); //&& x.BranchId==2097321063
        var rayan2 = datalist2.Where(x => x.BranchId == 2097321063).ToList();
        var concatList = rayan1.Concat(rayan2).ToList();
        List<string> isins = new List<string>();
        var assetDailyBalance = new List<AssetDailyBalance>();
        string insertionTime = concatList[0].TransactionDate;

        foreach (var data in concatList)
        {
            isins.Add(data.Isin);
        }

        // isins = (isins.Distinct<string>().ToList());
        HashSet<string> midel = new HashSet<string>(isins);
        List<string> filteredList = midel.ToList();
        filteredList.Add("irr");

        var dbAssets = new List<AssetDailyBalance>();
        // foreach (var isin in filteredList)
        // {
        //     var dbAsset = _assetDailyBalanceRepository.GetAssetByIsin(isin);
        //     if (dbAsset.Result.ToList().Count != 0)
        //     {
        //         dbAssets.Add(dbAsset.Result.ToList()[0]);
        //     }
        // }
        decimal totalRial = 0;

        dbAssets.AddRange(_assetDailyBalanceRepository.GetLatestData().Result);


        foreach (var asset in dbAssets)
        {
            if (asset.Isin == "irr")
            {
                continue;
            }

            decimal sellQty = 0;
            decimal buyQty = 0;
            decimal commissionSell = 0;
            decimal commissionBuy = 0;
            decimal sellAmount = 0;
            decimal buyAmount = 0;
            ;
            var result = concatList.Where(x => x.Isin == asset.Isin).ToList();

            foreach (var transaction in result)
            {
                if (transaction.IsAPurchase == 0)
                {
                    sellQty += transaction.Quantity;
                    sellAmount += transaction.Amount;
                    commissionSell += (transaction.BrokerFee + transaction.BourseOrganizationFee);
                    //var rial = filteredList.ElementAt(filteredList.IndexOf("irr"));
                    totalRial += transaction.Amount - (transaction.BrokerFee + transaction.BourseOrganizationFee);
                }
                else
                {
                    buyQty += transaction.Quantity;
                    buyAmount += transaction.Amount;
                    commissionBuy += (transaction.BrokerFee + transaction.BourseOrganizationFee);
                    totalRial -= transaction.Amount - transaction.BrokerFee - transaction.BourseOrganizationFee;
                }

                //insertionTime = transaction.TransactionDate;
            }


            var qty = buyQty - sellQty;
            GetCandlestickRequest candlestickRequest = new GetCandlestickRequest()
            {
                Isin = asset.Isin,
                Count = 1000,
                Version = "1",
                TimeFrame = GetCandlestickRequest.TimeFrameType.Day,
                Direction = GetCandlestickRequest.DirectionType.Forward,
                PriceType = GetCandlestickRequest.PricingType.NormalPrice,
                StartDate = Convert.ToDateTime("11/04/2023 12:00:00 AM"), //DateTime.Today,
                StartRangeTime = DateTime.MinValue,
                EndRangeTime = DateTime.MinValue,
            };
            //HttpClient client = new HttpClient();
            try
            {
                var candlestickInfo = await _candlestickDataProvider.GetCandlestickInfo(candlestickRequest);
                if (candlestickInfo != null)
                {
                    var closePrice = candlestickInfo.candleStickDtos[0].C;

                    // if (insertionTime.Length==0)
                    // {
                    //     insertionTime = result[0].TransactionDate;
                    // }

                    assetDailyBalance.Add(new AssetDailyBalance
                        {
                            AssetName = asset.AssetName,
                            Isin = asset.Isin,
                            Quantity = (asset.Quantity + qty),
                            Value = Math.Round(((asset.Quantity + qty) * (closePrice)) *
                                               Convert.ToDecimal(0.998765), 6),
                            DateTime = insertionTime,
                            InsertionDateTime = DateTime.Now.ToUniversalTime(),
                        }
                    );
                }
                else
                {
                    Console.WriteLine($"candle does not exist: {candlestickInfo.candleStickDtos}");
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        var previousRial = dbAssets.SingleOrDefault(x => x.Isin == "irr");
        assetDailyBalance.Add(new AssetDailyBalance
        {
            AssetName = "ریال",
            Isin = "irr",
            Quantity = previousRial.Quantity + totalRial,
            Value = previousRial.Value + totalRial,
            DateTime = insertionTime,
            InsertionDateTime = DateTime.Now.ToUniversalTime(),
        });

        _assetDailyBalanceRepository.Add(assetDailyBalance);
        return assetDailyBalance;
    }
}