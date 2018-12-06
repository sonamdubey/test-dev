using Bikewale.DTO.BikeData;
using Bikewale.DTO.BikeData.NewLaunched;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeData
{
    /// <summary>
    /// New Launched Bike Controller
    /// Created By : Sadhana Upadhyay on 25 Aug 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class NewLaunchedBikeController : CompressionApiController//ApiController
    {
        private readonly IPager _objPager = null;
        private readonly INewBikeLaunchesBL _newBikeLaunchBL = null;

        /// <summary>
        /// Modified by :   Sumit Kate on 13 Feb 2017
        /// Description :   Assign INewBikeLaunchesBL object
        /// Modified by :   Rajan Chauhan on 17 Apr 2018
        /// Description :   Removed IBikeModelsRepository and IBikeModelsCacheRepository dependency
        /// </summary>
        /// <param name="objPager"></param>
        /// <param name="newBikeLaunchBL"></param>
        public NewLaunchedBikeController(IPager objPager, INewBikeLaunchesBL newBikeLaunchBL)
        {
            _objPager = objPager;
            _newBikeLaunchBL = newBikeLaunchBL;
        }
        /// <summary>
        /// Created By : Sadhana Upadhyay on 25 Aug 2015
        /// Summary : To get Recently Launched Bike
        /// </summary>
        /// <param name="pageSize">No. of Record</param>
        /// <param name="curPageNo">Current Page No. (Optional)</param>
        /// <returns></returns>
        [ResponseType(typeof(LaunchedBikeList))]
        public IHttpActionResult Get(int pageSize, int? curPageNo = null)
        {
            try
            {
                int startIndex = 0, endIndex = 0, currentPageNo = 1;
                currentPageNo = curPageNo.HasValue ? curPageNo.Value : 1;

                _objPager.GetStartEndIndex(pageSize, currentPageNo, out startIndex, out endIndex);
                NewLaunchedBikesBase objNewLaunched = _newBikeLaunchBL.GetNewLaunchedBikesList(startIndex, endIndex);
                if (objNewLaunched != null)
                {
                    IEnumerable<NewLaunchedBikeEntity> objRecent = objNewLaunched.Models;
                    LaunchedBikeList objLaunched = new LaunchedBikeList();
                    objLaunched.LaunchedBike = LaunchedBikeListMapper.Convert(objRecent);
                    if (objLaunched.LaunchedBike != null && objLaunched.LaunchedBike.Any())
                        return Ok(objLaunched);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.BikeDiscover.GetRecentlyLaunchedBikeList");
               
                return InternalServerError();
            }
            return NotFound();
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   API to return New Launched bikes
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet, ResponseType(typeof(NewLaunchedBikeResultDTO)), Route("api/v2/newlaunched/")]
        public IHttpActionResult Get([FromUri]InputFilterDTO filter)
        {
            try
            {
                if (ModelState.IsValid && filter != null)
                {
                    InputFilter filterEntity = LaunchedBikeListMapper.Convert(filter);
                    if(filterEntity != null)
                    {
                        NewLaunchedBikeResult entity = _newBikeLaunchBL.GetBikes(filterEntity);
                        if (entity.TotalCount > 0)
                        {
                            NewLaunchedBikeResultDTO dto = LaunchedBikeListMapper.Convert(entity);
                            return Ok(dto);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    return NotFound();
                    
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("NewLaunchedBikeController.Get({0})", filter));
                return InternalServerError();
            }
        }
    }
}