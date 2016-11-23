using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.service;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Bikewale.ServiceCenter
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 27 Jun 2016
    /// Summary: To show dealers in State and list of cities
    /// </summary>
    public class ServiceCenterInCountry : Page
    {
        protected BikeMakeEntityBase objMMV;
        protected BikeCare ctrlBikeCare;
        public ushort makeId;
        public uint cityId;
        public string makeMaskingName = string.Empty;
        public ServiceCenterLocatorList ServiceCenterList;
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
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (ProcessQS() && checkServiceCenterForMakeCity(makeId))
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    objMMV = makesRepository.GetMakeDetails(makeId.ToString());

                }
                BindStatesCities();
                ctrlBikeCare.TotalRecords = 3;

            }
        }
        /// <summary>
        /// Created by : Subodh Jain on 16 Nov 2016
        /// Summary: To show list of State and list of cities for service center
        /// </summary>
        private void BindStatesCities()
        {
            try
            {
                IServiceCenterCacheRepository ObjServiceCenter = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>()
                    .RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                    .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                    ObjServiceCenter = container.Resolve<IServiceCenterCacheRepository>();
                    ServiceCenterList = ObjServiceCenter.GetServiceCenterList(Convert.ToUInt32(makeId));

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenterInCountry.BindCities");
                objErr.SendMail();
            }
        }



        /// <summary>
        /// Modified by :   Sumit Kate on 03 Oct 2016
        /// Description :   Handle Make masking name rename 301 redirection
        /// </summary>
        /// <returns></returns>
        protected bool ProcessQS()
        {
            bool isSuccess = true;
            MakeMaskingResponse objResponse = null;

            if (!string.IsNullOrEmpty(Request["make"]))
            {
                makeMaskingName = Request["make"];
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                              .RegisterType<ICacheManager, MemcacheManager>()
                              .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                             ;
                        var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();

                        objResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                    }

                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "ServiceCenterInCountry.ProcessQS");
                    objErr.SendMail();
                    isSuccess = false;
                }
                finally
                {
                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            makeId = Convert.ToUInt16(objResponse.MakeId);
                            isSuccess = true;
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                            isSuccess = false;
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                            isSuccess = false;
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                        isSuccess = false;
                    }
                }
            }
            else
            {
                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 31st March 2016
        /// Description : To redirect user to dealer listing page if make and city already provided by user
        /// </summary>
        /// <param name="_makeId"></param>
        private bool checkServiceCenterForMakeCity(ushort _makeId)
        {
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            cityId = currentCityArea.CityId;
            if (cityId > 0)
            {
                IEnumerable<CityEntityBase> _cities = null;
                try
                {
                    IServiceCenterCacheRepository ObjServiceCenter = null;
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>()
                     .RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                     .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                     .RegisterType<ICacheManager, MemcacheManager>();
                        ObjServiceCenter = container.Resolve<IServiceCenterCacheRepository>();
                        _cities = ObjServiceCenter.GetServiceCenterCities(_makeId);
                        if (_cities != null && _cities.Count() > 0)
                        {
                            var _city = _cities.FirstOrDefault(x => x.CityId == cityId);
                            if (_city != null)
                            {
                                string _redirectUrl = String.Format("/{0}-service-center-in-{1}/", makeMaskingName, _city.CityMaskingName);
                                Response.Redirect(_redirectUrl, false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                                this.Page.Visible = false;
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "ServiceCenterInCountry.checkDealersForMakeCity");
                    objErr.SendMail();
                }
            }
            return true;
        }

    }   // End of class
}   // End of namespace
