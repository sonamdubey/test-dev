using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.Dealer;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bikewale.Mobile.bikebooking
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Summary :  Booking Landing Page
    /// Modified by :   Sumit Kate on 22 Apr 2016
    /// Description :   Removed User testimonial
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        List<CityEntityBase> bookingCities = null;
        IEnumerable<AreaEntityBase> bookingAreas = null;
        protected uint cityId = 0, areaId = 0;
        protected StringBuilder cityListData = new System.Text.StringBuilder(), areaListData = new System.Text.StringBuilder();

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
                        bool citySelected = false;
                        foreach (var city in bookingCities)
                        {
                            if (cityId != city.CityId)
                                cityListData.AppendFormat("<li cityId='{0}'>{1}</li>", city.CityId, city.CityName);
                            else
                            {
                                cityListData.AppendFormat("<li class='activeCity' cityId='{0}'>{1}</li>", city.CityId, city.CityName);
                                citySelected = true;
                            }
                        }

                        if (citySelected)
                        {
                            GetDealerAreas();
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
        /// </summary>
        private void GetDealerAreas()
        {
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
                        foreach (var area in bookingAreas)
                        {
                            if (areaId != area.AreaId)
                                areaListData.AppendFormat("<li areaId='{0}'>{1}</li>", area.AreaId, area.AreaName);
                            else
                            {
                                areaListData.AppendFormat("<li class='activeArea' areaId='{0}'>{1}</li>", area.AreaId, area.AreaName);
                            }
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

    }   // class
}   // namespace