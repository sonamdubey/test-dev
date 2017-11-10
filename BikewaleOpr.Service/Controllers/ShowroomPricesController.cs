using Bikewale.Notifications;
using BikewaleOpr.Interface.Dealers;
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
        private readonly IShowroomPricesRepository _showroomPricesRepository;
        public ShowroomPricesController(IShowroomPricesRepository showroomPricesRepository)
        {
            _showroomPricesRepository = showroomPricesRepository;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 09 Nov 2017
        /// Descripiton : API to save bikewale price.
        /// </summary>
        /// <param name="versionAndPriceList"></param>
        /// <param name="citiesList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/price/save/")]
        public IHttpActionResult SaveBikePrices(string versionAndPriceList, string citiesList, int userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(versionAndPriceList) && !String.IsNullOrEmpty(citiesList) && userId > 0)
                {
                    bool IsSaved = _showroomPricesRepository.SaveBikePrices(versionAndPriceList, citiesList, userId);
                    return Ok(IsSaved);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ShowroomPricesController.SaveBikePrices:_{0}_{1}_{2}", versionAndPriceList, citiesList, userId));
                return InternalServerError();
            }
        }
    }
}
