using AutoMapper;
using Bikewale.BAL.BikeData;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
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
    public class SimilarBikeController : ApiController
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 25 Aug 2015
        /// Summary : To get Alternative/Similar bikes Lisr
        /// </summary>
        /// <param name="versionId">Version Id</param>
        /// <param name="topCount">Top Count (Optional)</param>
        /// <param name="deviation">Deviation (Optional)</param>
        /// <returns></returns>
        public HttpResponseMessage Get(int versionId, uint topCount, uint? deviation =null)
        {
            SimilarBikeList objSimilar=new SimilarBikeList();
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersions<BikeVersionEntity, int>, BikeVersions<BikeVersionEntity, int>>();
                    IBikeVersions<BikeVersionEntity, int> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, int>>();

                    uint percentDeviation = deviation.HasValue ? deviation.Value : 15;

                    List<SimilarBikeEntity> objSimilarBikes = objVersion.GetSimilarBikesList(versionId, topCount, percentDeviation);

                    Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
                    Mapper.CreateMap<SimilarBikeEntity, SimilarBike>();

                    objSimilar.SimilarBike = Mapper.Map<List<SimilarBikeEntity>, List<SimilarBike>>(objSimilarBikes);

                    if (objSimilarBikes != null && objSimilarBikes.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, objSimilar);
                    else
                        return Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.BikeDiscover.GetFeaturedBikeList");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}