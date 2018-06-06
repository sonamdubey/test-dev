using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.service;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
namespace Bikewale.Mobile.Service
{
    /// <summary>

    /// Created by :Subodh Jain 7 nov 2016
    /// Summary: For Service Center Locator page in India
    /// Modified By : Aditi Srivastava on 16 Dec 2016
    /// Summary     : Added widget for service centers by brand
    /// </summary>
    public class ServiceCenterInCountry : Page
    {
        protected BikeMakeEntityBase objMMV;
        protected BikeCare ctrlBikeCare;
        protected ServiceCentersByBrand ctrlOtherServiceCenters;
        protected UsedBikeModel ctrlusedBikeModel;
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
            if (ProcessQS() && checkServiceCenterForMakeCity(makeId))
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    objMMV = makesRepository.GetMakeDetails(makeId);

                }
                BindCities();
                BindWidget();
            }

        }
        /// <summary>
        /// Created By :- Subodh Jain 15 March 2017
        /// Summary :- Bind widget funtion
        /// </summary>
        private void BindWidget()
        {
            try
            {
                ctrlBikeCare.TotalRecords = 3;
                ctrlOtherServiceCenters.makeId = makeId;
                if (ctrlusedBikeModel != null)
                {
                    CityEntityBase _cityDetails = null;
                    if (cityId > 0)
                        _cityDetails = new CityHelper().GetCityById(cityId);
                    ctrlusedBikeModel.MakeId = makeId;

                    ctrlusedBikeModel.CityId = cityId;
                    ctrlusedBikeModel.WidgetTitle = string.Format("Second Hand Bikes in {0}", cityId > 0 ? _cityDetails.CityName : "India");
                    ctrlusedBikeModel.header = string.Format("Used {0} bikes in {1}", objMMV.MakeName, cityId > 0 ? _cityDetails.CityName : "India");
                    ctrlusedBikeModel.WidgetHref = string.Format("/m/used/{0}-bikes-in-{1}/", objMMV.MaskingName, cityId > 0 ? _cityDetails.CityMaskingName : "india");
                    ctrlusedBikeModel.TopCount = 9;
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCenterInCountry.BindWidget");
            }
        }
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
                        if (_cities != null && _cities.Any())
                        {
                            var _city = _cities.FirstOrDefault(x => x.CityId == cityId);
                            if (_city != null)
                            {
                                string _redirectUrl = String.Format("/m/service-centers/{0}/{1}/", makeMaskingName, _city.CityMaskingName);
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
                    ErrorClass.LogError(ex, "ServiceCenterInCountry.checkDealersForMakeCity");
                }
            }
            return true;
        }
        /// <summary>
        /// Created By:-Subodh Jain 7 nov 2016
        /// Summary:- For Service center city state list
        /// </summary>
        private void BindCities()
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
                ErrorClass.LogError(ex, "ServiceCenterInCountry.BindCities");
            }
        }

        protected bool ProcessQS()
        {
            bool isSuccess = true;
            if (!string.IsNullOrEmpty(Request["make"]))
            {
                makeMaskingName = Request["make"].ToString();
                string _makeId = string.Empty;

                MakeMaskingResponse objMakeResponse = null;

                try
                {
                    using (IUnityContainer containerInner = new UnityContainer())
                    {
                        containerInner.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                              .RegisterType<ICacheManager, MemcacheManager>()
                              .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                             ;
                        var objCache = containerInner.Resolve<IBikeMakesCacheRepository>();

                        objMakeResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "ParseQueryString");

                    UrlRewrite.Return404();
                }
                finally
                {
                    if (objMakeResponse != null)
                    {
                        if (objMakeResponse.StatusCode == 200)
                        {
                            _makeId = Convert.ToString(objMakeResponse.MakeId);
                            makeId = Convert.ToUInt16(_makeId);

                        }
                        else if (objMakeResponse.StatusCode == 301)
                        {
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objMakeResponse.MaskingName));
                        }
                        else
                        {
                            UrlRewrite.Return404();
                        }
                    }
                    else
                    {
                        UrlRewrite.Return404();
                    }
                }

                //verify the id as passed in the url
                if (CommonOpn.CheckId(_makeId) == false)
                {
                    UrlRewrite.Return404();
                    isSuccess = false;
                }


            }
            else
            {
                Response.Redirect("/m/new-bikes-in-india/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSuccess = false;
            }
            return isSuccess;
        }



    }   // End of class
}   // End of namespace