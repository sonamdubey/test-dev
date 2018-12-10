using Carwale.Notifications.Logs;
using Carwale.BL.Notifications;
using Carwale.DAL.Notifications;
using Carwale.Entity.Classified.Chat;
using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.Chat;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Stock;
using System.Configuration;
using System.Linq;
using Carwale.BL.Stock;
using AEPLCore.Utils.UrlShortener;

namespace Carwale.BL.Classified.Chat
{
    public class ChatBL : IChatBL
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IUsedCarBuyerCacheRepository _usedCarBuyerCacheRepository;
        private readonly IDealerCache _dealerCache;
        private readonly IChatSmsRepository _chatSmsRepository;
        private readonly IStockBL _stockBL;
        private readonly IChatNotifications _chatNotifications;
        private readonly string _cteChatUrl = ConfigurationManager.AppSettings["CarTradeChatUrl"];

        public ChatBL(ILeadRepository leadRepository, IUsedCarBuyerCacheRepository usedCarBuyerCacheRepository, IDealerCache dealerCache, IChatSmsRepository chatSmsRepository, IStockBL stockBL, IChatNotifications chatNotifications)
        {
            _leadRepository = leadRepository;
            _usedCarBuyerCacheRepository = usedCarBuyerCacheRepository;
            _dealerCache = dealerCache;
            _chatSmsRepository = chatSmsRepository;
            _stockBL = stockBL;
            _chatNotifications = chatNotifications;
        }
        public bool IsLegitimateLead(string buyerId, string sellerChatToken, out string mobile)
        {
            bool isLegitimateLead = true;
            BuyerInfo buyerInfo = _usedCarBuyerCacheRepository.GetBuyerInfo(buyerId);
            mobile = buyerInfo?.Mobile;
            if (string.IsNullOrEmpty(mobile))
            {
                isLegitimateLead = false;
            }
            if (!_leadRepository.IsLeadGivenToDealer(mobile, sellerChatToken))
            {
                isLegitimateLead = false;
            }
            return isLegitimateLead;
        }

        public bool SendChatFallbackSms(ChatSmsPayload chatSmsPayload, string mobile, bool isBuyerToSellerChat)
        {
            string shortUrl;
            bool isSmsSent = false;
            string dealerChatToken = isBuyerToSellerChat ? chatSmsPayload.To : chatSmsPayload.From;
            var dealerChatInfo = _dealerCache.GetDealerMobileFromChatToken(dealerChatToken);

            if (isBuyerToSellerChat)
            {
                mobile = dealerChatInfo.MobileNo;
                shortUrl = _cteChatUrl;
            }
            else
            {
                shortUrl = GetDealerLastLeadDetailsPageShortUrl(mobile, dealerChatInfo.Id);
                if (string.IsNullOrWhiteSpace(shortUrl))
                {
                    return isSmsSent;
                }
            }

            isSmsSent = _chatNotifications.SendSmsToChatUser(mobile, shortUrl, isBuyerToSellerChat);
            if (isSmsSent)
            {
                _chatSmsRepository.InsertChatSmsDetails(chatSmsPayload, isBuyerToSellerChat);
            }

            return isSmsSent;
        }

        private string GetDealerLastLeadDetailsPageShortUrl(string mobile, int dealerid)
        {
            string shortUrl = string.Empty;
            string profileId = _leadRepository.GetDealerStockLeads(mobile, 1, false, dealerid).FirstOrDefault().ProfileId;

            var stock = _stockBL.GetStock(profileId);
            if (stock == null || stock.BasicCarInfo == null || stock.IsSold)
            {
                Logger.LogError("Invalid stock id.");
            }
            else
            {
                string longUrl = StockBL.GetDetailsPageUrl(stock.BasicCarInfo.CityName, stock.BasicCarInfo.MakeName, stock.BasicCarInfo.MaskingName, stock.BasicCarInfo.ProfileId, true);
                shortUrl = UrlShortenerService.GetShortUrl($"{longUrl}?ischatsms=true"); //appending ischatsms to identify landing from chat sms on vdp page to directly open chat window.
                if (string.IsNullOrWhiteSpace(shortUrl))
                {
                    Logger.LogError("Url shortener service is unavailable.");
                }
            }
            return shortUrl;
        }
    }
}
