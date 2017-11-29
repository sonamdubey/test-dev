using Bikewale.Notifications;
using BikewaleOpr.DTO.BwPrice;
using BikewaleOpr.Interface.BikePricing;
using System;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 09 Nov 2017
    /// Descripiton : Provide apis for bikewale price.
    /// </summary>
    public class ShowroomPricesController : ApiController
    {
        private readonly IBwPrice _bwPrice;
        public ShowroomPricesController(IBwPrice bwPrice)
        {
            _bwPrice = bwPrice;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 09 Nov 2017
        /// Descripiton : API to save bikewale price.
        /// </summary>
        /// <param name="versionAndPriceList">Bike version id list and price list in format "versionId#c0l#ex-showroom#c0l#insurance#c0l#rto|r0w|"</param>
        /// <param name="citiesList">City id list in format "cityid|r0w|"</param>
        /// <param name="updatedBy">User updated by</param>
        /// <returns></returns>
        [HttpPost, Route("api/price/save/")]
        public IHttpActionResult SaveBikePrices(BWPriceInputDTO bwPrice)
        {
            try
            {
                if (bwPrice != null && !string.IsNullOrEmpty(bwPrice.VersionAndPriceList) && !string.IsNullOrEmpty(bwPrice.CitiesList) && !string.IsNullOrEmpty(bwPrice.ModelIds) && bwPrice.MakeId > 0 && bwPrice.UserId > 0)
                {
                    bool IsSaved = _bwPrice.SaveBikePrices(bwPrice.VersionAndPriceList, bwPrice.CitiesList, bwPrice.MakeId, bwPrice.ModelIds, bwPrice.UserId);
                    return Ok(IsSaved);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("ShowroomPricesController.SaveBikePrices:_{0}_{1}_{2}_{3}_{4}", bwPrice.VersionAndPriceList, bwPrice.CitiesList, bwPrice.MakeId, bwPrice.ModelIds, bwPrice.UserId));
                return InternalServerError();
            }
        }
    }
}
