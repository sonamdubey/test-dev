using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Web.Http;

namespace Carwale.Service.Controllers.Classified
{
    public class SellLeadController : ApiController
    {
        private readonly ISellCarBL _sellCarBL;
        private readonly IConsumerToBusinessBL _consumerToBusinessBL;

        public SellLeadController(
            ISellCarBL sellCarBL,
            IConsumerToBusinessBL consumerToBusinessBL
            )
        {
            _sellCarBL = sellCarBL;
            _consumerToBusinessBL = consumerToBusinessBL;
        }

        [Route("api/selllead/{inquiryId}/hotlead")]
        [HttpPut]
        public IHttpActionResult PushHotLead(int inquiryId, [FromBody]int hotLeadPrice)
        {
            try
            {
                if (inquiryId <= 0)
                    ModelState.AddModelError("InquiryId", "InquiryId is not valid");
                if (hotLeadPrice <= 0)
                    ModelState.AddModelError("hotLead", "hotLeadPrice is not valid");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                _consumerToBusinessBL.PushToIndividualStockQueue(inquiryId, hotLeadPrice);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/selllead/")]
        [HttpPost]
        public IHttpActionResult PushSellLead([FromBody]int hotLeadPrice)
        {
            try
            {
                int inquiryId = Convert.ToInt32(CookieManager.GetEncryptedCookie("SellInquiry"));
                if (inquiryId <= 0)
                {
                    return BadRequest();
                }

                _consumerToBusinessBL.PushToIndividualStockQueue(-1, C2BActionType.AddCarDetails, inquiryId, hotLeadPrice);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
    }
}
