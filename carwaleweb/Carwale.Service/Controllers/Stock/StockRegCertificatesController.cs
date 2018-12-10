using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Interfaces.Stock;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Configuration;
using System.Web.Http;

namespace Carwale.Service.Controllers.Stock
{
    public class StockRegCertificatesController : ApiController
    {
        private readonly IStockRegCertificatesRepository _regCertRepo;
        private readonly ISellCarBL _sellCarBL;
        private readonly IConsumerToBusinessBL _consumerToBusinessBL;

        public StockRegCertificatesController(
            IStockRegCertificatesRepository regCertRepo, 
            ISellCarBL sellCarBL,
            IConsumerToBusinessBL consumerToBusinessBL
            )
        {
            _regCertRepo = regCertRepo;
            _sellCarBL = sellCarBL;
            _consumerToBusinessBL = consumerToBusinessBL;
        }

        [HttpPost]
        [Route("api/stockregistrationcertificates")]
        public IHttpActionResult Post([FromBody]string originalImgPath)
        {
            try
            {
                int inquiryId = Convert.ToInt32(CookieManager.GetEncryptedCookie("SellInquiry"));
                if (inquiryId <= 0 || string.IsNullOrWhiteSpace(originalImgPath))
                {
                    return BadRequest();
                }

                int rcId = _regCertRepo.AddStockRegCertificate(inquiryId, ConfigurationManager.AppSettings["CDNHostURL"], $"/{originalImgPath}");
                if (rcId > 0)
                {
                    _consumerToBusinessBL.PushToIndividualStockQueue(-1, C2BActionType.AddRC, inquiryId);
                    return Ok(rcId);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("api/stockregistrationcertificates")]
        public IHttpActionResult Delete([FromBody]int rcId)
        {
            try
            {
                int inquiryId = Convert.ToInt32(CookieManager.GetEncryptedCookie("SellInquiry"));
                if (inquiryId <= 0 || rcId <= 0)
                {
                    return BadRequest();
                }

                _regCertRepo.DeleteStockRegCertificate(inquiryId, rcId);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError(ex);
            }
        }
    }
}
