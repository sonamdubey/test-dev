using Bikewale.BAL.Pager;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.DTO.BikeData;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using AutoMapper;
using Bikewale.DTO.Series;

namespace Bikewale.Service.Controllers.BikeData
{
    public class NewLaunchedBikeController : ApiController
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 25 Aug 2015
        /// Summary : To get Recently Launched Bike
        /// </summary>
        /// <param name="pageSize">No. of Record</param>
        /// <param name="curPageNo">Current Page No. (Optional)</param>
        /// <returns></returns>
        public HttpResponseMessage Get(int pageSize,int? curPageNo = null)
        {
            try
            {
                LaunchedBikeList objLaunched = new LaunchedBikeList();
                using (IUnityContainer container = new UnityContainer())
                {
                    int recordCount = 0;
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsRepository<BikeModelEntity, int> modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();

                    container.RegisterType<IPager, Pager>();
                    IPager objPager = container.Resolve<IPager>();

                    int startIndex = 0, endIndex = 0, currentPageNo = 1;
                    currentPageNo = curPageNo.HasValue ? curPageNo.Value : 1;

                    objPager.GetStartEndIndex(pageSize, currentPageNo, out startIndex, out endIndex);

                    List<NewLaunchedBikeEntity> objRecent = modelRepository.GetNewLaunchedBikesList(startIndex, endIndex, out recordCount);

                    Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
                    Mapper.CreateMap<NewLaunchedBikeEntity, LaunchedBike>();

                    objLaunched.LaunchedBike = Mapper.Map<List<NewLaunchedBikeEntity>, List<LaunchedBike>>(objRecent);

                    if (objRecent != null && objRecent.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, objLaunched);
                    else
                        return Request.CreateResponse(HttpStatusCode.NoContent, "no data found");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.BikeDiscover.GetRecentlyLaunchedBikeList");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}