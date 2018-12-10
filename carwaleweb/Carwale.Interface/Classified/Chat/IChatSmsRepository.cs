using Carwale.Entity.Classified.Chat;

namespace Carwale.Interfaces.Classified.Chat
{
    public interface IChatSmsRepository
    {
        bool shouldMessageBeSent(string from, string to);
        void InsertChatSmsDetails(ChatSmsPayload chatSmsPayload, bool isBuyerToSellerChat);
    }
}
