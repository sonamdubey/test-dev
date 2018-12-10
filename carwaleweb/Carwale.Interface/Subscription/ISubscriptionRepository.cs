
namespace Carwale.Interfaces.Subscription
{
    public interface ISubscriptionRepository
    {
        bool Subscribe(string email, int subscriptionCategory, int subscriptionType);
    }
}
