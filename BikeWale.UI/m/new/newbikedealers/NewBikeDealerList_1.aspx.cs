using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Memcache;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace Bikewale.Mobile.New
{
    /// <summary>
    /// Created By : Sushil Kumar on 19th March 2016
    /// Class to show the bike dealers details
    /// </summary>
    public class NewBikeDealerList : PageBase
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, urlCityMaskingName = string.Empty;
        protected uint cityId, makeId;
        protected ushort totalDealers;
        protected Repeater rptMakes, rptCities, rptDealers;
        protected string clientIP = string.Empty, pageUrl = string.Empty;

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

            ProcessQueryString();
            GetMakeIdByMakeMaskingName(makeMaskingName);

            if (makeId > 0 && cityId > 0)
            {
                BindMakesDropdown();
                BindCitiesDropdown();

                BindDealerList();
            }

        }

        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
        /// Description : To bind dealerlist
        /// </summary>
        private void BindDealerList()
        {
            DealersEntity _dealers = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IDealer, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    _dealers = objCache.GetDealerByMakeCity(cityId, makeId);

                    if (_dealers != null && _dealers.TotalCount > 0)
                    {
                        rptDealers.DataSource = _dealers.Dealers;
                        rptDealers.DataBind();
                        totalDealers = _dealers.TotalCount;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindDealerList : ");
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
        /// Description : To bind makes list to dropdown
        /// Modified by :   Sumit Kate on 29 Mar 2016
        /// Description :   Get the makes list of BW and AB dealers
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
                    _makes = objCache.GetMakesByType(EnumBikeType.Dealer);
                    if (_makes != null && _makes.Count() > 0)
                    {
                        rptMakes.DataSource = _makes;
                        rptMakes.DataBind();
                        makeName = _makes.Where(x => x.MakeId == makeId).FirstOrDefault().MakeName;
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "BindMakesDropdown : ");
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
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

                        var _city = _cities.FirstOrDefault(x => x.CityId == cityId);
                        if (_city != null)
                        {
                            cityName = _city.CityName;
                            cityMaskingName = _city.CityMaskingName;
                        }
                        else
                        {
                            Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
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



        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
        /// Description : To get makeId from make masking name
        /// </summary>
        private void GetMakeIdByMakeMaskingName(string maskingName)
        {
            try
            {
                if (!string.IsNullOrEmpty(maskingName))
                {
                    string _makeId = MakeMapping.GetMakeId(maskingName);
                    if (string.IsNullOrEmpty(_makeId) || !uint.TryParse(_makeId, out makeId))
                    {
                        Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindCitiesDropdown ");
                objErr.SendMail();
            }
        }



        #region Private Method to process querystring
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 16th March 2016 
        /// Description : Private Method to query string fro make masking name and cityId
        /// </summary>
        private void ProcessQueryString()
        {
            var currentReq = HttpContext.Current.Request;
            try
            {
                if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                {
                    makeMaskingName = currentReq.QueryString["make"];
                    urlCityMaskingName = currentReq.QueryString["city"];
                    if (!String.IsNullOrEmpty(urlCityMaskingName) && !String.IsNullOrEmpty(makeMaskingName))
                    {
                        cityId = CitiMapping.GetCityId(urlCityMaskingName);
                        //isValidQueryString = true;
                    }
                    else
                    {
                        Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    clientIP = Bikewale.Common.CommonOpn.GetClientIP();
                    pageUrl = currentReq.ServerVariables["URL"];
                }
                else
                {
                    Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                //if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                //{
                //    makeMaskingName = currentReq.QueryString["make"];
                //    uint.TryParse(currentReq.QueryString["city"], out cityId);
                //    clientIP = Bikewale.Common.CommonOpn.GetClientIP();
                //    pageUrl = currentReq.ServerVariables["URL"];
                //}
                //else
                //{
                //    Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //    this.Page.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, " : ProcessQueryString ");
                objErr.SendMail();
            }

        }

        /// <summary>
        /// Modified By : Lucky Rathore on 30 March 2016
        /// Description : link URL changed.
        /// </summary>
        /// <param name="dealerType"></param>
        /// <param name="dealerId"></param>
        /// <param name="campId"></param>
        /// <param name="dealerName"></param>
        /// <returns></returns>
        public string GetDealerDetailLink(string dealerType, string dealerId, string campId, string dealerName)
        {
            string retString = string.Empty;
            if (dealerType == "2" || dealerType == "3")
            {
                string link = "/m/new/newbikedealers/dealerdetails.aspx/?query=" + Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(String.Format("dealerId={0}&campId={1}&cityId={2}", dealerId, campId, cityId));
                retString = String.Format("<a class=\"text-black\" href=\"{0}\">{1}</a>", link, dealerName);
            }
            else
            {
                retString = dealerName;
            }

            return retString;
        }

        #endregion
    }   // End of class
}   // End of namespace