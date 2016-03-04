﻿using Bikewale.BAL.Dealer;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    public class BookingListing : System.Web.UI.Page
    {
        protected DropDownList bookingCitiesList, bookingAreasList;
        List<CityEntityBase> bookingCities = null;
        IEnumerable<AreaEntityBase> bookingAreas = null;
        protected uint cityId = 0, areaId = 0;
        protected string clientIP = String.Empty;
        protected Repeater rptPopularBrand, rptOtherBrands;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            CheckLocationCookie();
            GetDealerCities();

            clientIP = Bikewale.Common.CommonOpn.GetClientIP();
            BindRepeaters();
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 04 Mar 2016
        /// Bind the Repeaters
        /// </summary>
        private void BindRepeaters()
        {
            IEnumerable<Entities.BikeData.BikeMakeEntityBase> makes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    makes = objCache.GetMakesByType(EnumBikeType.New);
                    if (makes != null && makes.Count() > 0)
                    {
                        rptPopularBrand.DataSource = makes.Take(9);
                        rptPopularBrand.DataBind();

                        rptOtherBrands.DataSource = makes.Skip(9).OrderBy(m => m.MakeName);
                        rptOtherBrands.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindRepeaters");
                objErr.SendMail();
            }
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
                    container.RegisterType<IDealer, Dealer>();
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
                        {
                            GetDealerAreas();
                        }
                        else
                        {
                            Response.Redirect("/bikebooking/", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }


                    }

                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
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

                    if (bookingAreas != null && bookingAreas.Count() > 0)
                    {
                        bookingAreasList.DataSource = bookingAreas.ToList();
                        bookingAreasList.DataTextField = "AreaName";
                        bookingAreasList.DataValueField = "AreaId";
                        bookingAreasList.DataBind();
                        bookingAreasList.Items.Insert(0, " Select Area ");

                        if (areaId > 0 && bookingAreas.Any(p => p.AreaId == areaId))
                        {
                            bookingAreasList.Items.FindByValue(Convert.ToString(areaId)).Selected = true;
                        }
                        else
                        {
                            Response.Redirect("/bikebooking/", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }

                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                Response.Redirect("/bikebooking/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

        }
    }
}