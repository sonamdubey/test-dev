using Bikewale.BAL.Location;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
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
    public class DealerInCountry : Page
    {
        protected Repeater rptCity;
        protected DataList dlCity;

        protected DataSet dsStateCity = null;
        protected BikeMakeEntityBase objMMV;
        protected MNewLaunchedBikes ctrlNewLaunchedBikes;
        protected MUpcomingBikes ctrlUpcomingBikes;
        public ushort makeId;
        public string cityArr = string.Empty, makeMaskingName = string.Empty, stateMaskingName = string.Empty, stateName = string.Empty, stateArray = string.Empty;
        public int stateCount = 0, DealerCount = 0; protected uint countryCount = 0;
        public int citiesCount = 0, stateCountDealers = 0;
        public uint cityId = 0, DealerCountCity, stateId = 0;
        public DealerStateCities dealerCity;
        public List<StateCityEntity> stateList = null;

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
            IEnumerable<DealerListIndia> states = null;
            IState objStatesCity = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IState, States>();
                objStatesCity = container.Resolve<IState>();
                states = objStatesCity.GetDealerStatesCities(Convert.ToUInt32(makeId));

                if (objMMV != null && states != null && states.Count() > 0)
                {
                    stateList = new List<StateCityEntity>();
                    var uniqueStates = from st in states
                                       group st by new { st.stateId }
                                           into mygroup
                                           select mygroup.FirstOrDefault();
                    StateCityEntity newState = null;

                    foreach (var state in uniqueStates)
                    {
                        newState = new StateCityEntity()
                        {
                            Id = state.stateId,
                            Name = state.stateName,
                            Lat = state.stateLattitude,
                            Long = state.stateLongitude,
                            stateMaskingName = state.stateMaskingName

                        };

                        var cityList = (from st in states
                                        where st.stateId == state.stateId
                                        select st).ToList();

                        DealerCityEntity newCity = null;
                        newState.Cities = new List<DealerCityEntity>();
                        foreach (var c in cityList)
                        {
                            newCity = new DealerCityEntity()
                            {
                                CityId = c.cityid,
                                CityName = c.cityName,
                                CityMaskingName = c.cityMaskingName,
                                Lattitude = c.cityLattitude,
                                Longitude = c.cityLongitude,
                                DealersCount = (uint)c.dealerCountCity
                            };
                            stateCountDealers += c.dealerCountCity;
                            citiesCount++;
                            newState.Cities.Add(newCity);
                        }
                        newState.DealerCountState = (uint)stateCountDealers;
                        DealerCount += (int)newState.DealerCountState;
                        stateCountDealers = 0;
                        stateList.Add(newState);

                    }


                }
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