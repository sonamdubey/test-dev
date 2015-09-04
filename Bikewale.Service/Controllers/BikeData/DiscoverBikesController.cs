using AutoMapper;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
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
    public class DiscoverBikesController : ApiController
    {
        #region GetFeaturedBikeList Method
        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 aUG 2015
        /// sUMMARY : Get Featured BIke list
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public HttpResponseMessage GetFeaturedBikeList(uint topCount)
        {
            FeaturedBikeList FeaturedList = new FeaturedBikeList();
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsRepository<BikeModelEntity, int> modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();

                    List<FeaturedBikeEntity> objFeature = modelRepository.GetFeaturedBikes(topCount);

                    Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    Mapper.CreateMap<FeaturedBikeEntity, FeaturedBike>();
                        //.ForMember(src => src.MakeBase, des => des.MapFrom(y => Mapper.Map <BikeMakeEntityBase, MakeBase>(y.MakeBase)))
                        //.ForMember(src => src.ModelBase, des => des.MapFrom(y => Mapper.Map<BikeModelEntityBase, ModelBase>(y.ModelBase)));

                    FeaturedList.FeaturedBike = Mapper.Map<List<FeaturedBikeEntity>, List<FeaturedBike>>(objFeature);

                    if (objFeature != null && objFeature.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, FeaturedList);
                    else
                        return Request.CreateResponse(HttpStatusCode.NoContent, "no data found");
                }
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.BikeDiscover.GetFeaturedBikeList");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
        #endregion

        #region GetRecentlyLaunchedBikeList Method
        public HttpResponseMessage GetRecentlyLaunchedBikeList(uint startIndex,uint endIndex)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    int recordCount = 0;
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsRepository<BikeModelEntity, int> modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();

                    List<NewLaunchedBikeEntity> objRecent = modelRepository.GetNewLaunchedBikesList(Convert.ToInt32(startIndex), Convert.ToInt32(endIndex), out recordCount);

                    if (objRecent != null && objRecent.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, "");
                    else
                        return Request.CreateResponse(HttpStatusCode.NoContent, "no data found");
                }
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.BikeDiscover.GetRecentlyLaunchedBikeList");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
        #endregion

        #region GetUpcomingBikeList Method
        public HttpResponseMessage GetUpcomingBikeList(EnumUpcomingBikesFilter sortBy, [FromBody]UpcomingBikesListInputEntity inputParams)
        {
            UpcomingBikeList upcomingBikes=new UpcomingBikeList();
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    int recordCount = 0;
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsRepository<BikeModelEntity, int> modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();

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
    }   //class
}   //namespace