using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.SEO;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BindViewModels.Webforms.Compare
{
    /// <summary>
    /// Created By : Sushil kumar on 9th Feb 2017 
    /// Description : View model to bind compare bikes landing page
    /// </summary>
    public class CompareBikes
    {
        private readonly IBikeMakesCacheRepository _objMakeCache = null;

        public bool isPageNotFound;
        public PageMetaTags PageMetas = null;
        public IEnumerable<BikeMakeEntityBase> makes = null;

        /// <summary>
        /// Created By : Sushil kumar on 9th Feb 2017 
        /// Description : Constructor to resolve unity containers and initialize model
        /// </summary>
        public CompareBikes()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakes<BikeMakeEntity, int>>()
                    .RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                    .RegisterType<ICacheManager, MemcacheManager>();

                    _objMakeCache = container.Resolve<IBikeMakesCacheRepository>();

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.CompareBikes : CompareBikes");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 9th Feb 2017 
        /// Description : To get comparisions makes whose specifications are available
        /// </summary>
        public void GetCompareBikeMakes()
        {
            try
            {
                makes = _objMakeCache.GetMakesByType(EnumBikeType.NewBikeSpecification);

                if (makes != null && makes.Any())
                {
                    SetPageMetas();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikes.GetComparisionMakes");
                isPageNotFound = true;
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 9th Feb 2017 
        /// Description : To set page meta tags 
        /// </summary>
        private void SetPageMetas()
        {
            try
            {
                PageMetas = new PageMetaTags();
                PageMetas.Title = "Compare Bikes | New Bike Comparisons in India - BikeWale";
                PageMetas.Keywords = "bike compare, compare bike, compare bikes, bike comparison, bike comparison india";
                PageMetas.Description = "Comparing bikes in India was never this easy. BikeWale presents you the easiest way of comparing bikes. Compare 2 bikes on prices, specs, features, colours and more!";
                PageMetas.CanonicalUrl = string.Format("{0}/comparebikes/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs);
                PageMetas.AlternateUrl = string.Format("{0}/m/comparebikes/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.Compare.CompareBikes.SetPageMetas");
            }
        }


    }
}