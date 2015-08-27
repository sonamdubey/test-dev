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
    public class FeaturedBikeController : ApiController
    {
        // GET: FeaturedBike
        #region GetFeaturedBikeList Method
        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 aUG 2015
        /// Summary : Get Featured Bike list
        /// </summary>
        /// <param name="topCount">Top Count</param>
        /// <returns></returns>
        public HttpResponseMessage Get(uint topCount)
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

                    FeaturedList.FeaturedBike = Mapper.Map<List<FeaturedBikeEntity>, List<FeaturedBike>>(objFeature);

                    if (objFeature != null && objFeature.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, FeaturedList);
                    else
                        return Request.CreateResponse(HttpStatusCode.NoContent, "no data found");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.BikeDiscover.GetFeaturedBikeList");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
        #endregion
    }
}