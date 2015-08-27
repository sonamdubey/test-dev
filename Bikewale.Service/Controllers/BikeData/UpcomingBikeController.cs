using AutoMapper;
using Bikewale.BAL.Pager;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Bikewale.Service.Controllers.BikeData
{
    public class UpcomingBikeController : ApiController
    {
        #region GetUpcomingBikeList Method
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
        public HttpResponseMessage Get(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo=null)
        {
            UpcomingBikeList upcomingBikes = new UpcomingBikeList();
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    int recordCount = 0;
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsRepository<BikeModelEntity, int> modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();

                    container.RegisterType<IPager, Pager>();
                    IPager objPager = container.Resolve<IPager>();

                    int startIndex = 0, endIndex = 0, currentPageNo=0;
                    currentPageNo = curPageNo.HasValue ? curPageNo.Value : 1;

                    objPager.GetStartEndIndex(pageSize, currentPageNo, out startIndex, out endIndex);

                    UpcomingBikesListInputEntity inputParams = new UpcomingBikesListInputEntity()
                    {
                        StartIndex = startIndex,
                        EndIndex = endIndex,
                        MakeId = makeId.HasValue ? makeId.Value : 0,
                        ModelId = modelId.HasValue ? modelId.Value : 0
                    };

                    List<UpcomingBikeEntity> objUpcoming = modelRepository.GetUpcomingBikesList(inputParams, sortBy, out recordCount);

                    Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    Mapper.CreateMap<BikeDescriptionEntity, BikeDiscription>();
                    Mapper.CreateMap<UpcomingBikeEntity, UpcomingBike>();

                    upcomingBikes.UpcomingBike = Mapper.Map<List<UpcomingBikeEntity>, List<UpcomingBike>>(objUpcoming);

                    if (objUpcoming != null && objUpcoming.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, upcomingBikes);
                    else
                        return Request.CreateResponse(HttpStatusCode.NoContent, "no data found");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.BikeDiscover.GetUpcomingBikeList");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
        #endregion
    }
}