using Bikewale.DTO.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bikewale.Service.Controllers.BikeData
{
    /// <summary>
    /// Similar Bike Controller
    /// Created By : Sadhana Upadhyay on 25 Aug 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class SimilarBikeController : CompressionApiController//ApiController
    {
        private readonly IBikeVersions<BikeVersionEntity, int> _objVersion = null;

        public SimilarBikeController(IBikeVersions<BikeVersionEntity, int> objVersion)
        {
            _objVersion = objVersion;
        }
        /// <summary>
        /// Created By : Sadhana Upadhyay on 25 Aug 2015
        /// Summary : To get Alternative/Similar bikes Lisr
        /// </summary>
        /// <param name="versionId">Version Id</param>
        /// <param name="topCount">Top Count (Optional)</param>
        /// <param name="deviation">Deviation (Optional)</param>
        /// <returns></returns>
        public IHttpActionResult Get(int versionId, uint topCount, uint? deviation = null)
        {
            SimilarBikeList objSimilar = new SimilarBikeList();
            try
            {

                uint percentDeviation = deviation.HasValue ? deviation.Value : 15;

                List<SimilarBikeEntity> objSimilarBikes = _objVersion.GetSimilarBikesList(versionId, topCount, percentDeviation);

                objSimilar.SimilarBike = SimilarBikeListMapper.Convert(objSimilarBikes);


                if (objSimilarBikes != null)
                {
                    objSimilarBikes.Clear();
                    objSimilarBikes = null;
                }

                if (objSimilar != null && objSimilar.SimilarBike != null && objSimilar.SimilarBike.Count() > 0)
                    return Ok(objSimilar);
                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.BikeDiscover.GetFeaturedBikeList");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}