using StoreSample.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace StoreSample.Assets;

public class Asset : AggregateRoot<UserId>
{
    public Asset(UserId id, ValueObjects.TokenValue balance) : base(id)
    {
        Balance = balance;
    }

    public TokenValue Balance { get; private set; }

    public bool CheckBalance(ValueObjects.TokenValue tokenValue)
    {
        return Balance >= tokenValue;
    }


    public void IncBalance()
    {
        Balance += TokenValue.From(1);
    }

    public void Pay(ValueObjects.TokenValue tokenValue)
    {
        if (Balance < tokenValue)

        {
            throw new UserFriendlyException("Insufficient balance");
        }

        Balance -= tokenValue;
    }
}