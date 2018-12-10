using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Service;
using Microsoft.Practices.Unity;
using MobileWeb.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Carwale.Interfaces.Campaigns;
using Carwale.BL.Campaigns;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.Interfaces;

namespace MobileWeb.Research
{
    public class DealerDetails : System.Web.UI.Page
    {
        protected Repeater rptDealerPremium, rptDealer, rptModel;
        protected List<NewCarDealer> entireList = new List<NewCarDealer>();
        protected List<CarModelSummary> _modelList;
        private ICarModels _carModelsBL;
        private ICarPriceQuoteAdapter _iPrices;
        protected string subHeading = "";
        protected NewCarDealer dealerDetails;

        protected string cityId = "-1", makeId = "", makeName = "", cityName = "";
        protected bool isEligibleForOrp = CookiesCustomers.IsEligibleForORP;
        private readonly ICarMakesCacheRepository _carMakesCacheRepo;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;

        public DealerDetails()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _carMakesCacheRepo = container.Resolve<ICarMakesCacheRepository>();
                _geoCitiesCacheRepo = container.Resolve<IGeoCitiesCacheRepository>();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["cityId"] != null && Request.QueryString["cityId"] != "" && CommonOpn.CheckId(Request.QueryString["cityId"]))
                {
                    if (Request.QueryString["makeId"] != null && Request.QueryString["makeId"] != "" && CommonOpn.CheckId(Request.QueryString["makeId"]))
                    {
                        cityId = Request.QueryString["cityId"].ToString();
                        makeId = Request.QueryString["makeId"].ToString();

                        makeName = _carMakesCacheRepo.GetCarMakeDetails(Convert.ToInt32(makeId)).MakeName;
                        try
                        {
                            IGeoCitiesCacheRepository geoCitiesCacheRepo;
                            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
                            {
                                geoCitiesCacheRepo = container.Resolve<IGeoCitiesCacheRepository>();
                            }
                            cityName = geoCitiesCacheRepo.GetCityNameById(cityId);
                        }
                        catch (Exception err)
                        {
                            Trace.Warn(err.Message);
                            ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                            objErr.SendMail();
                        }

                        LoadDealer();
                        LoadModels(makeId, cityId);
                    }
                }
            }
        }

        private void LoadModels(string makeId, string cityId)
        {
            try
            {
                IUnityContainer container = UnityBootstrapper.Resolver.GetContainer();
                _carModelsBL = container.Resolve<ICarModels>();
                _iPrices = container.Resolve<ICarPriceQuoteAdapter>();

                _modelList = _carModelsBL.GetNewModelsByMake(Convert.ToInt32(makeId));

                var modellist = _modelList.Select((item) => item.ModelId).ToList();
                var modelPrices = _iPrices.GetModelsCarPriceOverview(modellist, Convert.ToInt32(cityId));
                _modelList.ForEach((item) => { var priceOverview = modelPrices[item.ModelId]; item.CarPriceOverview = (priceOverview != null ? priceOverview : new PriceOverview()); });

                rptModel.DataSource = _modelList;
                rptModel.DataBind();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /*
         * Created Date : 25/02/2015
         * Author : Rohan 
         * Desc : To load New Car Dealers based on makeId & CityId passed
         */
        private void LoadDealer()
        {
            try
            {

                INewCarDealers newCarDealerList = UnityBootstrapper.Resolve<INewCarDealers>();

                entireList = newCarDealerList.GetDealersList(-1, Convert.ToInt32(cityId), Convert.ToInt32(makeId)).NewCarDealers;//ds.Tables[1];

                subHeading = makeName + " has " + entireList.Count + " authorized dealer outlet" + (entireList.Count > 1 ? "s" : "") + " / showroom" + (entireList.Count > 1 ? "s" : "") + " in " + cityName;

                rptDealerPremium.DataSource = entireList.FindAll(dealer => dealer.IsPremium == true);
                rptDealerPremium.DataBind();

                rptDealer.DataSource = entireList.FindAll(dealer => dealer.IsPremium == false);
                rptDealer.DataBind();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            if (entireList.Count < 1) { Response.Redirect("/m/", true); }

        }

        public string GetContactNumber(NewCarDealer dealerDetails)
        {
            try
            {
                string mobileNo = dealerDetails.MobileNo;
                string landlineNo = dealerDetails.LandlineNo;
                string landlineCode = dealerDetails.LandlineCode;

                if (!string.IsNullOrWhiteSpace(mobileNo))
                {
                    return mobileNo;
                }
                else if (!string.IsNullOrWhiteSpace(landlineNo))
                {
                    if (!string.IsNullOrWhiteSpace(landlineCode))
                    {
                        return "0" + landlineCode + "-" + landlineNo;
                    }
                    else
                    {
                        return landlineNo;
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return ConfigurationManager.AppSettings["CarwaleDefaultContact"];
        }
    }
}