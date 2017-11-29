using Bikewale.DTO.BikeData;
using Bikewale.DTO.BikeData.Upcoming;
using Bikewale.DTO.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using Bikewale.Service.AutoMappers.Make;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeData
{
    /// <summary>
    /// Upcoming Bike Controller
    /// Created By : Sadhana Upadhyay on 25 Aug 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// Modified by : Vivek Singh Tomar on 31st July 2017
    /// Summary : Added IUpcoming for filling upcoming bike list
    /// </summary>
    public class UpcomingBikeController : CompressionApiController//ApiController
    {
        private readonly IBikeModelsCacheRepository<int> _modelCacheRepository = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _makeRepository = null;
        private readonly IUpcoming _upcomingBL = null;

        public UpcomingBikeController(IBikeMakes<BikeMakeEntity, int> makeRepository, IBikeModelsCacheRepository<int> modelCacheRepository, IUpcoming upcomingBL)
        {
            _makeRepository = makeRepository;
            _modelCacheRepository = modelCacheRepository;
            _upcomingBL = upcomingBL;
        }

        #region GetUpcomingBikeList PopulateWhere
        /// <summary>
        /// Created By : Sadhana Upadhyay on 25 Aug 2015
        /// Summary : To get Upcoming Bike List
        /// </summary>
        /// Modified by: Vivek Singh Tomar on 31st July 2017
        /// Summary    : Replaced logic of fetching upcoming bike list.
        /// <param name="sortBy">Default = 0, PriceLowToHigh = 1, PriceHighToLow = 2, LaunchDateSooner = 3, LaunchDateLater = 4</param>
        /// <param name="pageSize">No of Records</param>
        /// <param name="makeId">Make Id (Optional)</param>
        /// <param name="modelId">Model Id(Optional)</param>
        /// <param name="curPageNo">Current Page No (Optional)</param>
        /// <returns></returns>
        public IHttpActionResult Get(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null)
        {
            UpcomingBikeList upcomingBikes = new UpcomingBikeList();
            int currentPageNo = 0;
            try
            {
                currentPageNo = curPageNo.HasValue ? curPageNo.Value : 1;
                var objFiltersUpcoming = new UpcomingBikesListInputEntity()
                {
                    PageSize = pageSize,
                    MakeId = Convert.ToInt32(makeId),
                    ModelId = Convert.ToInt32(modelId),
                    PageNo = currentPageNo
                };
                IEnumerable<UpcomingBikeEntity> objUpcoming = _upcomingBL.GetModels(objFiltersUpcoming, sortBy);

                upcomingBikes.UpcomingBike = UpcomingBikeListMapper.Convert(objUpcoming);

                if (objUpcoming != null)
                {
                    objUpcoming = null;
                }

                if (upcomingBikes != null && upcomingBikes.UpcomingBike != null && upcomingBikes.UpcomingBike.Any())
                    return Ok(upcomingBikes);
                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.BikeDiscover.GetUpcomingBikeList");
               
                return InternalServerError();
            }
        }
        #endregion

        /// <summary>
        /// Created By  :   Sumit Kate on 16 Nov 2015
        /// Summary     :   Returns the Upcoming Bike's Make list
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(MakeList))]
        public IHttpActionResult Get()
        {
            IEnumerable<BikeMakeEntityBase> objMakeList = null;
            MakeList objDTOMakeList = null;
            try
            {
                objMakeList = _makeRepository.UpcomingBikeMakes();

                if (objMakeList != null && objMakeList.Any())
                {
                    objDTOMakeList = new MakeList();

                    objDTOMakeList.Makes = MakeListMapper.Convert(objMakeList);

                    objMakeList = null;

                    return Ok(objDTOMakeList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.UpcomingBikeController.Get");
               
                return InternalServerError();
            }
            return NotFound();
        }


        /// <summary>
        /// Created by  :   Sajal Gupta on 10-04-2017
        /// Description :   API to return Upcoming bikes
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet, ResponseType(typeof(UpcomingBikeResultDTO)), Route("api/upcoming/")]
        public IHttpActionResult Get([FromUri]InputFilterDTO filter)
        {
            try
            {
                if (ModelState.IsValid && filter != null)
                {
                    UpcomingBikesListInputEntity filterEntity = UpcomingBikeListMapper.Convert(filter);

                    var bikesResult = _upcomingBL.GetBikes(filterEntity, EnumUpcomingBikesFilter.LaunchDateSooner);



                    if (bikesResult.TotalCount > 0)
                    {
                        UpcomingBikeResultDTO dto = UpcomingBikeListMapper.Convert(bikesResult);
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