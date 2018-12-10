using System;
using System.Web.Http;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.Stock;
using Carwale.Service.Filters;
using Carwale.Entity.Stock.Certification;
using Carwale.BL.Stock;
using Carwale.Utility;
using Carwale.Entity.Enum;
using Carwale.DTOs.Stock.Certification;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Carwale.Service.Controllers.Stock
{
    public class StockCertificationsController : ApiController
    {
        private readonly IStockCertificationRepository _stockCertificationRepo;
        private readonly IStockCertificationCacheRepository _stockCertificationCacheRepo;
        private readonly IStockCertificationBL _stockCertificationBL;
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        { 
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };


        public StockCertificationsController(IStockCertificationRepository stockCertificationrepo, IStockCertificationCacheRepository stockCertificationCacheRepo, IStockCertificationBL stockCertificationBL)
        {
            _stockCertificationRepo = stockCertificationrepo;
            _stockCertificationCacheRepo = stockCertificationCacheRepo;
            _stockCertificationBL = stockCertificationBL;
        }

        [Route("api/stocks/{profileId}/certification")]
        [LogApi, ApiAuthorization]
        public IHttpActionResult Post(string profileId, [FromBody]StockCertification stockCertification)
        {
            try
            {
                bool isDealer;
                int inquiryId = StockBL.GetInquiryId(profileId, out isDealer);

                if (inquiryId <= 0)
                {
                    return BadRequest("ProfileId is not Valid.");
                }
                if (stockCertification == null)
                {
                    ModelState.AddModelError("certification", "'Certification' must be provided.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                stockCertification.InquiryId = inquiryId;
                stockCertification.IsDealer = isDealer;
                stockCertification.ExteriorOriginalImgPath = _stockCertificationBL.GetExteriorImagePath(profileId, stockCertification.CarExteriorImageUrl);
                int certificationId = _stockCertificationRepo.AddStockCertification(stockCertification);
                if (certificationId > 0)
                {
                    _stockCertificationBL.UploadCarExteriorImage(certificationId, stockCertification.CarExteriorImageUrl, stockCertification.ExteriorOriginalImgPath);
                    _stockCertificationCacheRepo.RefreshCarCertification(inquiryId, isDealer);
                    return Ok(certificationId);
                }
                else if (certificationId == -1)
                {
                    ModelState.AddModelError("profileid", "profileId does not exist on carwale.");
                    return BadRequest(ModelState);
                }
                else if (certificationId == -2)
                {
                    ModelState.AddModelError("certification", "Certification already exists for this stock.");
                    return BadRequest(ModelState);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/stocks/{profileId}/certification")]
        [LogApi, ApiAuthorization]
        public IHttpActionResult Put(string profileId, [FromBody]StockCertification stockCertification)
        {
            try
            {
                bool isDealer;
                int inquiryId = StockBL.GetInquiryId(profileId, out isDealer);

                if (inquiryId <= 0)
                {
                    return BadRequest("ProfileId is not Valid.");
                }
                if (stockCertification == null)
                {
                    ModelState.AddModelError("certification", "'Certification' must be provided.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                stockCertification.InquiryId = inquiryId;
                stockCertification.IsDealer = isDealer;
                stockCertification.ExteriorOriginalImgPath = _stockCertificationBL.GetExteriorImagePath(profileId, stockCertification.CarExteriorImageUrl);
                int certificationId = _stockCertificationRepo.UpdateStockCertification(stockCertification);

                if(certificationId > 0)
                {
                    _stockCertificationBL.UploadCarExteriorImage(certificationId, stockCertification.CarExteriorImageUrl, stockCertification.ExteriorOriginalImgPath);
                    _stockCertificationCacheRepo.RefreshCarCertification(inquiryId, isDealer);
                    return Ok();
                }
                else if (certificationId == -1)
                {
                    ModelState.AddModelError("profileid", "profileId does not exist on carwale.");
                    return BadRequest(ModelState);
                }
                else if (certificationId == -2)
                {
                    ModelState.AddModelError("certification", "Certification does not exist for this stock.");
                    return BadRequest(ModelState);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/stocks/{profileId}/certification/status")]
        [LogApi, ApiAuthorization]
        public IHttpActionResult Put(string profileId, [FromBody]CertificationStatus status)
        {
            try
            {
                bool isDealer;
                int inquiryId = StockBL.GetInquiryId(profileId, out isDealer);

                if (inquiryId <= 0)
                {
                    return BadRequest("ProfileId is not Valid.");
                }
                if (status == null || status.IsActive == null)
                {
                    ModelState.AddModelError("isActive", "'IsActive' must be provided.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int certificationId = _stockCertificationRepo.UpdateStockCertificationStatus(inquiryId, isDealer, (bool)status.IsActive);
                if (certificationId > 0)
                {
                    _stockCertificationCacheRepo.RefreshCarCertification(inquiryId, isDealer);
                    return Ok();
                }
                else if (certificationId == -1)
                {
                    ModelState.AddModelError("stockId", "stockId does not exist on carwale.");
                    return BadRequest(ModelState);
                }
                else if (certificationId == -2)
                {
                    ModelState.AddModelError("certification", "Certification does not exist for this stock.");
                    return BadRequest(ModelState);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/stocks/{profileId}/certification")]
        [LogApi]
        public IHttpActionResult Get(string profileId)
        {
            try
            {
                bool isDealer;
                int inquiryId = StockBL.GetInquiryId(profileId, out isDealer);

                if (inquiryId <= 0)
                {
                    return BadRequest("ProfileId is not Valid.");
                }

                int requestSource = Request.Headers.GetValueFromHttpHeader<int>("SourceId");
                StockCertification stockCertification = _stockCertificationCacheRepo.GetCarCertification(inquiryId, isDealer);
                if (stockCertification != null)
                {
                    switch ((Platform)requestSource)
                    {
                        case Platform.CarwaleAndroid:
                        case Platform.CarwaleiOS:
                            {
                                StockCertificationApp certificationDTO = Mapper.Map<StockCertification, StockCertificationApp>(stockCertification);
                                return Json(certificationDTO, _serializerSettings);
                            }
                        default:
                            return Ok(stockCertification);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
    }
}
