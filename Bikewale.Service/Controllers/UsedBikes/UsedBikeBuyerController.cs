using Bikewale.DTO.Used;
using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.UsedBikes;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 Sep 2016
    /// Description :   Used Bike Buyer Controller
    /// </summary>
    public class UsedBikeBuyerController : CompressionApiController
    {
        private readonly IUsedBikeBuyer _objUsedBikeBuyerBL = null;
        private readonly IUsedBikes _objUsedBikes = null;
        /// <summary>
        /// Created by  :   Sumit Kate on 03 Sep 2016
        /// Description :   Constructor to initialize the member variables
        /// </summary>
        /// <param name="objUsedBikeBuyerBL"></param>
        public UsedBikeBuyerController(IUsedBikeBuyer objUsedBikeBuyerBL, IUsedBikes objUsedBikes)
        {
            _objUsedBikeBuyerBL = objUsedBikeBuyerBL;
            _objUsedBikes = objUsedBikes;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 03 Sep 2016
        /// Description :   Submits the upload photo request to seller
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("api/bikebuyer/requestphotos/"), ResponseType(typeof(bool))]
        public IHttpActionResult RequestPhotos([FromBody]PhotoRequestDTO request)
        {
            try
            {
                if (request != null)
                {
                    if (!String.IsNullOrEmpty(request.BikeName) && UsedBikeProfileId.IsValidProfileId(request.ProfileId))
                    {
                        PhotoRequest requestEntity = UsedBikeBuyerMapper.Convert(request);
                        return Ok(_objUsedBikeBuyerBL.UploadPhotosRequest(requestEntity));
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("RequestPhotos({0})", Newtonsoft.Json.JsonConvert.SerializeObject(request)));
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 Sep 2016
        /// Description :   Call BL
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="isDealer"></param>
        /// <param name="buyer"></param>
        /// <returns></returns>
        [HttpPost, Route("api/bikebuyer/showninterest/"), ResponseType(typeof(BikeInterestDetailsDTO))]
        public IHttpActionResult ShownInterestInThisBike(string profileId, bool isDealer, [FromBody] DTO.Customer.CustomerBase buyer)
        {
            try
            {
                if (UsedBikeProfileId.IsValidProfileId(profileId))
                {
                    Entities.Customer.CustomerEntityBase buyerEntity = null;
                    if (buyer != null)
                    {
                        buyerEntity = UsedBikeBuyerMapper.Convert(buyer);
                    }
                    BikeInterestDetails interestDetails = _objUsedBikeBuyerBL.ShowInterestInBike(buyerEntity, profileId, isDealer);
                    BikeInterestDetailsDTO dto = UsedBikeBuyerMapper.Convert(interestDetails);
                    return Ok(dto);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("ShownInterestInThisBike({0},{1})", profileId, isDealer));
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Sep 2016
        /// Description :   Used bike purchase inquiry API
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="pageUrl"></param>
        /// <param name="buyer"></param>
        /// <returns></returns>
        [HttpPost, Route("api/usedbike/purchaseinquiry/"), ResponseType(typeof(PurchaseInquiryResultDTO))]
        public IHttpActionResult PurchaseInquiry(string profileId, string pageUrl, [FromBody] DTO.Customer.CustomerBase buyer)
        {
            try
            {
                string platformId = "";

                if (Request.Headers.Contains("platformId"))
                {
                    platformId = Request.Headers.GetValues("platformId").First().ToString();
                    if (!String.IsNullOrEmpty(platformId) && Utility.CommonValidators.IsValidNumber(platformId))
                    {
                        Entities.Customer.CustomerEntityBase buyerEntity = null;
                        if (buyer != null)
                        {
                            buyerEntity = UsedBikeBuyerMapper.Convert(buyer);
                        }
                        PurchaseInquiryResultEntity inquiryresult = _objUsedBikeBuyerBL.SubmitPurchaseInquiry(buyerEntity, profileId, pageUrl, Convert.ToUInt16(platformId));
                        PurchaseInquiryResultDTO dto = UsedBikeBuyerMapper.Convert(inquiryresult);
                        return Ok(dto);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PurchaseInquiry({0},{1})", profileId, pageUrl));
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 06-10-2016
        /// Description : Getting used bike details by profileId
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/used/inquiry/url/{profileId}/{customerId}/"), ResponseType(typeof(InquiryDetailsDTO))]
        public IHttpActionResult GetInquiryDetailsByProfileId(string profileId, string customerId)
        {
            InquiryDetails objInquiryDetailsByProfileId = null;
            InquiryDetailsDTO objInquiryDetailsDTO = null;
            string platformId = string.Empty;
            try
            {
                if (Request.Headers.Contains("platformId")
                    && Request.Headers.GetValues("platformId").FirstOrDefault() != null
                    && UsedBikeProfileId.IsValidProfileId(profileId))
                {
                    platformId = Request.Headers.GetValues("platformId").FirstOrDefault().ToString();

                    objInquiryDetailsByProfileId = _objUsedBikes.GetInquiryDetailsByProfileId(profileId, customerId, platformId);

                    if (objInquiryDetailsByProfileId != null)
                    {
                        objInquiryDetailsDTO = UsedBikeBuyerMapper.Convert(objInquiryDetailsByProfileId);
                        if (objInquiryDetailsDTO != null)
                            return Ok(objInquiryDetailsDTO);
                        else
                            return NotFound();
                    }
                    else
                        return BadRequest();
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception in API function GetInquiryDetailsByProfileId for profileId : {0}, customerId : {1}", profileId, customerId));
               
                return InternalServerError();
            }
        }
    }
}

