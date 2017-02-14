using BikewaleOpr.Cache;
using BikewaleOpr.DALs.BikePricing;
using BikewaleOpr.Entities.BikePricing;
using BikewaleOpr.Interface.BikePricing;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    public class ShowroomPrices : Page
    {
        protected Button btnSearch, btnSavePrices;
        protected DropDownList ddlMakes, ddlStates, ddlCities, ddlPriceCities, ddlPriceStates;
        protected HiddenField hdnSelectedCity, hdnSelectedCities;
        protected Repeater rptPrices;
        protected string qryStrVersion;


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

                hdnSelectedCity.Value = "0";
                ddlCities.Enabled = false;
                ddlPriceCities.Enabled = false;
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
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


        private void SavePrices()
        {
            string priceData = string.Empty, citiesList = string.Empty;
            bool isUpdated = false;

            priceData = ParsePriceData();
            citiesList = ParseCitiesList();

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IShowroomPricesRepository, BikeShowroomPrices>();
                IShowroomPricesRepository pricesRepo = container.Resolve<IShowroomPricesRepository>();

                isUpdated = pricesRepo.SaveBikePrices(priceData, citiesList, Convert.ToInt32(CurrentUser.Id));
                ClearBWCache();
            }

            ShowBikePrices();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   ClearBWCache
        /// </summary>
        private void ClearBWCache()
        {
            string cities = hdnSelectedCities.Value;
            string[] arrCity = cities.Split(',');
            foreach (string cityId in arrCity)
            {
                BwMemCache.ClearNewLaunchesBikes(cityId);
            }
        }

        /// <summary>
        /// Function to retrieve the prices from the table and populate them in the string
        /// e.g. 900#c0l#150000#c0l#3213#c0l#1500|r0w|902#c0l#175000#c0l#3683#c0l#1500
        /// where |r0w| represents row split string and #c0l# represents column split string
        /// </summary>
        /// <returns>Returns prices in the form of the string e.g. </returns>
        private string ParsePriceData()
        {
            string priceData = string.Empty;

            try
            {
                // Parsing the price data
                for (int i = 0; i < rptPrices.Items.Count; i++)
                {
                    TextBox txtPrice = (TextBox)rptPrices.Items[i].FindControl("txtPrice");
                    TextBox txtInsurance = (TextBox)rptPrices.Items[i].FindControl("txtInsurance");
                    TextBox txtRTO = (TextBox)rptPrices.Items[i].FindControl("txtRTO");
                    CheckBox chkUpdate = (CheckBox)rptPrices.Items[i].FindControl("chkUpdate");

                    string versionid = txtPrice.Attributes["VersionId"];
                    string price = txtPrice.Text;
                    string insurance = txtInsurance.Text;
                    string rto = txtRTO.Text;

                    if (chkUpdate.Checked)
                    {
                        Trace.Warn("Saving Prices...");

                        if (!String.IsNullOrEmpty(price) && !String.IsNullOrEmpty(insurance) && !String.IsNullOrEmpty(rto))
                        {
                            priceData += String.Format("{0}#c0l#{1}#c0l#{2}#c0l#{3}|r0w|", versionid, price, insurance, rto);
                        }
                    }
                }

                priceData = priceData.Substring(0, priceData.LastIndexOf("|r0w|"));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
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