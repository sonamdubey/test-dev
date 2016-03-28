using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.Dealer;
using Bikewale.BAL.Dealer;
using System.Collections.Generic;
using Bikewale.Entities.Dealer;
using System.Linq;
using Bikewale.Controls;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.DAL.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;
using Bikewale.Entities.Location;
using Bikewale.DAL.Dealer;


namespace Bikewale.New
{
    /// <summary>
    /// Created By  : Sushil Kumar 
    /// Created on : 25th March 2016
    /// Locate new bike dealers  
    /// </summary>
    public class LocateNewBikeDealers : Page
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty;
        protected uint cityId, makeId;
        protected ushort totalDealers;
        protected Repeater rptMakes, rptCities, rptDealers;
        protected string clientIP = String.Empty, pageUrl = String.Empty;


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
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();

            BindMakesDropdown();
            BindCitiesDropdown();


        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 25th March 2016
        /// Description : To bind makes list to dropdown
        /// </summary>
        private void BindMakesDropdown()
        {
            IEnumerable<BikeMakeEntityBase> _makes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    _makes = objCache.GetMakesByType(EnumBikeType.New);
                    if (_makes != null && _makes.Count() > 0)
                    {
                        rptMakes.DataSource = _makes;
                        rptMakes.DataBind();
                        if (_makes.FirstOrDefault()!=null)
                            uint.TryParse(_makes.FirstOrDefault().MakeId.ToString(),out makeId);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "BindMakesDropdown");
                objErr.SendMail();
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
                    container.RegisterType<IDealer, DealersRepository>();

                    var objCities = container.Resolve<IDealer>();
                    _cities = objCities.FetchDealerCitiesByMake(makeId);
                    if (_cities != null && _cities.Count() > 0)
                    {
                        rptCities.DataSource = _cities;
                        rptCities.DataBind();
                    }
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
                ErrorClass objErr = new ErrorClass(ex, "GetLocationCookie");
                objErr.SendMail();
            }
        }
        #endregion

    }
}