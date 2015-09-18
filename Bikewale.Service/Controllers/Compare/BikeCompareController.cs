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

namespace Bikewale.Service.Controllers.Compare
{
    /// <summary>
    /// Bike Compare controller
    /// Author  :   Sumit Kate
    /// Created On  :   26 Aug 2015
    /// </summary>
    public class BikeCompareController : ApiController
    {
        private readonly IBikeCompare _bikeCompare = null;
        public BikeCompareController(IBikeCompare bikeCompare)
        {
            _bikeCompare = bikeCompare;
        }

        /// <summary>
        /// Gets the Bike Comparision details
        /// </summary>
        /// <param name="versionList">Bike version list(comma separated values)</param>
        /// <returns></returns>
        public IHttpActionResult Get(string versionList)
        {
            BikeCompareEntity compareEntity = null;
            try
            {
                compareEntity = _bikeCompare.DoCompare(versionList);
                if (compareEntity != null)
                {
                    return Ok(compareEntity);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.Compare.BikeCompareController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
