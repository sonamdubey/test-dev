﻿using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
namespace Bikewale.Common
{
    /// <summary>
    /// Created By: Sangram Nandkhile on 23 Nov 2016
    /// Desc: Common Function to return Make related data
    /// This class will enable us to reduce code redundancy 
    /// </summary>
    public class MakeHelper
    {
        /// <summary>
        /// Created by : Sangram Nandkhile on 23 Nov 2016
        /// Description: Method to get make name by makeId.
        /// Modified by sajal Gupta on 15-05-2017
        /// Description : Call to cache layer instead of dal.
        /// </summary>
        /// <param name="cityMaskingName"></param>
        public BikeMakeEntityBase GetMakeNameByMakeId(uint makeId)
        {
            BikeMakeEntityBase objBikeMakeEntityBase = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>();

                    var makesCacheRepo = container.Resolve<IBikeMakesCacheRepository>();
                    objBikeMakeEntityBase = makesCacheRepo.GetMakeDetails(makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("MakeHelper.GetMakeNameByMakeId() - Makeid :{0}", makeId));
                objErr.SendMail();
            }
            return objBikeMakeEntityBase;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 23 Nov 2016
        /// Description: Method to get make name by makeId.
        /// </summary>
        /// <param name="cityMaskingName"></param>
        public MakeMaskingResponse GetMakeByMaskingName(string makeMaskingName)
        {
            MakeMaskingResponse objResponse = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                          .RegisterType<ICacheManager, MemcacheManager>()
                          .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                         ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository>();
                    objResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("MakeHelper.GetMakeByMaskingName() => Make:{0}", makeMaskingName));
                objErr.SendMail();
            }
            return objResponse;
        }
    }
}