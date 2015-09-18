using Bikewale.DAL.Compare;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Compare
{
    /// <summary>
    /// Bike Compare List Controller
    /// Author  :   Sumit Kate
    /// Created On : 27 Aug 2015
    /// </summary>
    public class BikeCompareListController : ApiController
    {
        private readonly IBikeCompare _bikeCompare = null;
        public BikeCompareListController(IBikeCompare bikeCompare)
        {
            _bikeCompare = bikeCompare;
        }

        /// <summary>
        /// Gets the Top 'n' Bike Compare List
        /// </summary>
        /// <param name="topCount">Top count</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<TopBikeCompareBase>))]
        public IHttpActionResult Get(UInt16 topCount)
        {
            IEnumerable<TopBikeCompareBase> topBikeComapreList = null;
            try
            {
                topBikeComapreList = _bikeCompare.CompareList(topCount);
                if (topBikeComapreList != null)
                {
                    return Ok(topBikeComapreList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.Compare.BikeCompareListController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
