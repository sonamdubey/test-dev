using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.Dealer;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.bikebooking
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 09th Febrauary 2016
    /// Summary  : Use to Provide funtionality for Booling Listing Page.
    /// </summary>
    public partial class BookingListing : System.Web.UI.Page
    {
        protected Repeater rptCities, rptAreas;
        protected List<CityEntityBase> bookingCities = null;
        protected IEnumerable<AreaEntityBase> bookingAreas = null;
        protected uint cityId = 0, areaId = 0;
        protected string clientIP = String.Empty;
        protected Repeater rptPopularBrand, rptOtherBrands;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckLocationCookie();
                GetDealerCities();
            }
            clientIP = Bikewale.Common.CommonOpn.GetClientIP();
            BindRepeaters();
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 04 Mar 2016
        /// Bind the Brands Repeaters
        /// </summary>
        private void BindRepeaters()
        {
            IEnumerable<Entities.BikeData.BikeMakeEntityBase> makes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository>();
                    makes = objCache.GetMakesByType(EnumBikeType.New);
                    if (makes != null && makes.Any())
                    {
                        rptPopularBrand.DataSource = makes.Where(m => m.PopularityIndex > 0);
                        rptPopularBrand.DataBind();

                        rptOtherBrands.DataSource = makes.Where(m => m.PopularityIndex == 0);
                        rptOtherBrands.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                Bikewale.Notifications.ErrorClass.LogError(ex, "BindRepeaters");
                
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar 
        /// Created On : 3rd Febrauary 2016
        /// Summary  :  To fetch dealer cities and if city already selected then get areas for the same 
        /// modified By : Lucky Rathore
        /// Created On : 09 feb 2016
        /// Summary : Change drop down list to repeater for mobile site.
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
                        rptCities.DataSource = bookingCities;
                        rptCities.DataBind();

                        if (cityId > 0 && bookingCities.Any(p => p.CityId == cityId))
                        {
                            GetDealerAreas();
                        }
                        else
                        {
                            Response.Redirect("/m/bikebooking/", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }

                    }

                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Bikewale.Notifications.ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar 
        /// Created On : 3rd Febrauary 2016
        /// Summary  :  To fetch dealer areas and if city already selected and autoselect area from cookie 
        /// Modified By : Lucky Rathore
        /// Created On : 09 feb 2016
        /// Summary : Change drop down list to repeater for mobile site.
        /// </summary>
        private void GetDealerAreas()
        {
            //bookingCitiesList.Items.FindByValue(Convert.ToString(cityId)).Selected = true;

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
                        rptAreas.DataSource = bookingAreas;
                        rptAreas.DataBind();

                        if (!(areaId > 0 && bookingAreas.Any(p => p.AreaId == areaId)))
                        {
                            Response.Redirect("/m/bikebooking/", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Bikewale.Notifications.ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
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
            bool isValid = false;
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
                        isValid = true;
                    }
                }
            }

            if (!isValid)
            {
                Response.Redirect("/m/bikebooking/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

        }
    }
}