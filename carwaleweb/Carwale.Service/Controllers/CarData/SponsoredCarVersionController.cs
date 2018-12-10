using Carwale.BL.SponsoredCar;
using Carwale.DAL.SponsoredCar;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Notifications;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Carwale.Service.Controllers.CarData
{
    public class SponsoredCarVersionController : ApiController
    {
        private readonly ISponsoredCarCache _sponsoredCars;

        public SponsoredCarVersionController(ISponsoredCarCache sponsoredCars)
        {
            _sponsoredCars = sponsoredCars;
        }

        /// <summary>
        /// Created Date : 27/8/2014\
        /// Author : Supriya Khartode
        /// Desc : Populates the sponsored car version based on versionids,categoryId & platformid passed
        /// Parameters : CategoryId = 1(compare car) , 2 (pricequote)
        ///            : PlatformId = 1(carwale) , 2 (bikewale)
        /// Modified by: Rakesh Yadav On 02 Sep 2015
        /// desc:Resolving dependency injection using UnityBootstraper and UnityResolver and send status code using web api 2
        /// </summary>
        /// <returns>FeaturedVersionId</returns>
        public IHttpActionResult GetSponsoredCarVersion(string vids, int categoryId, int platformId)
        {
            if (String.IsNullOrEmpty(vids) || categoryId <= 0 || platformId <= 0)
                return BadRequest();

            return Ok(_sponsoredCars.GetFeaturedCar(vids, categoryId, platformId));
        }

    }
}