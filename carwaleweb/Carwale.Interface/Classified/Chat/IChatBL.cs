using Carwale.Entity.Classified.Chat;

namespace Carwale.Interfaces.Classified.Chat
{
    public interface IChatBL
    {
        bool IsLegitimateLead(string buyerId, string sellerChatToken, out string mobile);
        bool SendChatFallbackSms(ChatSmsPayload chatSmsPayload, string mobile, bool isBuyerToSellerChat);
    }
}
