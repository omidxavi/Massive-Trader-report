using System.Net;
using Algorithm.Mdp;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using ProfitLossCalculatorWebApi.Handler;
using ProfitLossCalculatorWebApi.Model;
using ProfitLossCalculatorWebApi.Persistence;
using ProfitLossCalculatorWebApi.Persistence.Repositories;
using ProfitLossCalculatorWebApi.Persistence.Repositories.Asset;
using ProfitLossCalculatorWebApi.Persistence.Repositories.AssetBalance;
using ProfitLossCalculatorWebApi.Persistence.Repositories.AssetDailyBalance;
using ProfitLossCalculatorWebApi.Persistence.Repositories.AssetTransaction;
using ProfitLossCalculatorWebApi.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                true)
            .Build();

        

        try
        {
            
            Console.WriteLine("Starting web host");
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, configuration.GetValue<int>("HostPort"));
            });

            ConfigServices(builder.Services, configuration);


            var app = builder.Build();

            ConfigApp(app);
            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Host terminated unexpectedly");
        }

    }
    
    private static void ConfigServices(IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddSingleton(a => new Setting()
        {
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        });
        GeneralOptions.GeneralOption1.Init(configuration["DsCode"],configuration["FromDate"],configuration["ToDate"],configuration["NationalCode"],configuration["Size"]);
        GeneralOptions.GeneralOption2.Init(configuration["DsCode2"],configuration["FromDate2"],configuration["ToDate2"],configuration["NationalCode2"],configuration["Size2"]);


        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpClient();
        ConfigSwagger(services);
        
        //DI
        
        var PostgressConnectionString = configuration.GetConnectionString("postgresDb");
        services.AddSingleton((_) => new PostgresConnectionString(PostgressConnectionString));

        services.AddDbContext<BourseAccountingDbContext>();

        ConfigAppServices(services);


        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IAssetBalanceRepository, AssetBalanceRepository>();
        services.AddScoped<IAssetTransactionRepository, AssetTransactionRepository>();
        services.AddScoped<IAssetDailyBalanceRepository, AssetDailyBalanceRepository>();
        services.AddScoped<ReportManager>();
        services.AddScoped<QueryHandler>();

        ConfigSwagger(services);

        MdpConfigure.Config(services, new MdpConfigure.MdpConfig(
            marketDataProviderAddress: "http://fluentmdpservice.sntinfra.local",
            candlestickUrlAddress: "http://tsecandlestick.sntinfra.local",
            stockWatchUrlAddress: "http://mdp-stock-watch.prodkube.farabixo.tech",
            deductionServiceUrl:
            "http://deduction.sntinfra.local/BrokerBackOffice/Deduction/BrokerBackOfficeDeductionService.svc"
        ));
    }
    
    private static void ConfigSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var filePath = Path.Combine(System.AppContext.BaseDirectory, "ProfitLossCalculatorWebApi.xml");
            options.IncludeXmlComments(filePath);
          
           
        });
    }
    
    
    private static void ConfigAppServices(IServiceCollection services)
    {
        services.AddScoped<IMarketDataService, MarketDataService>();
    }
    
    private static void ConfigApp(WebApplication app)
    {
        app.UseExceptionHandler($"/error");

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
    



    private static void ConfigHost(IServiceCollection host)
    {

        ConfigMetrics(host);
    }
    
    private static void ConfigMetrics(IServiceCollection services)
    {
        services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
    }
}