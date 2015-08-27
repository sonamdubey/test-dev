using Bikewale.DAL.Compare;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
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
        /// <summary>
        /// Gets the Top 'n' Bike Compare List
        /// </summary>
        /// <param name="topCount">Top count</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<TopBikeCompareBase>))]
        public HttpResponseMessage Get(UInt16 topCount)
        {
            IEnumerable<TopBikeCompareBase> topBikeComapreList = null;
            using (IUnityContainer container = new UnityContainer())
            {
                IBikeCompare bikeCompare = null;

                container.RegisterType<IBikeCompare, BikeCompareRepository>();
                bikeCompare = container.Resolve<IBikeCompare>();

                topBikeComapreList = bikeCompare.CompareList(topCount);
                if (topBikeComapreList != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, topBikeComapreList);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                }
            }

        }
    }
}
