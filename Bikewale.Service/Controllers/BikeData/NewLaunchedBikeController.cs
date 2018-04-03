using Bikewale.BAL.GrpcFiles.Specs_Features;
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
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        private readonly IPager _objPager = null;
        private readonly IBikeModelsCacheRepository<int> _modelCacheRepository = null;
        private readonly INewBikeLaunchesBL _newBikeLaunchBL = null;

        /// <summary>
        /// Modified by :   Sumit Kate on 13 Feb 2017
        /// Description :   Assign INewBikeLaunchesBL object
        /// </summary>
        /// <param name="modelRepository"></param>
        /// <param name="objPager"></param>
        /// <param name="modelCacheRepository"></param>
        /// <param name="newBikeLaunchBL"></param>
        public NewLaunchedBikeController(IBikeModelsRepository<BikeModelEntity, int> modelRepository, IPager objPager, IBikeModelsCacheRepository<int> modelCacheRepository, INewBikeLaunchesBL newBikeLaunchBL)
        {
            _modelRepository = modelRepository;
            _objPager = objPager;
            _modelCacheRepository = modelCacheRepository;
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
                LaunchedBikeList objLaunched = new LaunchedBikeList();
                int startIndex = 0, endIndex = 0, currentPageNo = 1;
                currentPageNo = curPageNo.HasValue ? curPageNo.Value : 1;

                _objPager.GetStartEndIndex(pageSize, currentPageNo, out startIndex, out endIndex);

                IEnumerable<NewLaunchedBikeEntity> objRecent = _modelCacheRepository.GetNewLaunchedBikesList(startIndex, endIndex).Models;

                objLaunched.LaunchedBike = LaunchedBikeListMapper.Convert(objRecent);

                if (objLaunched != null && objLaunched.LaunchedBike != null && objLaunched.LaunchedBike.Any())
                    return Ok(objLaunched);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.BikeDiscover.GetRecentlyLaunchedBikeList");
               
                return InternalServerError();
            }
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
                    NewLaunchedBikeResult entity = _newBikeLaunchBL.GetBikes(filterEntity);
                    IEnumerable<NewLaunchedBikeEntityBase> newLaunchesList = entity.Bikes;
                    if (newLaunchesList != null && newLaunchesList.Any())
                    {
                        IEnumerable<VersionMinSpecsEntity> versionMinSpecs = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(newLaunchesList.Select(m => m.VersionId));
                        if (versionMinSpecs != null)
                        {
                            var minSpecs = versionMinSpecs.GetEnumerator();
                            foreach (var bike in newLaunchesList)
                            {
                                if (minSpecs.MoveNext())
                                {
                                    bike.MinSpecsList = minSpecs.Current.MinSpecsList;
                                }
                            }
                        }
                    }
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