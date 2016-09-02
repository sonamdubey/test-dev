using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.UsedBikes;
using Bikewale.Service.Utilities;
using System;
using System.Web.Http;

namespace Bikewale.Service.Controllers.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 Sep 2016
    /// Description :   Used Bike Buyer Controller
    /// </summary>
    public class UsedBikeBuyerController : CompressionApiController
    {
        private readonly IUsedBikeBuyer _objUsedBikeBuyerBL = null;
        public UsedBikeBuyerController(IUsedBikeBuyer objUsedBikeBuyerBL)
        {
            _objUsedBikeBuyerBL = objUsedBikeBuyerBL;
        }

        [HttpPost, Route("api/bikebuyer/requestphotos/")]
        public IHttpActionResult RequestPhotos([FromBody]PhotoRequestDTO request)
        {
            try
            {
                PhotoRequest requestEntity = UsedBikeBuyerMapper.Convert(request);
                return Ok(_objUsedBikeBuyerBL.UploadPhotosRequest(requestEntity));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("RequestPhotos({0})", Newtonsoft.Json.JsonConvert.SerializeObject(request)));
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpPost, Route("api/bikebuyer/showninterest/")]
        public IHttpActionResult ShownInterestInThisBike(string profileId, bool isDealer, [FromBody] DTO.Customer.CustomerBase buyer)
        {
            try
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("ShownInterestInThisBike({0},{1})", profileId, isDealer));
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
