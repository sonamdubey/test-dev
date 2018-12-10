namespace Carwale.Interfaces.Classified.Chat
{
    public interface IChatNotifications
    {
        bool SendSmsToChatUser(string mobile, string shortUrl, bool isBuyerToSellerChat);
    }
}
