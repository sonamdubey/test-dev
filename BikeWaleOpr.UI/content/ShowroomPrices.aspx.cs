using BikewaleOpr.BAL.BikePricing;
using BikewaleOpr.Cache;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.DALs.BikePricing;
using BikewaleOpr.Entities.BikePricing;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Interface.Dealers;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BikewaleOpr.Cache.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;
using BikewaleOpr.Interface.Amp;
using BikewaleOpr.BAL.Amp;

namespace BikeWaleOpr.Content
{
    public class ShowroomPrices : Page
    {
        protected Button btnSearch, btnSavePrices;
        protected DropDownList ddlMakes, ddlStates, ddlCities, ddlPriceCities, ddlPriceStates;
        protected HiddenField hdnSelectedCity, hdnSelectedCities;
        protected Repeater rptPrices;
        protected string qryStrVersion;
        private string modelIds, seriesIds;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSearch.Click += new EventHandler(ShowPrices);
            btnSavePrices.Click += new EventHandler(SaveBikePrices);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMakes();
                BindStates();
                if (Request.QueryString["state"] != null && Request.QueryString["city"] != null && Request.QueryString["make"] != null)
                {
                    ddlMakes.SelectedValue = Request.QueryString["make"].ToString();
                    ddlStates.SelectedValue = Request.QueryString["state"].ToString();
                    hdnSelectedCity.Value = Request.QueryString["city"].ToString();
                    ShowBikePrices();
                }
                else
                {
                    hdnSelectedCity.Value = "0";
                    ddlCities.Enabled = false;
                    ddlPriceCities.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Function to bind the makes drop down list
        /// </summary>
        private void BindMakes()
        {
            MakeModelVersion mmv = new MakeModelVersion();
            DataTable dt = mmv.GetMakes("NEW");

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlMakes.DataSource = dt;
                ddlMakes.DataTextField = "Text";
                ddlMakes.DataValueField = "Value";
                ddlMakes.DataBind();

                ddlMakes.Items.Insert(0, new ListItem("--Select Make--", "0"));
            }
        } // Page_Load        

        /// <summary>
        /// Function to bind the states drop down list
        /// </summary>
        private void BindStates()
        {
            try
            {
                ManageStates objMS = new ManageStates();
                DataTable dt = objMS.FillStates();
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlStates.DataSource = dt;
                    ddlStates.DataTextField = "Text";
                    ddlStates.DataValueField = "Value";
                    ddlStates.DataBind();

                    ddlStates.Items.Insert(0, new ListItem("--Select State--", "0"));

                    ddlPriceStates.DataSource = dt;
                    ddlPriceStates.DataTextField = "Text";
                    ddlPriceStates.DataValueField = "Value";
                    ddlPriceStates.DataBind();

                    ddlPriceStates.Items.Insert(0, new ListItem("--Select State--", "0"));
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("objMS.FillStates  ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);

            }
        }

        /// <summary>
        /// Function to bind the cities drop down list
        /// </summary>
        private void BindCities()
        {
            try
            {
                ManageCities objMC = new ManageCities();
                DataSet ds = objMC.GetCities(Convert.ToInt32(ddlStates.SelectedValue), "7");

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ddlCities.DataSource = ds.Tables[0];
                    ddlCities.DataTextField = "Text";
                    ddlCities.DataValueField = "Value";
                    ddlCities.DataBind();
                    ddlCities.Items.Insert(0, new ListItem("--Select City--", "0"));

                    ddlPriceCities.DataSource = ds.Tables[0];
                    ddlPriceCities.DataTextField = "Text";
                    ddlPriceCities.DataValueField = "Value";
                    ddlPriceCities.DataBind();
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);

            }
        }

        /// <summary>
        /// Function to bind the make pricing for given cities with repeater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPrices(object sender, EventArgs e)
        {
            ShowBikePrices();
        }

        private void ShowBikePrices()
        {
            uint makeId = !String.IsNullOrEmpty(ddlMakes.SelectedValue) ? Convert.ToUInt32(ddlMakes.SelectedValue) : 0;
            uint cityId = !String.IsNullOrEmpty(hdnSelectedCity.Value) ? Convert.ToUInt32(hdnSelectedCity.Value) : 0;

            BindPrices(makeId, cityId);

            BindCities();

            ddlCities.SelectedValue = cityId.ToString();
            ddlCities.Enabled = true;

            ddlPriceStates.SelectedValue = ddlStates.SelectedValue;
            ddlPriceCities.Enabled = true;
        }

        /// <summary>
        /// Function to bind the make pricing for given cities with repeater
        /// </summary>
        private void BindPrices(uint makeId, uint cityId)
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IShowroomPricesRepository, BikeShowroomPrices>();
                IShowroomPricesRepository pricesRepo = container.Resolve<IShowroomPricesRepository>();

                IEnumerable<BikePrice> objPriceList = pricesRepo.GetBikePrices(makeId, cityId);

                if (objPriceList != null)
                {
                    rptPrices.DataSource = objPriceList;
                    rptPrices.DataBind();
                }
            }
        }

        private void SaveBikePrices(object sender, EventArgs e)
        {
            SavePrices();
        }


        /// <summary>
        /// Modified By : Deepak Israni on 21 Feb 2018
        /// Description : Add call to update bikewalepricingindex [ES Index]
        /// </summary>
        private void SavePrices()
        {
            string priceData = string.Empty, citiesList = string.Empty;


            priceData = ParsePriceData();
            citiesList = ParseCitiesList();

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IShowroomPricesRepository, BikeShowroomPrices>();
                container.RegisterType<IBikeModelsRepository, BikeModelsRepository>();
                container.RegisterType<ICacheManager, MemcacheManager>();
                container.RegisterType<IBikeVersions, BikeVersionsRepository>();
                container.RegisterType<IBikeVersionsCacheRepository, BikeVersionsCacheRepository>();

                IShowroomPricesRepository pricesRepo = container.Resolve<IShowroomPricesRepository>();

                container.RegisterType<IBwPrice, BwPrice>();
                IBwPrice bwPrice = container.Resolve<IBwPrice>();

                if (!String.IsNullOrEmpty(priceData) && !String.IsNullOrEmpty(citiesList))
                {
                    pricesRepo.SaveBikePrices(priceData, citiesList, Convert.ToInt32(CurrentUser.Id));
                }
                ClearBWCache();
                ClearAmpCache();

                bwPrice.UpdateModelPriceDocument(priceData, citiesList);
            }

            ShowBikePrices();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   ClearBWCache
        /// Modified By : Vivek Singh Tomar on 31 July 2017
        /// Description : Clear city list cache for model when city prices is updated
        /// Modified by : Ashutosh Sharma on 27 Nov 2017
        /// Description : Added call to ClearSeriesCache.
        /// </summary>
        private void ClearBWCache()
        {
            string cities = hdnSelectedCities.Value;
            string[] arrCity = cities.Split(',');
            foreach (string cityId in arrCity)
            {
                BwMemCache.ClearNewLaunchesBikes(cityId);
            }
            string[] arrModel = modelIds.Split(',');
            foreach (string model in arrModel)
            {
                //To clear price quote for city
                uint modelId;
                if (uint.TryParse(model, out modelId))
                {
                    BwMemCache.ClearPriceQuoteCity(modelId);
                }

            }
            string[] arrSeries = seriesIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var series in arrSeries)
            {
                BwMemCache.ClearSeriesCache(Convert.ToUInt32(series), !String.IsNullOrEmpty(ddlMakes.SelectedValue) ? Convert.ToUInt32(ddlMakes.SelectedValue) : 0);
            }
            //To clear new launched bikes cache
            MemCachedUtil.Remove("BW_NewLaunchedBikes_V1");
            BwMemCache.ClearPopularBikesByMakes(!String.IsNullOrEmpty(ddlMakes.SelectedValue) ? Convert.ToUInt32(ddlMakes.SelectedValue) : 0);
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 17 July 2018
        /// Description : AMP Cache clear for Model Page
        /// </summary>
        private void ClearAmpCache()
        {
            string cities = hdnSelectedCities.Value;
            string[] commaSeperator = new string[] { "," };
            string[] arrCity = cities.Split(commaSeperator, StringSplitOptions.RemoveEmptyEntries);
            uint[] arrModel = Array.ConvertAll(modelIds.Split(commaSeperator, StringSplitOptions.RemoveEmptyEntries), s => uint.Parse(s));
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMakesRepository, BikeMakesRepository>()
                        .RegisterType<IBikeModelsRepository, BikeModelsRepository>()
                        .RegisterType<IAmpCache,AmpCache>();
                IAmpCache _ampCache = container.Resolve<IAmpCache>();

                bool isExshowroomCity = false;
                if (Array.IndexOf(arrCity, "1") > -1)
                {
                    isExshowroomCity = true;
                }
                if (isExshowroomCity)
                {
                    _ampCache.UpdateModelAmpCache(arrModel);
                }
            }
        }

        /// <summary>
        /// Function to retrieve the prices from the table and populate them in the string
        /// e.g. 900#c0l#150000#c0l#3213#c0l#1500|r0w|902#c0l#175000#c0l#3683#c0l#1500
        /// where |r0w| represents row split string and #c0l# represents column split string
        /// Modified by : Ashutosh Sharma on 27 Nov 2017
        /// Description : Added seriesIds to get series id of all selected models.
        /// </summary>
        /// <returns>Returns prices in the form of the string e.g. </returns>
        private string ParsePriceData()
        {
            string priceData = string.Empty;

            try
            {
                // Parsing the price data
                if (rptPrices.Items != null)
                {
                    for (int i = 0; i < rptPrices.Items.Count; i++)
                    {
                        TextBox txtPrice = (TextBox)rptPrices.Items[i].FindControl("txtPrice");
                        TextBox txtInsurance = (TextBox)rptPrices.Items[i].FindControl("txtInsurance");
                        TextBox txtRTO = (TextBox)rptPrices.Items[i].FindControl("txtRTO");
                        CheckBox chkUpdate = (CheckBox)rptPrices.Items[i].FindControl("chkUpdate");

                        string versionid = txtPrice.Attributes["VersionId"];
                        string modelId = txtPrice.Attributes["data-modeldid"];
                        string seriesId = txtPrice.Attributes["data-seriesId"];
                        string price = txtPrice.Text;
                        string insurance = txtInsurance.Text;
                        string rto = txtRTO.Text;
                        modelIds += String.Format("{0},", modelId);
                        if (chkUpdate.Checked)
                        {
                            Trace.Warn("Saving Prices...");

                            if (!String.IsNullOrEmpty(price) && !String.IsNullOrEmpty(insurance) && !String.IsNullOrEmpty(rto))
                            {
                                priceData += String.Format("{0}#c0l#{1}#c0l#{2}#c0l#{3}|r0w|", versionid, price, insurance, rto);
                            }
                            seriesIds += string.Format("{0},", seriesId);
                        }
                    }
                }

                priceData = priceData.Substring(0, priceData.LastIndexOf("|r0w|"));
                modelIds = modelIds.Substring(0, modelIds.LastIndexOf(","));
                seriesIds = seriesIds.Substring(0, seriesIds.LastIndexOf(','));


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);

            }

            return priceData;
        }

        private string ParseCitiesList()
        {
            string citiesList = string.Empty;

            citiesList = hdnSelectedCities.Value.Replace(",", "|r0w|");

            return citiesList;
        }

    } // class
} // namespace