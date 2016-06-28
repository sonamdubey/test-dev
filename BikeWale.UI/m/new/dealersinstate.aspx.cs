using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Common;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Memcache;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.New
{
    /// <summary>
    /// Created by : Vivek Gupta on 28th jun 2016
    /// Summary: To show dealers in State and list of cities
    /// </summary>
    public class DealersInState : Page
    {
        protected Repeater rptCity;
        protected DataList dlCity;

        protected DataSet dsStateCity = null;
        protected MakeModelVersion objMMV;

        public ushort makeId;
        public string cityArr = string.Empty, makeMaskingName = string.Empty, stateMaskingName = string.Empty, stateName = string.Empty;
        public int citesCount = 0;
        public uint cityId = 0, DealerCount = 0, stateId = 0;
        public DealerStateCities dealerCity;

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
            if (ProcessQS())
            {
                checkDealersForMakeCity(makeId);
                objMMV = new MakeModelVersion();
                objMMV.GetMakeDetails(makeId.ToString());
                BindCities();
            }
        }


        private void BindCities()
        {
            ICity objCities = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICity, CityRepository>();
                objCities = container.Resolve<ICity>();
                dealerCity = objCities.GetDealerStateCities(makeId, stateId);
                if (dealerCity != null && dealerCity.dealerCities != null && dealerCity.dealerCities.Count() > 0)
                {
                    rptCity.DataSource = dealerCity.dealerCities;
                    rptCity.DataBind();
                    cityArr = Newtonsoft.Json.JsonConvert.SerializeObject(dealerCity.dealerCities);
                    cityArr = cityArr.Replace("cityId", "id").Replace("cityName", "name");
                    DealerCount = dealerCity.dealerCities.Select(o => o.DealersCount).Aggregate((x, y) => x + y);
                    citesCount = dealerCity.dealerCities.Count();
                    stateName = dealerCity.dealerStates.StateName;
                }
            }
        }

        protected bool ProcessQS()
        {
            bool isSuccess = true;
            if (!string.IsNullOrEmpty(Request["make"]) && !string.IsNullOrEmpty(Request["state"]))
            {
                makeMaskingName = Request["make"].ToString();
                string _makeId = MakeMapping.GetMakeId(makeMaskingName);
                makeId = Convert.ToUInt16(_makeId);
                //verify the id as passed in the url
                if (CommonOpn.CheckId(_makeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }
                stateMaskingName = Request.QueryString["state"];
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IState, StateRepository>()
                             .RegisterType<IStateCacheRepository, StateCacheRepository>()
                            ;
                    var objCache = container.Resolve<IStateCacheRepository>();
                    var objResponse = objCache.GetStateMaskingResponse(stateMaskingName);
                    if (objResponse != null)
                    {
                        stateId = objResponse.StateId;
                    }
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

        /// <summary>
        /// Created By : Sushil Kumar bon 31st March 2016
        /// Description : To redirect user to dealer listing page if make and city already provided by user
        /// </summary>
        /// <param name="_makeId"></param>
        private bool checkDealersForMakeCity(ushort _makeId)
        {
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            cityId = currentCityArea.CityId;
            if (cityId > 0)
            {
                IEnumerable<CityEntityBase> _cities = null;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealer, DealersRepository>();
                        var objCities = container.Resolve<IDealer>();
                        _cities = objCities.FetchDealerCitiesByMake(_makeId);
                        if (_cities != null && _cities.Count() > 0)
                        {
                            var _city = _cities.FirstOrDefault(x => x.CityId == cityId);
                            if (_city != null)
                            {
                                string _redirectUrl = String.Format("/m/{0}-bikes/dealers-in-{1}/", makeMaskingName, _city.CityMaskingName);
                                Response.Redirect(_redirectUrl, false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                                this.Page.Visible = false;
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "checkDealersForMakeCity");
                    objErr.SendMail();
                }
            }
            return false;
        }

    }   // End of class
}   // End of namespace