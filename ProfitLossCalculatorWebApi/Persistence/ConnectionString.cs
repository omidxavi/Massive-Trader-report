namespace ProfitLossCalculatorWebApi.Persistence;

public  class ConnectionString
{
    public ConnectionString(string value)
    {
        Value = value;
    }

    public readonly string Value;
}