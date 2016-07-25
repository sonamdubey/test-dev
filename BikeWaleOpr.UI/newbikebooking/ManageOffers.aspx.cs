using BikewaleOpr.DAL;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.BikeBooking
{
    public class ManageOffers : System.Web.UI.Page
    {
        protected DropDownList DropDownMake, DropDownModels, drpCity, drpOffers, ddlUpdOffers, ddlHours, ddlMins, ddlState;
        protected TextBox offerText, txtUpdOffer, offerValue, txtUpdOfferValue;
        protected CheckBox chkIsPriceImpact;
        protected Button btnAdd, btnUpdate, btnCopyOffers;
        public string cwHostUrl = string.Empty;
        protected Repeater offer_table;
        protected Label lblSaved, lblTransferStatus;
        protected DateControl dtDate;
        protected HiddenField hdn_modelId, hdnCities, hdnOffersIds; //hdn_cityId, , hdn_offerType, hdn_dtDate, hdn_dtMonth, hdn_ddlHours, hdn_ddlMins;
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
            if ((!String.IsNullOrEmpty(hdnCities.Value)) && (!String.IsNullOrEmpty(hdnOffersIds.Value)))
            {
                cities = hdnCities.Value.Trim();
                offerIds = hdnOffersIds.Value.Trim();
                CopyOffersToCities(dealerId, offerIds, cities);
            }
        }

        /// <summary>
        /// Author  :   Sumit Kate
        /// Copies the Dealers offers to different cities
        /// </summary>
        /// <param name="dealerId">Dealer Id</param>
        /// <param name="lstOfferIds">Comma Separated Offer ids</param>
        /// <param name="lstCityId">Comma Separated City ids</param>
        private void CopyOffersToCities(string dealerId, string lstOfferIds, string lstCityId)
        {
            string requestType = String.Empty;
            string apiUrl = String.Empty;
            bool isSuccess = false;
            bool isPost = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    isSuccess = objCity.CopyOffersToCities(Convert.ToUInt32(dealerId), lstOfferIds, lstCityId);
                }

                if (isPost)
                {
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
                drpCity.Items.Insert(0, new ListItem("--Select City--", "-1"));
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

        private void GetOfferTypes()
        {
            try
            {
                DataTable dt = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    dt = objCity.GetOfferTypes();
                }

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

        private void GetDealerOffers()
        {
            try
            {
                int dealerId = Convert.ToInt32(Request.QueryString["dealerId"]);
                List<OfferEntity> objOfferList = null;
                DateTime dateTimeVal = new DateTime();

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    objOfferList = objCity.GetDealerOffers(dealerId);
                }

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
                if (Convert.ToInt32(ddlHours.SelectedValue) < 10)
                    hour = "0" + ddlHours.SelectedValue; //hdn_ddlHours.Value;
                else
                    hour = ddlHours.SelectedValue; // hdn_ddlHours.Value;
                if (Convert.ToInt32(ddlMins.SelectedValue) < 10)
                    min = "0" + ddlMins.SelectedValue; //hdn_ddlMins.Value;
                else
                    min = ddlMins.SelectedValue; //hdn_ddlMins.Value;
                int dealerId = Convert.ToInt32(Request.QueryString["dealerId"]);
                string fullDate = dtDate.Value.ToString("yyyy-MM-dd") + "T" + hour + ":" + min + ":00";
                bool isPriceImpact = chkIsPriceImpact.Checked;
                bool isSuccess = false;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    isSuccess = objCity.SaveDealerOffer(dealerId, Convert.ToUInt32(userId), Convert.ToInt32(drpCity.SelectedValue), (hdn_modelId.Value), Convert.ToInt32(drpOffers.SelectedValue), Server.UrlEncode(offerText.Text), Convert.ToInt32(offerValue.Text), dtDate.Value, chkIsPriceImpact.Checked);
                }

                if (isSuccess)
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