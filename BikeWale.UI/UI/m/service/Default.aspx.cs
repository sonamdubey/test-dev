using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Bikewale.Mobile.Service
{
    /// <summary>
    /// Created By  : Subodh Jain 08 nov 2016
    /// Summary :- Locate new bike dealers  
    /// </summary>
    public class LocateServiceCenter : Page
    {
        protected uint cityId, makeId;
        protected ushort totalDealers;
        protected BikeCare ctrlBikeCare;
        protected IEnumerable<BikeMakeEntityBase> TopMakeList;
        protected IEnumerable<BikeMakeEntityBase> OtherMakeList;
        protected IEnumerable<BikeMakeEntityBase> makes;
        protected IEnumerable<CityEntityBase> cities;
        protected MUpcomingBikes ctrlUpcomingBikes;
        protected MNewLaunchedBikes ctrlNewLaunchedBikes;
        protected MMostPopularBikes ctrlMostPopularBikes;
        protected UsedBikeModel ctrlusedBikeModel;
        protected UsedBikeInCities ctrlusedBikeInCities;
        protected string cityName = string.Empty;
        protected string usedBikeLink = string.Empty, usedBikeTitle = string.Empty;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindMakes();
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            cityName = currentCityArea.City;
            cityId = currentCityArea.CityId;
            BindBikesWidgets();


        }
        /// <summary>
        /// Created By : Subodh Jain  on 28th Nov 2016
        /// Description : Added new launched,upcoming and poular bikes binding 
        /// Modified by :- Subodh Jain on 20 march 2017
        /// Summary :-added model city budget used bike widget
        /// </summary>
        private void BindBikesWidgets()
        {
            try
            {
                //to get Most Popular Bikes
                ctrlMostPopularBikes.totalCount = 9;
                ctrlMostPopularBikes.PQSourceId = (int)PQSourceEnum.Mobile_ServiceCenter_DefaultPage;

                //To get Upcoming Bike List Details 
                ctrlNewLaunchedBikes.pageSize = 9;
                ctrlNewLaunchedBikes.curPageNo = null;
                ctrlNewLaunchedBikes.PQSourceId = (int)PQSourceEnum.Mobile_ServiceCenter_DefaultPage;

                //To get Upcoming Bike List Details 
                ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcomingBikes.pageSize = 9;

                CityEntityBase cityDetails = null;



                if (ctrlusedBikeModel != null)
                {
                    if (cityId > 0)
                    {
                        cityDetails = new CityHelper().GetCityById(cityId);
                        ctrlusedBikeModel.CityId = cityId;
                    }

                    ctrlusedBikeModel.TopCount = 9;
                    ctrlusedBikeModel.IsLandingPage = true;
                    usedBikeLink = string.Format("/m/used/bikes-in-{0}/", cityDetails != null ? cityDetails.CityMaskingName : "india");
                    usedBikeTitle = string.Format("Second Hand Bikes in {0}", cityId > 0 ? cityName : "India");
                    ctrlusedBikeModel.WidgetTitle = usedBikeTitle;
                    ctrlusedBikeModel.WidgetHref = usedBikeLink;

                }
                if (ctrlusedBikeInCities != null)
                {
                    ctrlusedBikeInCities.IsLandingPage = true;
                    ctrlusedBikeInCities.WidgetHref = usedBikeLink;
                    ctrlusedBikeInCities.WidgetTitle = usedBikeTitle;
                }

                ctrlBikeCare.TotalRecords = 3;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindBikesWidgets");
                
            }
        }
        /// <summary>
        /// Created By:-Subodh Jain 8 nov 2016
        /// Submmary:- Bind Make for service center
        /// </summary>
        private void BindMakes()
        {

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository>();
                    makes = objCache.GetMakesByType(EnumBikeType.ServiceCenter);
                    if (makes != null && makes.Any())
                    {

                        TopMakeList = makes.Take(6);
                        OtherMakeList = makes.Skip(6);

                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, "LocateServiceCenter.BindMakes");
                
            }
        }
    }
}