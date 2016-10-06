using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Cache.UsedBikes;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.UsedBikes;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 Oct 2016
    /// </summary>
    public class UsedBikeLandingPage
    {
        public IEnumerable<UsedBikeMakeEntity> TopMakeList;
        public IEnumerable<UsedBikeMakeEntity> OtherMakeList;
        public IEnumerable<CityEntityBase> Cities = null;

        public UsedBikeLandingPage()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    ICityCacheRepository objCitiesCache = null;
                    container.RegisterType<IUsedBikes, Bikewale.BAL.UsedBikes.UsedBikes>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    container.RegisterType<IUsedBikesCache, UsedBikesCache>();
                    container.RegisterType<ICityCacheRepository, CityCacheRepository>();

                    IUsedBikesCache objUsedBikes = container.Resolve<IUsedBikesCache>();
                    var totalList = objUsedBikes.GetUsedBikeMakesWithCount();
                    if (totalList != null && totalList.Count() > 0)
                    {
                        TopMakeList = totalList.Take(6);
                        OtherMakeList = totalList.Skip(6);
                    }

                    objCitiesCache = container.Resolve<ICityCacheRepository>();
                    Cities = objCitiesCache.GetAllCities(EnumBikeType.Used);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.Used.UsedBikeLandingPage.constructor");
                objErr.SendMail();
            }
        }

        private void GetAllCities()
        {
            ICity _city = new CityRepository();
            try
            {
                cities = _city.GetAllCities(EnumBikeType.Used);
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Exception : GetAllCities - used-Default");
                objErr.SendMail();
            }
        }
    }
}