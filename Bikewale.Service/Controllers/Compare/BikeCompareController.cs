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
        /// <summary>
        /// Gets the Bike Comparision details
        /// </summary>
        /// <param name="versionList">Bike version list(comma separated values)</param>
        /// <returns></returns>
        public HttpResponseMessage Get(string versionList)
        {
            BikeCompareEntity compareEntity = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeCompare bikeCompare = null;

                    container.RegisterType<IBikeCompare, BikeCompareRepository>();
                    bikeCompare = container.Resolve<IBikeCompare>();

                    compareEntity = bikeCompare.DoCompare(versionList);
                    if (compareEntity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, compareEntity);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.Compare.BikeCompareController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
