using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
//using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Bikewale.DAL.BikeData;
using System.Net.Http;
using Bikewale.BAL.BikeData;
using System.Net;

namespace Bikewale.Service.Controllers
{
    public class BikeModelsController : ApiController
    {
        //[HttpGet]
        //public HttpResponseMessage GetBikeModels(int makeId, EnumBikeType requestType)
        //{
        //    List<BikeModelEntityBase> objModels = null;
        //    using (IUnityContainer container = new UnityContainer())
        //    {
        //        container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
        //        IBikeModels<BikeModelEntity, int> objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();

        //        objModels = objModel.GetModelsByType(requestType, makeId);
        //    }
        //    return Request.CreateResponse<List<BikeModelEntityBase>>(HttpStatusCode.OK, objModels);
        //}

        //public string GetBikeModels()
        //{
        //    return "testing successful";
        //}

        //public ModelMaskingResponse GetMaskingName(string maskingName)
        //{
        //    ModelMaskingResponse objResponse = null;

        //    using (IUnityContainer container = new UnityContainer())
        //    {
        //        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
        //                 .RegisterType<ICacheManager, MemcacheManager>()
        //                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
        //                ;
        //        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

        //        objResponse = objCache.GetModelMaskingResponse(maskingName);
        //    }

        //    return objResponse;
        //}

    }   // class
}   // namespace
