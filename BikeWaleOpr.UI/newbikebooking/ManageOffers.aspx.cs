using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BikeWaleOpr.Common;
using System.Configuration;
using BikeWaleOpr.Entities;
using System.Net.Http.Headers;
using System.Net.Http;
using BikeWaleOpr.Controls;
using System.Data.SqlClient;

namespace BikeWaleOpr.BikeBooking
{
    public class ManageOffers : System.Web.UI.Page
    {
        protected DropDownList DropDownMake, DropDownModels, drpCity, drpOffers, ddlUpdOffers, ddlHours, ddlMins,ddlState;
        protected TextBox offerText, txtUpdOffer, offerValue, txtUpdOfferValue;
        protected CheckBox chkIsPriceImpact;
        protected Button btnAdd, btnUpdate,btnCopyOffers;
        protected string cwHostUrl = string.Empty;
        protected Repeater offer_table;
        protected Label lblSaved,lblTransferStatus;
        protected DateControl dtDate;
        protected HiddenField hdn_modelId,hdnCities,hdnOffersIds; //hdn_cityId, , hdn_offerType, hdn_dtDate, hdn_dtMonth, hdn_ddlHours, hdn_ddlMins;
        protected string userId = "0";

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            btnAdd.Click += new EventHandler(SaveOffers);  
            btnCopyOffers.Click += new EventHandler(btnCopyOffers_click);
        }
                
        #region Pivotal Tracker #95410582
		/// <summary>
        /// Author  :   Sumit Kate
        /// Handles the click event of Copy Offers button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyOffers_click(object sender, EventArgs e)
        {
         	string offerIds = null;
            string cities = null;
            string dealerId = String.Empty;

            dealerId = Request.QueryString["dealerId"];
            if((!String.IsNullOrEmpty(hdnCities.Value)) && (!String.IsNullOrEmpty(hdnOffersIds.Value)))
            {
                cities  = hdnCities.Value.Trim(); 
                offerIds= hdnOffersIds.Value.Trim();
                CopyOffersToCities(dealerId,offerIds,cities);
            }
        }

        /// <summary>
        /// Author  :   Sumit Kate
        /// Copies the Dealers offers to different cities
        /// </summary>
        /// <param name="dealerId">Dealer Id</param>
        /// <param name="lstOfferIds">Comma Separated Offer ids</param>
        /// <param name="lstCityId">Comma Separated City ids</param>
        private void CopyOffersToCities(string dealerId, string lstOfferIds,string lstCityId)
        {
            string requestType = String.Empty;
            string apiUrl = String.Empty;
            bool isSuccess = false;
            bool isPost = false;
            try
            {
                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                requestType = "application/json";
                apiUrl = String.Format("/api/Dealers/CopyOffersToCities/?dealerId={0}&lstOfferIds={1}&lstCityId={2}",dealerId,lstOfferIds ,lstCityId);                                
                isPost = BWHttpClient.PostSync<bool>(cwHostUrl, requestType, apiUrl, isSuccess);
                if(isPost){
                    lblTransferStatus.Visible = true;                    
                }                    
                GetDealerOffers();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
 
	    #endregion

        private void SaveOffers(object sender, EventArgs e)
        {
            SaveDealerOffers();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            userId = CurrentUser.Id;
            lblSaved.Text = "";
            lblTransferStatus.Visible = false;
            Trace.Warn("userId : ", userId.ToString());
            if (!IsPostBack)
            {
                userId = CurrentUser.Id;
                
                //DateTime _today = DateTime.Today;
                //Trace.Warn("date="+_today);
                ////dtDate.Value = _today;
                //dtDate.Value = DateTime.Today;
                FillCities();
                FillMakes();
                GetOfferTypes();
                LoadHoursMins();
                GetDealerOffers();
                ListItem item = new ListItem("--Select Model--", "0");
                DropDownModels.Items.Insert(0, item);
                dtDate.Value = DateTime.Now;
                FillStates();
            }
        }

        private void FillCities()
        {
            try
            {
                ManageCities objCities = new ManageCities();
                DataSet ds = objCities.GetCWCities(0, "ALL");
                //Trace.Warn("cities : " + ds.Tables[0].Rows.Count);
                drpCity.DataSource = ds.Tables[0];
                drpCity.DataTextField = "Text";
                drpCity.DataValueField = "Value";

                drpCity.DataBind();
                drpCity.Items.Insert(0,new ListItem("--Select City--","-1"));
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Populates the States dropdown in Copy Offers Section
        /// </summary>
        private void FillStates()
        {
         	try
            {
                ManageStates objStates = new ManageStates();
                DataSet ds = objStates.GetAllStatesDetails();
                //Trace.Warn("cities : " + ds.Tables[0].Rows.Count);
                ddlState.DataSource = ds.Tables[0];
                ddlState.DataTextField = "Name";
                ddlState.DataValueField = "ID";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("--Select State--", "-1"));
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Suresh Prajapati On 03rd Nov 2014.
        /// Description : Added for binding Make names in drop down.
        /// </summary>

        private void FillMakes()
        {
            try
            {
                DataTable dt;
                MakeModelVersion mmv = new MakeModelVersion();
                dt = mmv.GetMakes("New");

                if (dt.Rows.Count > 0)
                {
                    DropDownMake.DataSource = dt;
                    DropDownMake.DataTextField = "Text";
                    DropDownMake.DataValueField = "Value";
                    DropDownMake.DataBind();

                    ListItem item = new ListItem("--Select Make--", "0");
                    DropDownMake.Items.Insert(0, item);
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
        /// Created By : Suresh Prajapati On 03rd Nov 2014.
        /// Description : Added for binding Offer Types in drop down.
        /// </summary>

        private async void GetOfferTypes()
        {
            try
            {
                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                // get pager instance

                string _apiUrl = "/api/Dealers/GetOfferTypes/";
                // Send HTTP GET requests

                DataTable dt = null;
                dt = await BWHttpClient.GetApiResponse<DataTable>(cwHostUrl, _requestType, _apiUrl, dt);

                if (dt != null)
                {
                    drpOffers.DataSource = dt;
                    drpOffers.DataTextField = "Text";
                    drpOffers.DataValueField = "Value";
                    drpOffers.DataBind();
                    drpOffers.Items.Insert(0, new ListItem("--Select Offer Types--", "-1"));

                    //Binding in Update html drop down for offer types
                    ddlUpdOffers.DataSource = dt;
                    ddlUpdOffers.DataTextField = "Text";
                    ddlUpdOffers.DataValueField = "Value";
                    ddlUpdOffers.DataBind();
                    ddlUpdOffers.Items.Insert(0, new ListItem("--Select Offer Types--", "-1"));
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Suresh Prajapati on 03rd Nov, 2014.
        /// Discription : Binds the List of Entity to repeater for particular dealer of city.
        /// </summary>

        private async void GetDealerOffers()
        {
            try
            {
                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                // get pager instance

                string _apiUrl = "/api/Dealers/GetDealerOffers/?dealerId="+Request.QueryString["dealerId"];
                // Send HTTP GET requests

                List<OfferEntity> objOfferList = null;
                DateTime dateTimeVal = new DateTime();
                objOfferList = await BWHttpClient.GetApiResponse<List<OfferEntity>>(cwHostUrl, _requestType, _apiUrl, objOfferList);

                if (objOfferList != null && objOfferList.Count > 0)
                {
                    offer_table.DataSource = objOfferList;
                    offer_table.DataBind();
                    dtDate.Value = dateTimeVal;
                    ddlHours.SelectedValue = int.Parse(dateTimeVal.ToString("HH")).ToString();
                    ddlMins.SelectedValue = int.Parse(dateTimeVal.ToString("mm")).ToString();
                }
               
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Suresh Prajapati on 03rd Nov, 2014.
        /// Discription : Binds the Added .
        /// </summary>

        private void SaveDealerOffers()
        {
            string hour, min;
            try
            {
                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";
                //hdn_ddlHours.Value
                if(Convert.ToInt32(ddlHours.SelectedValue) < 10)
                     hour = "0" + ddlHours.SelectedValue; //hdn_ddlHours.Value;
                else
                     hour = ddlHours.SelectedValue; // hdn_ddlHours.Value;
                //hdn_ddlMins.Value
                if(Convert.ToInt32(ddlMins.SelectedValue) < 10)
                    min = "0" + ddlMins.SelectedValue; //hdn_ddlMins.Value;
                else
                    min = ddlMins.SelectedValue; //hdn_ddlMins.Value;

                string fullDate = dtDate.Value.ToString("yyyy-MM-dd") + "T" + hour + ":" + min + ":00";
                bool isPriceImpact = chkIsPriceImpact.Checked;
                string _apiUrl = "/api/Dealers/SaveDealerOffer/?dealerId=" + Request.QueryString["dealerId"] + "&cityId=" + drpCity.SelectedValue + "&userId=" + CurrentUser.Id + "&modelId=" + hdn_modelId.Value + "&offercategoryId=" + drpOffers.SelectedValue + "&offerText=" + Server.UrlEncode(offerText.Text) + "&offerValue=" + offerValue.Text + "&offervalidTill=" + fullDate +"&isPriceImpact=" + chkIsPriceImpact.Checked;
         
                Trace.Warn("url : " + cwHostUrl + _apiUrl);
                // Send HTTP GET requests
                bool isSuccess = false;
                var x = BWHttpClient.PostSync<bool>(cwHostUrl, _requestType, _apiUrl, isSuccess);
               
                if(x)
                    lblSaved.Text = "Offer added successfully";

                GetDealerOffers();
                //Response.Redirect("/newbikebooking/ManageOffers.aspx?dealerId=4", true);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }

        private void LoadHoursMins()
        {
            ListItem item;
            for (int i = 0; i < 24; ++i)
            {
                if (i < 10)
                    item = new ListItem("0" + i.ToString(), i.ToString());
                else
                    item = new ListItem(i.ToString(), i.ToString());

                ddlHours.Items.Insert(i, item);
            }
            for (int i = 0; i < 60; ++i)
            {
                if (i < 10)
                    item = new ListItem("0" + i.ToString(), i.ToString());
                else
                    item = new ListItem(i.ToString(), i.ToString());

                ddlMins.Items.Insert(i, item);
            }
            ddlHours.SelectedValue = int.Parse(DateTime.Now.ToString("HH")).ToString();
            ddlMins.SelectedValue = int.Parse(DateTime.Now.ToString("mm")).ToString();
        }
    }
}