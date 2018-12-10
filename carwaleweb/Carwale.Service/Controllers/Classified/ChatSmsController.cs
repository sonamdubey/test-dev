using Carwale.Entity.Classified.Chat;
using Carwale.Interfaces.Classified.Chat;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using Carwale.Service.Filters.AuthorizationFilters;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Carwale.Service.Controllers.Classified
{
    [BasicAuthentication("CaRwAlEmUmBaI", "Authentication")]
    public class ChatSmsController : ApiController
    {
        private readonly IChatSmsRepository _chatSmsRepository;
        private readonly IChatBL _chatBL;

        public ChatSmsController(IChatSmsRepository chatSmsRepository, IChatBL chatBL)
        {
            _chatSmsRepository = chatSmsRepository;
            _chatBL = chatBL;
        }

        [HandleException, FilterIP(AllowedIPs = "AllowedUsedChatSmsFallbackIPs"), ValidateModel("chatSmsPayload"), LogApi]
        public IHttpActionResult Post(List<ChatSmsPayload> chatSmsPayload)
        {
            if(!ModelState.IsValid || chatSmsPayload.Count < 1)
            {
                return BadRequest();
            }
            string sellerChatToken = string.Empty;
            string buyerId = string.Empty;
            bool isBuyerToSellerChat = false;
            string mobile;
            var payloadToProcess = chatSmsPayload[0];

            if (payloadToProcess.From.StartsWith("CW_"))      //if this condition true means buyer has send msg and was not read by seller
            {
                buyerId = payloadToProcess.From;
                sellerChatToken = payloadToProcess.To;
                isBuyerToSellerChat = true;
            }
            else if (payloadToProcess.To.StartsWith("CW_"))   //if this condition true means seller has send msg and was not read by buyer
            {
                buyerId = payloadToProcess.To;
                sellerChatToken = payloadToProcess.From;
            }
            else
            {
                return Ok();    //it is carTrade buyer
            }

            if (!_chatBL.IsLegitimateLead(buyerId, sellerChatToken, out mobile))
            {
                return BadRequest("Invalid user details.");
            }

            if (_chatSmsRepository.shouldMessageBeSent(payloadToProcess.From, payloadToProcess.To))
            {
                bool isSmsSent = _chatBL.SendChatFallbackSms(payloadToProcess, mobile, isBuyerToSellerChat);
                if (!isSmsSent)
                {
                    return Content(HttpStatusCode.ServiceUnavailable, "Something went wrong.");
                }
            }
            return Ok();
        }
    }
}
