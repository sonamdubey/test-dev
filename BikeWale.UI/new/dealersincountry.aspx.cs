using Bikewale.BAL.Location;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Memcache;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Bikewale.New
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 27 Jun 2016
    /// Summary: To show dealers in State and list of cities
    /// </summary>
    public class DealersInCountry : Page
    {
        protected BikeMakeEntityBase objMMV;
        protected NewLaunchedBikes_new ctrlNewLaunchedBikes;
        protected UpcomingBikes_new ctrlUpcomingBikes;
        public ushort makeId;
        public string cityArr = string.Empty, makeMaskingName = string.Empty, stateMaskingName = string.Empty, stateName = string.Empty, stateArray = string.Empty;
        public uint stateCount = 0, DealerCount = 0;
        protected uint countryCount = 0;
        public uint citiesCount = 0, stateCountDealers = 0;
        public uint cityId = 0, DealerCountCity, stateId = 0;
        public DealerLocatorList states = null;
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

            if (ProcessQS())
            {
                checkDealersForMakeCity(makeId);

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    objMMV = makesRepository.GetMakeDetails(makeId.ToString());

                }
                ctrlNewLaunchedBikes.pageSize = 6;
                ctrlNewLaunchedBikes.makeid = makeId;
                ctrlUpcomingBikes.pageSize = 6;
                ctrlUpcomingBikes.MakeId = makeId;
                BindStatesCities();

            }
        }
        private void BindStatesCities()
        {
            IState objStatesCity = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IState, States>();
                objStatesCity = container.Resolve<IState>();
                states = objStatesCity.GetDealerStatesCities(Convert.ToUInt32(makeId));
                DealerCount = states.totalDealers;
                citiesCount = states.totalCities;

            }
        }




        protected bool ProcessQS()
        {
            bool isSuccess = true;


            if (!string.IsNullOrEmpty(Request["make"]))
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



            }
            else
            {
                Trace.Warn("make id : ", Request.QueryString["make"]);
                Response.Redirect("/new/", false);
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
                                string _redirectUrl = String.Format("/{0}-dealer-showrooms-in-{1}/", makeMaskingName, _city.CityMaskingName);
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
                    Trace.Warn(ex.Message);
                    ErrorClass objErr = new ErrorClass(ex, "checkDealersForMakeCity");
                    objErr.SendMail();
                }
            }
            return false;
        }

    }   // End of class
}   // End of namespace