using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.service;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Bikewale.Mobile.Service
{
    /// <summary>
    /// Created By  : Sushil Kumar 
    /// Created on : 27th March 2016
    /// Locate new bike dealers  
    /// </summary>
    public class LocateServiceCenter : Page
    {
        protected uint cityId, makeId;
        protected ushort totalDealers;
        public IEnumerable<BikeMakeEntityBase> TopMakeList;
        public IEnumerable<BikeMakeEntityBase> OtherMakeList;
        public IEnumerable<BikeMakeEntityBase> makes;
        public IEnumerable<CityEntityBase> cities;
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
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            BindMakes();

            if (makeId > 0)
                BindCitiesDropdown();


        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 27th March 2016
        /// Description : To bind makes list to dropdown
        /// Modified by :   Sumit Kate on 29 Mar 2016
        /// Description :   Get the makes list of BW and AB dealers
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
                ErrorClass objErr = new ErrorClass(ex, "BindMakes");
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 27th March 2016
        /// Description : To bind cities list to dropdown
        /// </summary>
        private void BindCitiesDropdown()
        {

            try
            {
                IServiceCenter ObjServiceCenter = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>()
                    .RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                    .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                    ObjServiceCenter = container.Resolve<IServiceCenter>();
                    cities = ObjServiceCenter.GetServiceCenterCities(Convert.ToUInt32(makeId));
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "BindCitiesDropdown");
                objErr.SendMail();
            }
        }


        #region Set user location from location cookie
        /// <summary>
        /// Created By : Sushil Kumar on 27th March 2016
        /// Description : To set user location
        /// </summary>
        /// <returns></returns>
        private void GetLocationCookie()
        {
            string location = String.Empty;
            try
            {
                if (this.Context.Request.Cookies.AllKeys.Contains("location") && !string.IsNullOrEmpty(this.Context.Request.Cookies["location"].Value) && this.Context.Request.Cookies["location"].Value != "0")
                {
                    location = this.Context.Request.Cookies["location"].Value;
                    string[] arr = System.Text.RegularExpressions.Regex.Split(location, "_");

                    if (arr.Length > 0)
                    {
                        uint.TryParse(arr[0], out cityId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetLocationCookie");
                objErr.SendMail();
            }
        }
        #endregion

    }
}