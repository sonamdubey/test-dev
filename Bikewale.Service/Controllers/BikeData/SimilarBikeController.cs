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
        /// Summary : To get Alternative/Similar bikes List
        /// </summary>
        /// <param name="versionId">Version Id</param>
        /// <param name="topCount">Top Count (Optional)</param>
        /// <returns></returns>
        public IHttpActionResult Get(int versionId, uint topCount)
        {
            Bikewale.DTO.BikeData.SimilarBikeList objSimilar = new Bikewale.DTO.BikeData.SimilarBikeList();
            try
            {
                IEnumerable<SimilarBikeEntity> objSimilarBikes = _objVersion.GetSimilarBikesList(versionId, topCount, 1, true);

                objSimilar.SimilarBike = SimilarBikeListMapper.Convert(objSimilarBikes);

                if (objSimilar != null && objSimilar.SimilarBike != null && objSimilar.SimilarBike.Any())
                    return Ok(objSimilar);
                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.BikeDiscover.GetFeaturedBikeList");
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// Author  : Kartik Rathod on 11 May 2018
        /// Desc    : Get similar bikes based on road price for emi page in finance
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="topcount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet,Route("api/similarbikes/model/{versionId}/finance/")]
        public IHttpActionResult GetSimilarBikesForEMI(int versionId, short topcount, int cityId)
        {
            try
            {
                if (versionId > 0)
                {
                    IEnumerable<SimilarBikesForEMIEntity> objBikeList = _objVersion.GetSimilarBikesForEMI(versionId, topcount, cityId);
                    if (objBikeList != null && objBikeList.Any())
                    {
                        return Ok(objBikeList);
                    }   
                    else
                    {
                        return NotFound();
                    }   
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.SimilarBikeController.GetSimilarBikesForEMI");
                return InternalServerError();
            }
        }

    }
}