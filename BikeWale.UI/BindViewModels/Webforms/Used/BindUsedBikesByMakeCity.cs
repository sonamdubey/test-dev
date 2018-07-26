
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.DAL.Location;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
namespace Bikewale.BindViewModels.Webforms.Used
{
    /// <summary>
    /// Created by : Subodh Jain 29 Dec 2016
    /// Summary: Bind used bikes by make in a city with count.
    /// </summary>
    public class BindUsedBikesByMakeCity
    {
        private ICityCacheRepository objCitiesCache = null;
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public string title = string.Empty, description = string.Empty, canonical = string.Empty, keywords = string.Empty, alternative = string.Empty;
        public BindUsedBikesByMakeCity()
        {

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICityCacheRepository, CityCacheRepository>()
                    .RegisterType<ICity, CityRepository>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                    objCitiesCache = container.Resolve<ICityCacheRepository>();

                }

            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("BindUsedBikesByMakeCity.BindUsedBikesByMakeCity"));
            }

        }
        /// <summary>
        /// Created by : Subodh Jain 29 Dec 2016
        /// Summary: Get Used bikes by make in cities
        /// </summary>
        public IEnumerable<UsedBikeCities> GetUsedBikeByMakeCityWithCount(uint makeid)
        {
            IEnumerable<UsedBikeCities> objBikeCity = null;
            try
            {
                objBikeCity = objCitiesCache.GetUsedBikeByMakeCityWithCount(makeid);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BindUsedBikesByMakeCity.GetUsedBikeByMakeCityWithCount_{0}", makeid));
            }
            return objBikeCity;
        }
        /// <summary>
        /// Created by : Subodh Jain 29 Dec 2016
        /// Summary:Create Metas
        /// Modified by: Dhruv Joshi
        /// Dated: 19th July 2018
        /// Description: Correcting Meta URLs
        /// </summary>
        public void CreateMetas()
        {
            try
            {
                title = string.Format("Browse used {0} bikes by cities", MakeName);
                description = string.Format("Browse used {0} bikes by cities in India", MakeName);
                canonical = string.Format("https://www.bikewale.com/used/browse-{0}-bikes-in-cities/", MakeMaskingName);
                keywords = "city wise used bikes listing,used bikes for sale, second hand bikes, buy used bike";
                alternative = string.Format("https://www.bikewale.com/m/used/browse-{0}-bikes-in-cities/", MakeMaskingName);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindUsedBikesByMakeCity.CreateMetas");
            }

        }

    }
}