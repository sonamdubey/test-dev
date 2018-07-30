using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.DTO.Compare;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Compare;
using System;
using System.Linq;
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
        private readonly IApiGatewayCaller _apiGatewayCaller;
        /// <summary>
        /// Constructor to initialize the members
        /// </summary>
        
        public BikeCompareController(IBikeCompare bikeCompare, IBikeCompareCacheRepository cache, IApiGatewayCaller apiGatewayCaller)
        {
            _bikeCompare = bikeCompare;
            _cache = cache;
            _apiGatewayCaller = apiGatewayCaller;
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
                        GetVersionSpecsByIdAdapter adapt1 = new GetVersionSpecsByIdAdapter();
                        adapt1.AddApiGatewayCall(_apiGatewayCaller, versionList.Split(',').Select(int.Parse).ToList());
                        _apiGatewayCaller.Call();

                        compareEntity.VersionSpecsFeatures = adapt1.Output;
                        compareEntity.Features = null;
                        compareEntity.Specifications = null;
                        compareEntity.Color = null;
                    }
                    else
                    {
                        compareEntity.CompareColors = null;
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Compare.BikeCompareController.Get");
               
                return InternalServerError();
            }
        }
    }
}
