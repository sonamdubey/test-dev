using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Bikewale.New.DealerLocator
{
    /// <summary>
    /// Created By  : Sushil Kumar 
    /// Created on : 25th March 2016
    /// Locate new bike dealers  
    /// </summary>
    public class LocateNewBikeDealers : Page
    {
        protected uint cityId, makeId;
        protected ushort totalDealers;
        protected Repeater rptMakes, rptCities, rptPopularBrands, rptOtherBrands;
        protected string pageUrl = String.Empty;

        protected NewLaunchedBikes_new ctrlNewLaunchedBikes;
        protected UpcomingBikes_new ctrlUpcomingBikes;
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

            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();
            BindMakes();

            if (makeId > 0)
                BindCitiesDropdown();
            ctrlNewLaunchedBikes.pageSize = 6;
            ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            ctrlUpcomingBikes.pageSize = 6;


        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 25th March 2016
        /// Description : To bind makes list to dropdown
        /// Modified by :   Sumit Kate on 29 Mar 2016
        /// Description :   Get the makes list of BW and AB dealers
        /// </summary>
        private void BindMakes()
        {
            IEnumerable<BikeMakeEntityBase> _makes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository>();
                    _makes = objCache.GetMakesByType(EnumBikeType.Dealer);
                    if (_makes != null && _makes.Any())
                    {
                        rptMakes.DataSource = _makes;
                        rptMakes.DataBind();

                        rptPopularBrands.DataSource = _makes.Take(10);
                        rptPopularBrands.DataBind();

                        rptOtherBrands.DataSource = _makes.Skip(10).OrderBy(m => m.MakeName);
                        rptOtherBrands.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, "BindMakesDropdown");
                
            }
        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 25th March 2016
        /// Description : To bind cities list to dropdown
        /// </summary>
        private void BindCitiesDropdown()
        {
            IEnumerable<CityEntityBase> _cities = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerRepository, DealersRepository>();

                    var objCities = container.Resolve<IDealerRepository>();
                    _cities = objCities.FetchDealerCitiesByMake(makeId);
                    if (_cities != null && _cities.Any())
                    {
                        rptCities.DataSource = _cities;
                        rptCities.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, "BindCitiesDropdown");
                
            }
        }


        #region Set user location from location cookie
        /// <summary>
        /// Created By : Sushil Kumar on 25th March 2016
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
                ErrorClass.LogError(ex, "GetLocationCookie");
                
            }
        }
        #endregion

    }
}