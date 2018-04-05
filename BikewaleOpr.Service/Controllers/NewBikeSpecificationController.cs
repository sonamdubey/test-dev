using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 01 Ape 2018
    /// Description : Provide APIs for bike specifications and features.
    /// </summary>
    public class NewBikeSpecificationController : ApiController
    {
        private readonly IBikeModels _IBikeModels;
        public NewBikeSpecificationController(IBikeModels bikeModels)
        {
            _IBikeModels = bikeModels;
        }
        
        /// <summary>
        /// Created by : Ashutosh Sharma on 02 Apr 2018
        /// Description : API to update bike min specs in elastic search index when specs are updated for a version.
        /// </summary>
        /// <param name="versionId">Version Id for which min specs to be updated in elastic index.</param>
        /// <param name="specItemList">List of specs items with updated values which need to updated in ealstic index.</param>
        /// <returns></returns>
        [HttpPost, Route("api/versions/{versionId}/specs/")]
        public IHttpActionResult UpdateSpecsInESIndex(int versionId, [FromBody]IEnumerable<SpecsItem> specItemList)
        {
            try
            {
                if (versionId > 0 && specItemList != null)
                {
                    _IBikeModels.UpdateMinSpecsInESIndex(versionId, specItemList);
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
