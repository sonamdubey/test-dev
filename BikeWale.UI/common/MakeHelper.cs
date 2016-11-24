using Bikewale.Cache.BikeData;
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
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    objBikeMakeEntityBase = makesRepository.GetMakeDetails(makeId);
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
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                          .RegisterType<ICacheManager, MemcacheManager>()
                          .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                         ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    objResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("MakeHelper.GetMakeByMaskingName()", makeMaskingName));
                objErr.SendMail();
            }
            return objResponse;
        }
    }
}