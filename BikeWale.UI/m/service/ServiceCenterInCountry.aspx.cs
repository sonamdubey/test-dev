using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.service;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using System.Web.UI;
namespace Bikewale.Mobile.Service
{
    /// <summary>

    /// Created by :Subodh Jain 7 nov 2016
    /// Summary: For Service Center Locator page in India
    /// </summary>
    public class ServiceCenterInCountry : Page
    {
        protected BikeMakeEntityBase objMMV;
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
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            if (ProcessQS())
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    objMMV = makesRepository.GetMakeDetails(makeId.ToString());

                }
                BindCities();

            }

        }
        /// <summary>
        /// Created By:-Subodh Jain 7 nov 2016
        /// Summary:- For Service center city state list
        /// </summary>
        private void BindCities()
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
                    ServiceCenterList = ObjServiceCenter.GetServiceCenterList(Convert.ToUInt32(makeId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenterInCountry.BindCities");
                objErr.SendMail();
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
                        containerInner.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                              .RegisterType<ICacheManager, MemcacheManager>()
                              .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                             ;
                        var objCache = containerInner.Resolve<IBikeMakesCacheRepository<int>>();

                        objMakeResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                    }
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                    objErr.SendMail();
                    Response.Redirect("pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
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
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }

                //verify the id as passed in the url
                if (CommonOpn.CheckId(_makeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }


            }
            else
            {
                Response.Redirect("/m/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSuccess = false;
            }
            return isSuccess;
        }



    }   // End of class
}   // End of namespace