using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
namespace Bikewale.common
{
    /// <summary>
    /// Created By: Sangram Nandkhile on 23 Nov 2016
    /// Desc: Common Function to return Model related data
    /// This class will enable us to reduce code redundancy 
    /// </summary>
    public class ModelHelper
    {
        /// <summary>
        /// Created by : Sangram Nandkhile on 23 Nov 2016
        /// Description: Method to get Model name by makeId.
        /// </summary>
        public BikeModelEntity GetModelDataById(uint modelId)
        {
            BikeModelEntity objModel = default(BikeModelEntity);
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    IBikeModels<BikeModelEntity, int> obj = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                    objModel = obj.GetById((int)modelId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ModelHelper.GetModelDataById() - ModelId :{0}", modelId));
                objErr.SendMail();
            }
            return objModel;
        }


        /// <summary>
        /// Created by : Sangram Nandkhile on 23 Nov 2016
        /// Description: Method to get Model name by makeId.
        /// </summary>
        public ModelMaskingResponse GetModelDataByMasking(string modelMaskingName)
        {
            ModelMaskingResponse objModel = default(ModelMaskingResponse);
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                    objModel = objCache.GetModelMaskingResponse(modelMaskingName);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ModelHelper.GetModelDataById() - modelMaskingName :{0}", modelMaskingName));
                objErr.SendMail();
            }
            return objModel;
        }
    }
}