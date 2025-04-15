using Vogen;

namespace StoreSample.ValueObjects;

[ValueObject<decimal>(conversions: Conversions.SystemTextJson | Conversions.EfCoreValueConverter |
                                   Conversions.TypeConverter)]
public readonly partial struct TokenValue
{
    public static TokenValue operator +(TokenValue a, TokenValue b)
    {
        return From(a.Value + b.Value);
    }

    public static TokenValue operator -(TokenValue a, TokenValue b)
    {
        return From(a.Value - b.Value);
    }

    public static bool operator >(TokenValue a, TokenValue b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(TokenValue a, TokenValue b)
    {
        return a.Value < b.Value;
    }

    public static bool operator >=(TokenValue a, TokenValue b)
    {
        return a.Value >= b.Value;
    }

    public static bool operator <=(TokenValue a, TokenValue b)
    {
        return a.Value <= b.Value;
    }

    public static TokenValue operator *(TokenValue a, int b)
    {
        return From(a.Value * b);
    }
    
}