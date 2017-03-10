using Bikewale.Notifications;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Service.AutoMappers.BikeData;
using System;
using System.Web.Http;

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
        /// Modified By : Sajal Gupta on 10-03-2017
        /// Description : Fetch scooter synopsis along with bike synopsis
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/makes/{makeid}/synopsis/")]
        public IHttpActionResult GetSynopsis(int makeId)
        {
            if (makeId > 0)
            {
                SynopsisData objSynopsis = null;

                try
                {
                    objSynopsis = makesRepo.Getsynopsis(makeId);
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "GetSynopsis");

                    return InternalServerError();
                }

                if (objSynopsis != null)
                    return Ok(BikeDataMapper.Convert(objSynopsis));
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Writtten By : Ashish G. Kamble on 3 Feb 2017
        /// Summary : api to update the synopsis for the given make id
        /// Modified By : Sajal Gupta on 10-03-2017
        /// Description : Save scooter synopsis along with bike synopsis
        /// </summary>
        /// <param name="makeId">null not allowed</param>
        /// <param name="synopsis">null not allowed</param>
        /// <returns></returns>
        [HttpPost, Route("api/makes/{makeid}/synopsis/")]
        public IHttpActionResult SaveSynopsis(int makeId, [FromBody] SynopsisDataDto objSynopsisDto)
        {
            SynopsisData objSynopsis = BikeDataMapper.Convert(objSynopsisDto);

            if (makeId > 0 && !String.IsNullOrEmpty(objSynopsis.BikeDescription))
            {
                try
                {
                    int userId = 0;
                    int.TryParse(Bikewale.Utility.OprUser.Id, out userId);

                    makesRepo.UpdateSynopsis(makeId, userId, objSynopsis);
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
