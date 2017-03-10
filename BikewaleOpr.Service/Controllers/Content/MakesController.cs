using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bikewale.Notifications;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Interface.BikeData;
using Microsoft.Practices.Unity;

namespace BikewaleOpr.Service.Controllers.Content
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// </summary>
    [Authorize]
    public class MakesController : ApiController
    {
        private readonly IBikeMakes makesRepo = null;

        public MakesController(IBikeMakes _makeRepo)
        {
            makesRepo = _makeRepo;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble
        /// Summary : API to get the synopsis for the given make
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/makes/{makeid}/synopsis/")]
        public IHttpActionResult GetSynopsis(int makeId)
        {
            if (makeId > 0)
            {
                string synopsis = string.Empty;

                try
                {
                    synopsis = makesRepo.Getsynopsis(makeId);
                }
                catch (Exception ex)
                {                    
                    ErrorClass objErr = new ErrorClass(ex, "GetSynopsis");
                    
                    return InternalServerError();
                }

                if (!String.IsNullOrEmpty(synopsis))
                    return Ok(synopsis);
                else
                    return NotFound();
            }
            else
                return BadRequest();        
        }

        /// <summary>
        /// Writtten By : Ashish G. Kamble on 3 Feb 2017
        /// Summary : api to update the synopsis for the given make id
        /// </summary>
        /// <param name="makeId">null not allowed</param>
        /// <param name="synopsis">null not allowed</param>
        /// <returns></returns>
        [HttpPost, Route("api/makes/{makeid}/synopsis/")]
        public IHttpActionResult SaveSynopsis(int makeId, [FromBody] string synopsis)
        {
            if (makeId > 0 && !String.IsNullOrEmpty(synopsis))
            {
                try
                {
                    int userId = 0;
                    int.TryParse(Bikewale.Utility.OprUser.Id, out userId);

                    makesRepo.UpdateSynopsis(makeId, synopsis, userId);
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "SaveSynopsis");
                    
                    return InternalServerError();
                }
            }
            else
                return BadRequest("Invalid inputs");

            return Ok();
        }

    }   // class
}   // namespace
