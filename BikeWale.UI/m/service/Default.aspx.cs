﻿using Bikewale.Cache.BikeData;
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
        protected MUpcomingBikes mctrlUpcomingBikes;
        protected MNewLaunchedBikes mctrlNewLaunchedBikes;
        protected MMostPopularBikes mctrlMostPopularBikes;
        protected PopularUsedBikes ctrlPopularUsedBikes;

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
            BindBikesWidgets();
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            cityId = currentCityArea.CityId;
        }
        /// <summary>
        /// Created By : Subodh Jain  on 28th Nov 2016
        /// Description : Added new launched,upcoming and poular bikes binding 
        /// </summary>
        private void BindBikesWidgets()
        {
            try
            {
                //to get Most Popular Bikes
                mctrlMostPopularBikes.totalCount = 9;
                mctrlMostPopularBikes.PQSourceId = (int)PQSourceEnum.Mobile_ServiceCenter_DefaultPage;

                //To get Upcoming Bike List Details 
                mctrlNewLaunchedBikes.pageSize = 9;
                mctrlNewLaunchedBikes.curPageNo = null;
                mctrlNewLaunchedBikes.PQSourceId = (int)PQSourceEnum.Mobile_ServiceCenter_DefaultPage;

                //To get Upcoming Bike List Details 
                mctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                mctrlUpcomingBikes.pageSize = 9;

                ctrlPopularUsedBikes.PQSourceId = (int)PQSourceEnum.Mobile_ServiceCenter_DefaultPage;
                ctrlPopularUsedBikes.TotalRecords = 9;

                ctrlBikeCare.TotalRecords = 3;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindBikesWidgets");
                objErr.SendMail();
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
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    makes = objCache.GetMakesByType(EnumBikeType.ServiceCenter);
                    if (makes != null && makes.Count() > 0)
                    {

                        TopMakeList = makes.Take(6);
                        OtherMakeList = makes.Skip(6);

                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "LocateServiceCenter.BindMakes");
                objErr.SendMail();
            }
        }
    }
}