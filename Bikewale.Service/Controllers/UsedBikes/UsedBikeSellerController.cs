using Bikewale.DTO.Used;
using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.UsedBikes;
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
namespace Bikewale.Service.Controllers.UsedBikes
{
    public class UsedBikeSellerController : CompressionApiController
    {
        private readonly ISellBikes _usedBikesRepo = null;

        public UsedBikeSellerController(ISellBikes UsedBikesRepo)
        {
            _usedBikesRepo = UsedBikesRepo;
        }

        [HttpPost, Route("api/used/sell/listing/"), ResponseType(typeof(SellBikeInquiryResultDTO))]
        public IHttpActionResult Post([FromBody]SellBikeAdDTO ad)
        {
            SellBikeAd adEntity = UsedBikeBuyerMapper.Convert(ad);

            if (ModelState.IsValid)
            {
                SellBikeInquiryResultEntity inquiry = _usedBikesRepo.SaveSellBikeAd(adEntity);
                SellBikeInquiryResultDTO inquiryDTO = UsedBikeBuyerMapper.Convert(inquiry);
                return Ok(inquiryDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet, Route("api/used/sell/listing/"), ResponseType(typeof(SellBikeAdDTO))]
        public IHttpActionResult Get(int inquiryId, ulong customerId)
        {
            SellBikeAd adEntity = null;
            SellBikeAdDTO adDTO = null;
            if (ModelState.IsValid)
            {
                adEntity = _usedBikesRepo.GetById(inquiryId, customerId);
                adDTO = UsedBikeBuyerMapper.Convert(adEntity);
                return Ok(adDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost, Route("api/used/sell/listing/{profileId}/")]
        public IHttpActionResult Post([FromBody]SellBikeAdDTO ad, string profileId)
        {
            SellBikeAd adEntity = UsedBikeBuyerMapper.Convert(ad);
            if (ModelState.IsValid)
            {
                _usedBikesRepo.SaveSellBikeAd(adEntity);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost, Route("api/used/sell/listing/otherinfo/")]
        public IHttpActionResult Post(SellBikeAdOtherInformationDTO otherInfo, int inquiryId, ulong customerId)
        {
            SellBikeAdOtherInformation sellOtherEntity = null;
            if (ModelState.IsValid)
            {
                sellOtherEntity = UsedBikeBuyerMapper.Convert(otherInfo);
                SellBikeInquiryResultEntity inquiry = _usedBikesRepo.UpdateOtherInformation(sellOtherEntity, inquiryId, customerId);
                SellBikeInquiryResultDTO inquiryDTO = UsedBikeBuyerMapper.Convert(inquiry);
                return Ok(inquiryDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost, ResponseType(typeof(bool)), Route("api/used/sell/listing/verifymobile/")]
        public IHttpActionResult Post([FromBody]SellerDTO seller)
        {
            SellerEntity sellerEntity = null;
            bool result;
            if (ModelState.IsValid)
            {
                sellerEntity = UsedBikeBuyerMapper.Convert(seller);
                result = _usedBikesRepo.VerifyMobile(sellerEntity);
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Oct 2016
        /// Description :   Validates the Sell Bike Image Upload Request
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="isMain"></param>
        /// <returns></returns>
        [HttpPost, Route("api/used/{profileId}/image/validate/")]
        public IHttpActionResult Post(string profileId, string extension, bool? isMain)
        {
            try
            {
                string strCustomerId = Request.Headers.Contains("customerId") ? Request.Headers.GetValues("customerId").FirstOrDefault() : "";
                UInt64 customerId = 0;
                if (!string.IsNullOrEmpty(strCustomerId)
                    && Utility.UsedBikeProfileId.IsValidProfileId(profileId)
                    && UInt64.TryParse(strCustomerId, out customerId)
                    && customerId > 0
                    && !String.IsNullOrEmpty(extension))
                {

                    return Ok(true);

                }
                else
                {
                    return BadRequest("Invalid image");
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("api/used/{0}/image/validate/?isMain={1},FileUploadCount={2},contentType={3}", profileId, isMain, HttpContext.Current.Request.Files.Count, HttpContext.Current.Request.ContentType));

                return InternalServerError();
            }

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Oct 2016
        /// Description :   Sell Bike image upload API
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="isMain"></param>
        /// <returns></returns>
        [HttpPost, Route("api/used/{profileId}/image/upload/")]
        public IHttpActionResult Upload(string profileId, string extension, bool? isMain)
        {
            try
            {
                string strCustomerId = Request.Headers.Contains("customerId") ? Request.Headers.GetValues("customerId").FirstOrDefault() : "";
                UInt64 customerId = 0;
                if (!string.IsNullOrEmpty(strCustomerId)
                    && Utility.UsedBikeProfileId.IsValidProfileId(profileId)
                    && UInt64.TryParse(strCustomerId, out customerId)
                    && customerId > 0
                    && !String.IsNullOrEmpty(extension))
                {
                    SellBikeImageUploadResultEntity uploadResult = _usedBikesRepo.UploadBikeImage(
                    isMain.HasValue ? isMain.Value : false,
                    customerId,
                    profileId,
                    extension,
                    ""
                    );
                    SellBikeImageUploadResultDTO result = UsedBikeBuyerMapper.Convert(uploadResult);

                    if (result != null && result.Status == ImageUploadResultStatusDTO.Success)
                    {
                        return Ok(result);
                    }
                    else if (result == null)
                    {
                        return InternalServerError();
                    }
                    else
                    {
                        return BadRequest("Server refused the request");
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("api/used/{0}/image/upload/?isMain={1},FileUploadCount={2},contentType={3}", profileId, isMain, HttpContext.Current.Request.Files.Count, HttpContext.Current.Request.ContentType));

                return InternalServerError();
            }

        }
    }
}
