using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Pager;
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
    /// </summary>
    public class UpcomingBikeController : CompressionApiController//ApiController
    {
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _makeRepository = null;
        private readonly IPager _objPager = null;
        public UpcomingBikeController(IBikeModelsRepository<BikeModelEntity, int> modelRepository, IPager objPager, IBikeMakes<BikeMakeEntity, int> makeRepository)
        {
            _modelRepository = modelRepository;
            _objPager = objPager;
            _makeRepository = makeRepository;
        }

        #region GetUpcomingBikeList PopulateWhere
        /// <summary>
        /// Created By : Sadhana Upadhyay on 25 Aug 2015
        /// Summary : To get Upcoming Bike List
        /// </summary>
        /// <param name="sortBy">Default = 0, PriceLowToHigh = 1, PriceHighToLow = 2, LaunchDateSooner = 3, LaunchDateLater = 4</param>
        /// <param name="pageSize">No of Records</param>
        /// <param name="makeId">Make Id (Optional)</param>
        /// <param name="modelId">Model Id(Optional)</param>
        /// <param name="curPageNo">Current Page No (Optional)</param>
        /// <returns></returns>
        public IHttpActionResult Get(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null)
        {
            UpcomingBikeList upcomingBikes = new UpcomingBikeList();
            int recordCount = 0;
            int startIndex = 0, endIndex = 0, currentPageNo = 0;
            try
            {
                currentPageNo = curPageNo.HasValue ? curPageNo.Value : 1;

                _objPager.GetStartEndIndex(pageSize, currentPageNo, out startIndex, out endIndex);

                UpcomingBikesListInputEntity inputParams = new UpcomingBikesListInputEntity()
                {
                    StartIndex = startIndex,
                    EndIndex = endIndex,
                    MakeId = makeId.HasValue ? makeId.Value : 0,
                    ModelId = modelId.HasValue ? modelId.Value : 0
                };

                List<UpcomingBikeEntity> objUpcoming = _modelRepository.GetUpcomingBikesList(inputParams, sortBy, out recordCount);

                upcomingBikes.UpcomingBike = UpcomingBikeListMapper.Convert(objUpcoming);

                if (objUpcoming != null)
                {
                    objUpcoming.Clear();
                    objUpcoming = null;
                }

                if (upcomingBikes != null && upcomingBikes.UpcomingBike != null && upcomingBikes.UpcomingBike.Count() > 0)
                    return Ok(upcomingBikes);
                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.BikeDiscover.GetUpcomingBikeList");
                objErr.SendMail();
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

                if (objMakeList != null && objMakeList.Count() > 0)
                {
                    objDTOMakeList = new MakeList();

                    objDTOMakeList.Makes = MakeListMapper.Convert(objMakeList);

                    objMakeList = null;

                    return Ok(objDTOMakeList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UpcomingBikeController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }
    }
}