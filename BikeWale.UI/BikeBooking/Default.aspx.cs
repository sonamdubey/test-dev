using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.Dealer;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Common;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Carwale.DAL.Dealers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    /// <summary>
    /// Modified by :   Sumit Kate on 22 Apr 2016
    /// Description :   Removed User testimonial
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected DropDownList bookingCitiesList, bookingAreasList;
        List<CityEntityBase> bookingCities = null;
        IEnumerable<AreaEntityBase> bookingAreas = null;
        protected uint cityId = 0, areaId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 22 Apr 2016
        /// Description :   Removed User testimonial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            CheckLocationCookie();
            GetDealerCities();
        }


        /// <summary>
        /// Created By : Sushil Kumar 
        /// Created On : 3rd Febrauary 2016
        /// Summary  :  To fetch dealer cities and if city already selected then get areas for the same 
        /// </summary>
        private void GetDealerCities()
        {
            try
            {
                bookingCities = new List<CityEntityBase>();
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealer, Dealer>()
                        .RegisterType<IDealerRepository, DealersRepository>()
                        .RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                        .RegisterType<IApiGatewayCaller, ApiGatewayCaller>()
                        .RegisterType<ICacheManager, MemcacheManager>();
                    IDealer _objDealerPricequote = container.Resolve<IDealer>();

                    bookingCities = _objDealerPricequote.GetDealersBookingCitiesList();

                    if (bookingCities != null && bookingCities.Count > 0)
                    {
                        bookingCitiesList.DataSource = bookingCities;
                        bookingCitiesList.DataTextField = "CityName";
                        bookingCitiesList.DataValueField = "CityId";
                        bookingCitiesList.DataBind();
                        bookingCitiesList.Items.Insert(0, " Select City ");

                        if (cityId > 0 && bookingCities.Any(p => p.CityId == cityId))
                            GetDealerAreas();

                    }

                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar 
        /// Created On : 3rd Febrauary 2016
        /// Summary  :  To fetch dealer areas and if city already selected and autoselect area from cookie 
        /// </summary>
        private void GetDealerAreas()
        {
            bookingCitiesList.Items.FindByValue(Convert.ToString(cityId)).Selected = true;

            try
            {
                bookingAreas = new List<AreaEntityBase>();
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArea, AreaRepository>();
                    IArea _areaRepo = container.Resolve<IArea>();
                    bookingAreas = _areaRepo.GetAreasByCity(Convert.ToUInt16(cityId));

                    if (bookingAreas != null && bookingAreas.Any())
                    {
                        bookingAreasList.DataSource = bookingAreas.ToList();
                        bookingAreasList.DataTextField = "AreaName";
                        bookingAreasList.DataValueField = "AreaId";
                        bookingAreasList.DataBind();
                        bookingAreasList.Items.Insert(0, " Select Area ");

                        if (areaId > 0 && bookingAreas.Any(p => p.AreaId == areaId))
                            bookingAreasList.Items.FindByValue(Convert.ToString(areaId)).Selected = true;
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar 
        /// Created On : 3rd Febrauary 2016
        /// Read current cookie values
        /// Check if there are areas for current model and City
        /// If No then drop area cookie
        /// </summary>
        private void CheckLocationCookie()
        {
            string location = String.Empty;
            var cookies = this.Context.Request.Cookies;

            if (cookies.AllKeys.Contains("location"))
            {
                location = cookies["location"].Value;
                if (!String.IsNullOrEmpty(location) && location.IndexOf('_') != -1)
                {
                    string[] locArray = location.Split('_');
                    if (locArray.Length > 0)
                    {
                        UInt32.TryParse(locArray[0], out cityId);
                    }

                    if (locArray.Length > 3 && cityId != 0)
                    {
                        UInt32.TryParse(locArray[2], out areaId);
                    }
                }
            }
        }

    }
}