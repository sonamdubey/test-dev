using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;


namespace Bikewale.ServiceCenter
{
    /// <summary>
    /// Created By:-Subodh Jain 16 nov 2016
    /// Submmary:- Landing Page service center
    /// </summary>
    public class Default : Page
    {
        protected uint cityId, makeId;
        protected ushort totalDealers;
        protected BikeCare ctrlBikeCare;
        protected IEnumerable<BikeMakeEntityBase> objTopMakeList;
        protected IEnumerable<BikeMakeEntityBase> objOtherMakeList;
        protected IEnumerable<BikeMakeEntityBase> objMakes;
        protected UpcomingBikes_new ctrlUpcomingBikes;
        protected NewLaunchedBikes_new ctrlNewLaunchedBikes;
        protected MostPopularBikes_new ctrlMostPopularBikes;
        protected UsedBikeModel ctrlusedBikeModel;
        protected UsedBikeInCities ctrlusedBikeInCities;
        protected string cityName = string.Empty;
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

            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();
            BindMakes();
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            cityName = currentCityArea.City;
            cityId = currentCityArea.CityId;
            BindBikesWidgets();
        }
        /// <summary>
        /// Created By:-Subodh Jain 8 nov 2016
        /// Summary:- Bind Make for service center

        /// </summary>
        private void BindMakes()
        {

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    objMakes = objCache.GetMakesByType(EnumBikeType.ServiceCenter);
                    if (objMakes != null && objMakes.Count() > 0)
                    {

                        objTopMakeList = objMakes.Take(10);
                        objOtherMakeList = objMakes.Skip(10);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "LocateServiceCenter.BindMakes");
                objErr.SendMail();
            }
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
                ctrlMostPopularBikes.PQSourceId = (int)PQSourceEnum.Desktop_ServiceCenter_DefaultPage;

                //To get Upcoming Bike List Details 
                ctrlNewLaunchedBikes.pageSize = 9;
                ctrlNewLaunchedBikes.PQSourceId = (int)PQSourceEnum.Desktop_ServiceCenter_DefaultPage;

                //To get Upcoming Bike List Details 
                ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcomingBikes.pageSize = 9;

                ctrlBikeCare.TotalRecords = 3;

                if (ctrlusedBikeModel != null)
                {

                    CityEntityBase cityDetails = null;

                    if (cityId > 0)
                    {
                        cityDetails = new CityHelper().GetCityById(cityId);
                        ctrlusedBikeModel.CityId = cityId;
                    }

                    ctrlusedBikeModel.WidgetTitle = string.Format("Second-hand Honda Bikes in {0}", cityId > 0 ? cityDetails.CityName : "India");
                    ctrlusedBikeModel.WidgetHref = string.Format("/used/bikes-in-{0}/", cityDetails != null ? cityDetails.CityMaskingName : "india");
                    ctrlusedBikeModel.TopCount = 9;
                    ctrlusedBikeModel.IsLandingPage = true;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindBikesWidgets");
                objErr.SendMail();
            }
        }
    }
}