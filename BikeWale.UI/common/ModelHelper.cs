using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
namespace Bikewale.Common
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
        /// modified By:-Subodh jain 9 jan 2017
        /// Description :- Added cache call
        /// </summary>
        public BikeModelEntity GetModelDataById(uint modelId)
        {
            BikeModelEntity objModel = default(BikeModelEntity);
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    var obj = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                    objModel = obj.GetById((int)modelId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ModelHelper.GetModelDataById() - ModelId :{0}", modelId));
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("ModelHelper.GetModelDataByMasking() - modelMaskingName :{0}", modelMaskingName));
            }
            return objModel;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Get List of Area depending on City and Model Id
        /// </summary>
        public IEnumerable<AreaEntityBase> GetAreaForModelAndCity(uint modelId, uint cityId)
        {
            IEnumerable<AreaEntityBase> areaList = null;
            try
            {
                if (modelId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuote>()
                            .RegisterType<ICacheManager, MemcacheManager>()
                            .RegisterType<IAreaCacheRepository, AreaCacheRepository>();

                        IAreaCacheRepository objArea = container.Resolve<IAreaCacheRepository>();
                        areaList = objArea.GetAreaList(modelId, cityId);
                        return areaList;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ModelHelper.GetAreaForModelAndCity() - modelId:{0}, cityId {1}", modelId, cityId));
            }
            return areaList;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Gets City Details by ModelId
        /// </summary>
        public IEnumerable<CityEntityBase> GetCitiesByModelId(uint modelId)
        {
            IEnumerable<CityEntityBase> cityList = null;
            if (modelId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<ICity, CityRepository>()
                                     .RegisterType<ICacheManager, MemcacheManager>()
                                     .RegisterType<ICityCacheRepository, CityCacheRepository>();
                        ICityCacheRepository objcity = container.Resolve<ICityCacheRepository>();
                        cityList = objcity.GetPriceQuoteCities(modelId);
                        return cityList;
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, string.Format("ModelHelper.GetCitiesByModelId() - modelId:{0}", modelId));
                }
            }
            return cityList;
        }
    }
}