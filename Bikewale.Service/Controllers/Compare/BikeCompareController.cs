using Bikewale.DAL.Compare;
using Bikewale.DTO.Compare;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Compare;
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
    /// Bike Compare controller
    /// Author  :   Sumit Kate
    /// Created On  :   26 Aug 2015
    /// </summary>
    public class BikeCompareController : ApiController
    {
        private readonly IBikeCompare _bikeCompare = null;
        private readonly IBikeCompareCacheRepository _cache = null;
        /// <summary>
        /// Constructor to initialize the members
        /// </summary>
        /// <param name="bikeCompare"></param>
        /// <param name="cache"></param>
        public BikeCompareController(IBikeCompare bikeCompare, IBikeCompareCacheRepository cache)
        {
            _bikeCompare = bikeCompare;
            _cache = cache;
        }

        /// <summary>
        /// Gets the Bike Comparision details
        /// </summary>
        /// <param name="versionList">Bike version list(comma separated values)</param>
        /// <returns></returns>
        [ResponseType(typeof(BikeCompareDTO))]
        public IHttpActionResult Get(string versionList)
        {
            BikeCompareEntity compareEntity = null;
            BikeCompareDTO compareDTO = null;
            try
            {
                compareEntity = _cache.DoCompare(versionList);
                if (compareEntity != null)
                {
                    // If android, IOS client sanitize the article content 
                    string platformId = string.Empty;

                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }

                    if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                    {
                        compareEntity.Features = null;
                        compareEntity.Specifications = null;
                        compareEntity.Color = null;
                    }
                    else
                    {
                        compareEntity.ComapareSpecifications = null;
                        compareEntity.CompareColors = null;
                        compareEntity.CompareFeatures = null;
                    }
                    compareDTO = BikeCompareEntityMapper.Convert(compareEntity);
                    return Ok(compareDTO);
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
