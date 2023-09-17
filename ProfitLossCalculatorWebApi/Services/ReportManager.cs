using System.IO.Enumeration;
using Algorithm.Mdp.DataProvider.Instruments;
using Algorithm.Mdp.DataProvider.TechnicalInfo;
using Algorithm.Mdp.Models.Dto;
using ProfitLossCalculatorWebApi.Model;
using ProfitLossCalculatorWebApi.Persistence.Models;
using ProfitLossCalculatorWebApi.Persistence.Repositories;
using ProfitLossCalculatorWebApi.Persistence.Repositories.AssetTransaction;

namespace ProfitLossCalculatorWebApi.Services;

public class ReportManager
{
    private readonly IAssetTransactionRepository _assetTransactionRepository;
    private readonly IAssetRepository _assetRepository;

    public ReportManager(IAssetTransactionRepository assetTransactionRepository,IAssetRepository assetRepository)
    {
        _assetTransactionRepository = assetTransactionRepository;
        _assetRepository = assetRepository;
    }

    public decimal TotalTransaction(List<GetRayanDataResponse> rayan)
    {
        decimal total = 0;
        foreach (var data in rayan)
        {
            total += data.Amount;
        }

       var x = rayan.Sum(d => d.Amount);
        return total;
    }


    public int TotalTransactionNumber(List<GetRayanDataDetailResponse> rayan)
    {
        var total = rayan.Count;
        return total;
    }

    public int TotalTradeNumber(List<GetRayanDataDetailResponse> rayan)
    {
        var total = rayan.Count;
        return total;
    }

    public int BuyTradeNumber(List<GetRayanDataDetailResponse> rayan)
    {
        var total = 0;
        foreach (var data in rayan)
        {
            if (data.IsAPurchase == 1)
            {
                total += rayan.Count;
            }
        }

        return total;
    }

    public int SellTradeNumber(List<GetRayanDataDetailResponse> rayan)
    {
        var total = 0;
        foreach (var data in rayan)
        {
            if (data.IsAPurchase == 0)
            {
                total += rayan.Count;
            }
        }

        return total;
    }

    public decimal TotalFee(List<GetRayanDataResponse> rayan)
    {
        var total = 0;
        foreach (var data in rayan)
        {
            total += data.BrokerFee + data.BourseOrganizationFee;
        }

        return total;
    }

    public decimal BrokerFee(List<GetRayanDataResponse> rayan)
    {
        var total = 0;
        foreach (var data in rayan)
        {
            total += data.BrokerFee;
        }

        return total;
    }

    public decimal Benefit(List<GetRayanDataResponse> rayan)
    {

        
        decimal total = EntraDayCredit(rayan) + EntraDayDebit(rayan);
        // foreach (var data in rayan)
        // {
        //     if (data.IsAPurchase==0)
        //     {
        //        data.Amount= (-1)*data.Amount;
        //     }
        //
        //     total += data.Amount;
        //
        // }


        return total;
    }


    public decimal NetBenefit(List<GetRayanDataResponse> rayan)
    {
        //_assetTransactionRepository.GetAssetTransaction();
        return Benefit(rayan) + BrokerFee(rayan);
    }


    public decimal AvgBuyPrice(List<GetRayanDataResponse> rayan)
    {
        decimal avg = 0;
        if (rayan[0].IsAPurchase == 1)
        {
            avg = rayan[0].Amount / rayan[0].Quantity;
        }


        return avg;
    }

    public decimal AvgSellPrice(List<GetRayanDataResponse> rayan)
    {
        decimal avg = 0;
        if (rayan[0].IsAPurchase == 0)
        {
            avg = rayan[0].Amount / rayan[0].Quantity;
        }


        return avg;
    }

    public decimal EntraDayCredit(List<GetRayanDataResponse> rayan)
    {
        decimal credit = 0;
        foreach (var data in rayan)
        {
            if (data.IsAPurchase == 0)
            {
                credit += (data.Amount - data.BrokerFee - data.BourseOrganizationFee);
            }
        }

        return credit;
    }


    public decimal EntraDayDebit(List<GetRayanDataResponse> rayan)
    {
        decimal debit = 0;
        foreach (var data in rayan)
        {
            if (data.IsAPurchase == 1)
            {
                debit += (data.Amount + data.BrokerFee + data.BourseOrganizationFee);
            }

            
        }
        var result = debit * (-1);
        return (result);
    }

    public decimal AssetDifference(List<GetRayanDataResponse> rayan)
    {
        var diff = 0;
        var buy = 0;
        var sell = 0;

        foreach (var data in rayan)
        {
            if (data.IsAPurchase == 0)
            {
                buy += Convert.ToInt32(data.Quantity);
            }

            if (data.IsAPurchase == 1)
            {
                sell += Convert.ToInt32(data.Quantity);
            }

       
        }

        return Math.Abs(sell - buy);
    }

    public decimal EntraDayBuyQty(List<GetRayanDataResponse> rayan)
    {
        decimal qty = 0;
        foreach (var data in rayan)
        {
            if (data.IsAPurchase == 1)
            {
                qty += data.Quantity;
            }
        }


        return qty;
    }

    public decimal EntraDaySellQty(List<GetRayanDataResponse> rayan)
    {
        decimal qty = 0;
        foreach (var data in rayan)
        {
            if (data.IsAPurchase == 0)
            {
                qty += data.Quantity;
            }
        }


        return qty;
    }
}