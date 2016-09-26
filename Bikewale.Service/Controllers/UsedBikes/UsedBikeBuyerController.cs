using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
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
        /// <summary>
        /// Created by  :   Sumit Kate on 03 Sep 2016
        /// Description :   Constructor to initialize the member variables
        /// </summary>
        /// <param name="objUsedBikeBuyerBL"></param>
        public UsedBikeBuyerController(IUsedBikeBuyer objUsedBikeBuyerBL)
        {
            _objUsedBikeBuyerBL = objUsedBikeBuyerBL;
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("RequestPhotos({0})", Newtonsoft.Json.JsonConvert.SerializeObject(request)));
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("ShownInterestInThisBike({0},{1})", profileId, isDealer));
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("PurchaseInquiry({0},{1})", profileId, pageUrl));
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
