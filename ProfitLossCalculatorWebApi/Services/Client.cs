using Newtonsoft.Json;
using ProfitLossCalculatorWebApi.Dto;
using ProfitLossCalculatorWebApi.Model;

namespace ProfitLossCalculatorWebApi.Services;

public class Client
{

    private readonly HttpClient _client;

    public Client(HttpClient httpClient)
    {
        _client = httpClient;
    }
    public List<GetRayanDataResponseDto> GetDataAsync(string url)
    {
        try
        {
            var transactions = new List<GetRayanDataResponseDto>();
            var serializer = _client.GetStringAsync(url).Result;
            var deserializer = JsonConvert.DeserializeObject<Transactions>(serializer);

            foreach (var transaction in deserializer.result)
            {
                transactions.Add(transaction);
            }

            return transactions;


        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<GetRayanDataDetailResponseDto> GetDataDetailAsync(string url)
    {
        try
        {
            var transactions = new List<GetRayanDataDetailResponseDto>();
            var serializer = _client.GetStringAsync(url).Result;
            var deserializer = JsonConvert.DeserializeObject<TransactionDetail>(serializer);

            foreach (var transaction in deserializer.result)
            {
                transactions.Add(transaction);
            }

            return transactions;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}