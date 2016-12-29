using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// Created by : Subodh Jain 29 Dec 2016
    /// Summary: Get Used bikes by make in cities
    /// </summary>
    public class MakesInCity : System.Web.UI.Page
    {
        protected IEnumerable<UsedBikeCities> objBikeCityCountTop = null;
        protected IEnumerable<UsedBikeCities> objBikeCityCount = null;
        protected BikeMakeEntityBase MakeDetails;
        protected uint makeId;
        protected string pgTitle = string.Empty, pgDescription = string.Empty, pgCanonical = string.Empty, pgKeywords = string.Empty, makeMaskingName = string.Empty, pgAlternative = string.Empty;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ProcessQueryString();
            BindCities();
        }
        /// <summary>
        /// Created by : Subodh Jain 29 Dec 2016
        /// Summary: for processing querystring
        /// </summary>
        bool ProcessQueryString()
        {
            bool isSucess = true;

            if (!String.IsNullOrEmpty(Request.QueryString["make"]))
            {
                makeMaskingName = Request.QueryString["make"];

                if (!String.IsNullOrEmpty(makeMaskingName))
                {
                    MakeMaskingResponse objResponse = null;

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
                            if (objResponse != null && objResponse.MakeId != null)
                                MakeDetails = objCache.GetMakeDetails(objResponse.MakeId);
                        }
                    }
                    catch (Exception ex)
                    {
                        isSucess = false;
                        Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                        objErr.SendMail();
                        Response.Redirect("/new/", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    finally
                    {
                        if (objResponse != null)
                        {
                            if (objResponse.StatusCode == 200)
                            {
                                makeId = objResponse.MakeId;
                            }
                            else if (objResponse.StatusCode == 301)
                            {
                                CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                            }
                            else
                            {
                                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                                this.Page.Visible = false;
                                isSucess = false;
                            }
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                            isSucess = false;
                        }
                    }
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSucess = false;
                }
            }
            else
            {
                //invalid make id, hence redirect to the new default page
                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSucess = false;
            }

            return isSucess;
        }
        /// <summary>
        /// Created By Subodh Jain on 29 dec 2016
        /// Desc : Bind cities on City page accoding to make
        /// </summary>
        private void BindCities()
        {
            try
            {
                BindUsedBikesByMakeCity objBikeCity = new BindUsedBikesByMakeCity();
                objBikeCity.MakeName = MakeDetails.MakeName;
                objBikeCityCount = objBikeCity.GetUsedBikeByMakeCityWithCount(makeId);
                objBikeCity.CreateMetas();
                objBikeCityCountTop = objBikeCityCount.Where(x => x.priority > 0); ;
                objBikeCityCount = objBikeCityCount.OrderBy(c => c.CityName);
                pgKeywords = objBikeCity.keywords;
                pgTitle = objBikeCity.title;
                pgDescription = objBikeCity.description;
                pgCanonical = objBikeCity.canonical;
                pgAlternative = objBikeCity.alternative;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "makeincity.BindCities");
                objErr.SendMail();
            }
        }
    }
}