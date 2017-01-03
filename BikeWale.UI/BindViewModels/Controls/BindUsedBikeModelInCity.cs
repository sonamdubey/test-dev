using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Subodh Jain on 2 jan 2017 
    /// Description : Bind Used bikes Widget View Model
    /// </summary>
    public class BindUsedBikeModelInCity
    {
        private IUsedBikeDetailsCacheRepository objCache = null;

        public BindUsedBikeModelInCity()
        {
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                        .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>();

                    objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();

                }
            }
            catch (System.Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindUsedBikeModelInCity.BindUsedBikeModelInCity");
            }
        }
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Get Used Bike By Model Count In City with make
        /// </summary>
        public IEnumerable<MostRecentBikes> GetUsedBikeByModelCountInCity(uint makeid, uint cityid, uint topcount)
        {
            IEnumerable<MostRecentBikes> objBikeCity = null;
            try
            {
                objBikeCity = objCache.GetUsedBikeByModelCountInCity(makeid, cityid, topcount);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BindUsedBikeModelInCity.GetUsedBikeByModelCountInCity_{0}_{1}_{2}", makeid, cityid, topcount));
            }
            return objBikeCity;

        }
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Get Used Bike By Model Count In City
        /// </summary>
        public IEnumerable<MostRecentBikes> GetUsedBikeCountInCity(uint cityid, uint topcount)
        {
            IEnumerable<MostRecentBikes> objBikeCity = null;
            try
            {
                objBikeCity = objCache.GetUsedBikeCountInCity(cityid, topcount);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BindUsedBikeModelInCity.GetUsedBikeCountInCity_{0}", cityid));
            }
            return objBikeCity;

        }
    }
}