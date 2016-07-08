using BikewaleOpr.DAL;
using BikewaleOpr.DALs;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.BikeBooking
{
    public class Default : System.Web.UI.Page
    {
        protected DropDownList drpCity, drpAllCity, drpDealer, ddlState, ddlDealerCity;
        protected ListBox drpPriceHead;
        protected List<PQ_Price> _objNewCategory = null;
        protected List<PQ_Price> _objCategory = null;
        protected DataTable dtPQDistinctItems = null;
        protected string categories = string.Empty;
        protected DataTable dtVersionsData = null;
        protected Repeater rptModels, rptcatItem;
        private string _requestType = "application/json";
        protected HtmlInputControl hdnCityId, hdnMakeId, hdnDealerId, hdnCities, hdnDealerList, hdnDealerCity;
        protected string cwHostUrl = ConfigurationManager.AppSettings["BwOprHostUrlForJs"];
        protected string BwOprHostUrl = ConfigurationManager.AppSettings["BwOprHostUrlForJs"];
        protected string BwHostUrl = ConfigurationManager.AppSettings["BwHostUrlForJs"];
        protected HtmlInputButton btnManagePrice;
        protected HtmlGenericControl selectCityPriceHead;
        protected uint dealerId, cityId, makeId;
        protected Button btnAddCat, addShowPrice, btnUpdate, btnDelete, btnTransferPriceSheet, btnUpdateDealerPrice;
        //protected string choice = string.Empty;
        protected Label lblSaved, lblTransferStatus, lblDealerPriceStatus;
        protected string selectedChoice = string.Empty;
        protected string[] a;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnManagePrice.ServerClick += new EventHandler(DefineDealerCityMake);
            addShowPrice.Click += new EventHandler(AddShowprice);
            btnUpdate.Click += new EventHandler(UpdateVersionPrice);
            btnDelete.Click += new EventHandler(DeletePriceAvailability);
            btnTransferPriceSheet.Click += new EventHandler(btnTransferPriceSheet_Click);
            btnUpdateDealerPrice.Click += new EventHandler(btnCopyDealerPrice_Click);
            InitializeComponent();
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5 Oct 2015
        /// Summary : To copy Price sheet for other dealer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyDealerPrice_Click(object sender, EventArgs e)
        {
            string[] dealers = null;
            List<string> lstDealerId = null;
            try
            {
                if (!String.IsNullOrEmpty(hdnDealerList.Value))
                {
                    dealers = hdnDealerList.Value.Split(',');
                    if (dealers != null && dealers.Length > 0)
                    {
                        lstDealerId = new List<string>();
                        foreach (string dealerId in dealers)
                            lstDealerId.Add(dealerId);

                        CopyPricesToDealers(lstDealerId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "NewBikebooking.Default.btnCopyDealerPrice_Click");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5 Oct 2015
        /// Summary : To copy price to multiple dealer
        /// </summary>
        private void CopyPricesToDealers(List<string> objDealerList)
        {
            DataTable dtPriceSheet = null;
            TextBox txtValue = null;
            Label lbVersionId = null, lblCategoryId = null;
            Repeater rptValues = null;
            string jsonPriceSheet = String.Empty, apiUrl = String.Empty;
            bool isTransferred = false;

            try
            {
                dtPriceSheet = new DataTable();
                dtPriceSheet.Columns.Add("DealerId", typeof(int));
                dtPriceSheet.Columns.Add("BikeVersionId", typeof(int));
                dtPriceSheet.Columns.Add("CityId", typeof(int));
                dtPriceSheet.Columns.Add("ItemId", typeof(Int16));
                dtPriceSheet.Columns.Add("Itemvalue", typeof(object));
                uint dealerCity = Convert.ToUInt32(hdnDealerCity.Value);

                if (rptModels != null && rptModels.Items != null && rptModels.Items.Count > 0)
                {
                    for (int outerRepeaterItemIndex = 0; outerRepeaterItemIndex < rptModels.Items.Count; outerRepeaterItemIndex++)
                    {
                        lbVersionId = rptModels.Items[outerRepeaterItemIndex].FindControl("lblVersionId") as Label;
                        rptValues = rptModels.Items[outerRepeaterItemIndex].FindControl("rptValues") as Repeater;

                        if (rptValues != null && rptValues.Items.Count > 0)
                        {
                            for (int innerRepeaterItemIndex = 0; innerRepeaterItemIndex < rptValues.Items.Count; innerRepeaterItemIndex++)
                            {
                                txtValue = (TextBox)rptValues.Items[innerRepeaterItemIndex].FindControl("txtValue");
                                lblCategoryId = (Label)rptValues.Items[innerRepeaterItemIndex].FindControl("lblCategoryId");

                                if (lbVersionId.Text.Length > 0 && txtValue.Text.Trim().Length > 0)
                                {
                                    //3. populate newly created DataTable with Price Sheet data and with City list
                                    foreach (string dealerId in objDealerList)
                                    {
                                        dtPriceSheet.Rows.Add(dealerId, lbVersionId.Text.Trim(), dealerCity, lblCategoryId.Text.Trim(), txtValue.Text.Trim().Equals("NA") ? null : txtValue.Text.Trim());
                                    }
                                }
                            }
                        }
                    }
                }

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                    isTransferred = objPQ.SaveDealerPrice(dtPriceSheet);
                }

                if (isTransferred)
                    lblDealerPriceStatus.Visible = true;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "NewBikebooking.Default.CopyPricesToDealers");
                objErr.SendMail();
            }
        }

        #region Pivotal Tracker # : 95144444 & 96417936 Author : Sumit Kate
        /// <summary>
        /// Transfer Price Sheet button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTransferPriceSheet_Click(object sender, EventArgs e)
        {
            string[] cities = null;
            List<string> lstCityId = null;
            try
            {
                if (!string.IsNullOrEmpty(hdnCities.Value))
                {
                    cities = hdnCities.Value.Split(',');
                    if (cities != null && cities.Length > 0)
                    {
                        lstCityId = new List<string>();
                        foreach (string city in cities)
                        {
                            lstCityId.Add(city);
                        }
                        CopyPricesToCities(lstCityId);
                    }
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
        /// It reads the Price sheet repeater index table and sends the JSON of the Table to AutoBiz API
        /// </summary>
        /// <param name="lstCityId">List of Cities</param>
        private void CopyPricesToCities(List<string> lstCityId)
        {
            DataTable dtPriceSheet = null;
            TextBox txtValue = null;
            Label lbVersionId = null, lblCategoryId = null;
            Repeater rptValues = null;
            string jsonPriceSheet = String.Empty, apiUrl = String.Empty;
            bool isTransferred = false;

            try
            {

                //1. Create a DataTable object
                dtPriceSheet = new DataTable();
                dtPriceSheet.Columns.Add("DealerId", typeof(int));
                dtPriceSheet.Columns.Add("BikeVersionId", typeof(int));
                dtPriceSheet.Columns.Add("CityId", typeof(int));
                dtPriceSheet.Columns.Add("ItemId", typeof(Int16));
                dtPriceSheet.Columns.Add("Itemvalue", typeof(object));
                dealerId = Convert.ToUInt32(hdnDealerId.Value);

                //2. Read the price sheet repeater for Price Quote Data
                if (rptModels != null && rptModels.Items != null && rptModels.Items.Count > 0)
                {
                    for (int outerRepeaterItemIndex = 0; outerRepeaterItemIndex < rptModels.Items.Count; outerRepeaterItemIndex++)
                    {
                        lbVersionId = rptModels.Items[outerRepeaterItemIndex].FindControl("lblVersionId") as Label;
                        rptValues = rptModels.Items[outerRepeaterItemIndex].FindControl("rptValues") as Repeater;

                        if (rptValues != null && rptValues.Items.Count > 0)
                        {
                            for (int innerRepeaterItemIndex = 0; innerRepeaterItemIndex < rptValues.Items.Count; innerRepeaterItemIndex++)
                            {
                                txtValue = (TextBox)rptValues.Items[innerRepeaterItemIndex].FindControl("txtValue");
                                lblCategoryId = (Label)rptValues.Items[innerRepeaterItemIndex].FindControl("lblCategoryId");

                                if (lbVersionId.Text.Length > 0 && txtValue.Text.Trim().Length > 0)
                                {
                                    //3. populate newly created DataTable with Price Sheet data and with City list
                                    foreach (string cityId in lstCityId)
                                    {
                                        dtPriceSheet.Rows.Add(dealerId, lbVersionId.Text.Trim(), cityId, lblCategoryId.Text.Trim(), txtValue.Text.Trim().Equals("NA") ? null : txtValue.Text.Trim());
                                    }
                                }
                            }
                        }
                    }
                }

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                    isTransferred = objPQ.SaveDealerPrice(dtPriceSheet);
                }
                if (isTransferred)
                {
                    lblTransferStatus.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

        private void DeletePriceAvailability(object sender, EventArgs e)
        {
            lblSaved.Text = "";
            dealerId = Convert.ToUInt32(hdnDealerId.Value);
            //   cityId = Convert.ToUInt32(hdnCityId.Value);
            if (Convert.ToInt16(drpAllCity.SelectedValue) <= 0)
                cityId = Convert.ToUInt32(drpCity.SelectedValue);
            else
                cityId = Convert.ToUInt32(drpAllCity.SelectedValue);

            if (dealerId > 0)
            {
                makeId = Convert.ToUInt32(hdnMakeId.Value);
            }
            if (dealerId > 0 && cityId > 0 && makeId > 0)
            {
                DeletePrice(rptModels, dealerId, cityId);
                DeleteAvailability(rptModels, dealerId, cityId);
                GetDealerPrices(cityId, dealerId, makeId);
            }
        }

        private void DeletePrice(Repeater rptModels, uint dealerId, uint cityId)
        {
            string versionIds = GetSelectedIds(rptModels);

            if (!String.IsNullOrEmpty(versionIds))
                RemovePrices(cityId, dealerId, versionIds);
        }
        private void RemovePrices(uint cityId, uint dealerId, string versionIds)
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                objPQ.DeleteVersionPrices(dealerId, cityId, versionIds);
            }
        }
        private bool DeleteAvailability(Repeater rptModels, uint dealerId, uint cityId)
        {
            bool isDaysDeleted = false;
            DataTable dtDelDays = new DataTable();
            dtDelDays.Columns.Add("BikeVersionId", typeof(int));
            dtDelDays.Columns.Add("DealerId", typeof(int));

            for (int i = 0; i < rptModels.Items.Count; i++)
            {
                Label lbVersionId = (Label)rptModels.Items[i].FindControl("lblVersionId");
                HtmlInputCheckBox chkUpdate = (HtmlInputCheckBox)rptModels.Items[i].FindControl("chkUpdate");

                if (chkUpdate.Checked)
                {
                    if (lbVersionId.Text.Length > 0 && dealerId > 0)
                    {
                        dtDelDays.Rows.Add(lbVersionId.Text.Trim(), dealerId);
                    }
                    if (dtDelDays.Rows.Count > 0)
                    {
                        Trace.Warn("Inside if");
                        DeleteAvailabilityDays(dtDelDays);
                    }
                }
            }
            return isDaysDeleted;
        }

        private void DeleteAvailabilityDays(DataTable dtDelDays)
        {
            bool isDeleted = false;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealers, DealersRepository>();
                IDealers objDays = container.Resolve<DealersRepository>();
                isDeleted = objDays.DeleteBikeAvailabilityDays(dtDelDays);
            }
            if (isDeleted)
            {
                lblSaved.Text = "Record(s) Deleted Successfully";
            }
        }

        private string GetSelectedIds(Repeater rptModels)
        {
            StringBuilder sb = null;

            try
            {
                sb = new StringBuilder();

                foreach (RepeaterItem item in rptModels.Items)
                {
                    HtmlInputCheckBox objChkControl = (HtmlInputCheckBox)item.FindControl("chkUpdate");

                    if (objChkControl.Checked == true)
                    {
                        string id = objChkControl.Attributes["versionid"];

                        //concat the id
                        if (sb.Length == 0)
                            sb.Append(id);
                        else
                            sb.Append("," + id);
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return sb.ToString();
        }
        private void UpdateVersionPrice(object sender, EventArgs e)
        {
            dealerId = Convert.ToUInt32(hdnDealerId.Value);
            //cityId = Convert.ToUInt32(hdnCityId.Value);

            if (Convert.ToInt16(drpAllCity.SelectedValue) <= 0)
                cityId = Convert.ToUInt32(drpCity.SelectedValue);
            else
                cityId = Convert.ToUInt32(drpAllCity.SelectedValue);

            if (dealerId > 0)
            {
                makeId = Convert.ToUInt32(hdnMakeId.Value);
            }
            if (dealerId > 0 && cityId > 0 && makeId > 0)
            {
                UpdatePrice(rptModels, dealerId, cityId);
                UpdateDays(rptModels, dealerId, cityId);
                GetDealerPrices(cityId, dealerId, makeId);
            }
        }

        private bool UpdateDays(Repeater rptModels, uint dealerId, uint cityId)
        {
            bool isDaysSaved = false;
            DataTable daysTable = new DataTable();
            daysTable.Columns.Add("DealerId", typeof(int));
            daysTable.Columns.Add("BikeVersionId", typeof(int));
            daysTable.Columns.Add("NumOfDays", typeof(int));

            for (int i = 0; i < rptModels.Items.Count; i++)
            {
                Label lbVersionId = (Label)rptModels.Items[i].FindControl("lblVersionId");
                TextBox txtAvailableDays = (TextBox)rptModels.Items[i].FindControl("lblAvailableDays");
                HtmlInputCheckBox chkUpdate = (HtmlInputCheckBox)rptModels.Items[i].FindControl("chkUpdate");
                if (chkUpdate.Checked)
                {
                    if (lbVersionId.Text.Length > 0 && txtAvailableDays.Text.Trim().Length > 0)
                    {
                        Trace.Warn("na : " + txtAvailableDays.Text);
                        daysTable.Rows.Add(dealerId, lbVersionId.Text.Trim(), txtAvailableDays.Text.Trim());
                    }
                }
            }
            if (daysTable.Rows.Count > 0)
            {
                for (int i = 0; i < daysTable.Rows.Count; i++)
                {
                    Trace.Warn(i + "th row :DealerId : " + daysTable.Rows[i]["DealerId"] + " BikeVersionId: " + daysTable.Rows[i]["BikeVersionId"] + "Available Days : " + daysTable.Rows[i]["NumOfDays"]);
                }

                SaveVersionDays(daysTable);
            }
            return isDaysSaved;
        }
        private bool UpdatePrice(Repeater rptModels, uint dealerId, uint cityId)
        {
            bool isPriceSaved = false;

            DataTable table = new DataTable();

            table.Columns.Add("DealerId", typeof(int));
            table.Columns.Add("BikeVersionId", typeof(int));
            table.Columns.Add("CityId", typeof(int));
            table.Columns.Add("ItemId", typeof(Int16));
            table.Columns.Add("Itemvalue", typeof(int));

            for (int i = 0; i < rptModels.Items.Count; i++)
            {
                Label lbVersionId = (Label)rptModels.Items[i].FindControl("lblVersionId");

                HtmlInputCheckBox chkUpdate = (HtmlInputCheckBox)rptModels.Items[i].FindControl("chkUpdate");
                Repeater rptValues = (Repeater)rptModels.Items[i].FindControl("rptValues");
                if (rptValues.Items.Count > 0)
                {
                    for (int j = 0; j < rptValues.Items.Count; j++)
                    {
                        if (chkUpdate.Checked)
                        {
                            TextBox txtValue = (TextBox)rptValues.Items[j].FindControl("txtValue");
                            Label lblCategoryId = (Label)rptValues.Items[j].FindControl("lblCategoryId");
                            if (lbVersionId.Text.Length > 0 && txtValue.Text.Trim().Length > 0)
                            {
                                table.Rows.Add(dealerId, lbVersionId.Text.Trim(), cityId, lblCategoryId.Text.Trim(), txtValue.Text.Trim());
                            }
                        }
                    }
                }
            }
            if (table.Rows.Count > 0)
            {
                SaveVersionPrice(table);
            }
            return isPriceSaved;
        }

        private void SaveVersionDays(DataTable daysTable)
        {
            bool isSaved = false;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealers, DealersRepository>();
                IDealers objDays = container.Resolve<DealersRepository>();
                isSaved = objDays.SaveBikeAvailability(daysTable);
            }
            if (isSaved)
            {
                lblSaved.Text = "Record(s) Updated Successfully";
            }
        }

        private void SaveVersionPrice(DataTable table)
        {
            bool isSuccess = false;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                isSuccess = objPQ.SaveDealerPrice(table);
            }
            if (isSuccess)
            {
                lblSaved.Text = "Record(s) Updated Successfully";
            }
        }

        private void AddShowprice(object sender, EventArgs e)
        {
            lblSaved.Text = "";
            dealerId = Convert.ToUInt32(hdnDealerId.Value);
            if (Convert.ToInt16(drpAllCity.SelectedValue) <= 0)
                cityId = Convert.ToUInt32(drpCity.SelectedValue);
            else
                cityId = Convert.ToUInt32(drpAllCity.SelectedValue);

            if (dealerId > 0)
            {
                makeId = Convert.ToUInt32(hdnMakeId.Value);
            }
            if (dealerId > 0 && cityId > 0 && makeId > 0)
            {
                GetDealerPrices(cityId, dealerId, makeId);
            }

        }
        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnAddCat.Click += new EventHandler(AddCategories);
        }

        private void FillStates()
        {
            try
            {
                ManageStates objStates = new ManageStates();
                DataSet ds = objStates.GetAllStatesDetails();
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

        private void AddCategories(object sender, EventArgs e)
        {
            lblSaved.Text = "";
            foreach (ListItem listItem in drpPriceHead.Items)
            {
                if (listItem.Selected)
                {
                    categories += listItem.Value + ",";
                }
            }
            if (!String.IsNullOrEmpty(categories))
            {
                categories = categories.Substring(0, categories.Length - 1);
                List<PQ_Price> objList = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objCategoryNames = container.Resolve<DealerPriceQuoteRepository>();
                    objList = objCategoryNames.GetBikeCategoryItems(categories);
                }

                if (objList != null)
                {
                    try
                    {
                        DataTable table = new DataTable();
                        table.Columns.Add("ItemCategoryId", typeof(int));
                        table.Columns.Add("ItemName", typeof(string));
                        foreach (PQ_Price item in objList)
                        {
                            table.Rows.Add(item.CategoryId, item.CategoryName);
                        }
                        if (Convert.ToInt16(drpAllCity.SelectedValue) <= 0)
                            cityId = Convert.ToUInt32(drpCity.SelectedValue);
                        GetDealerPrices(cityId, Convert.ToUInt32(hdnDealerId.Value), Convert.ToUInt32(hdnMakeId.Value));
                    }
                    catch (Exception err)
                    {
                        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                        objErr.SendMail();
                    }
                }
            }
        }

        private void DefineDealerCityMake(object sender, EventArgs e)
        {
            lblSaved.Text = "";
            selectCityPriceHead.Attributes.Add("class", "show, margin-bottom10");
            dealerId = Convert.ToUInt32(hdnDealerId.Value);

            if (Convert.ToInt16(drpAllCity.SelectedValue) <= 0)
                cityId = Convert.ToUInt32(drpCity.SelectedValue);
            else
                cityId = Convert.ToUInt32(drpAllCity.SelectedValue);

            if (dealerId > 0)
            {
                makeId = Convert.ToUInt32(hdnMakeId.Value);
            }
            if (makeId > 0 && dealerId > 0 && cityId > 0)
            {
                GetDealerPrices(cityId, dealerId, makeId);
            }
        }



        private void FillCities()
        {
            try
            {
                ManageCities objCities = new ManageCities();
                DataSet ds = objCities.GetCWCities(0, "ALL");
                //Trace.Warn("cities : " + ds.Tables[0].Rows.Count);
                drpAllCity.DataSource = ds.Tables[0];
                drpAllCity.DataTextField = "Text";
                drpAllCity.DataValueField = "Value";
                drpAllCity.DataBind();
                drpAllCity.Items.Insert(0, new ListItem("--Select City--", "-1"));
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        protected string GetItemValue(string versionID, string categoryId)
        {
            //Trace.Warn("Line 141 :Inside GetItemValues ");

            if (dtVersionsData != null && dtVersionsData.Rows.Count > 0)
            {
                DataRow[] row = dtVersionsData.Select("VersionId=" + versionID + " and ItemCategoryId=" + categoryId);

                if (row.Length > 0)
                {
                    Trace.Warn("Line 149 ItemCategoryId :" + row[0]["ItemCategoryId"].ToString());
                    selectedChoice += row[0]["ItemCategoryId"].ToString() + ",";
                    a = selectedChoice.Split(',');
                    Trace.Warn("Line 150 ItemValue" + row[0]["ItemValue"].ToString());
                    Trace.Warn("Line 153 : selectedChoice : " + selectedChoice);
                    return row[0]["ItemValue"].ToString();
                }
                else
                {
                    return "NA";
                }
            }
            else
            {
                return "NA";
            }
        }
        private void FillCategories(object sender, EventArgs e)
        {
            foreach (RepeaterItem ri in rptcatItem.Items)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)ri.FindControl("chkCat");
                List<PQ_Price> _newList = new List<PQ_Price>();

                if (chk.Checked)
                    categories = categories + chk.Attributes["categoryid"].ToString() + ",";
            }

            if (!String.IsNullOrEmpty(categories))
                categories = categories.Substring(0, categories.Length - 1);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblTransferStatus.Visible = false;
            lblDealerPriceStatus.Visible = false;
            if (!IsPostBack)
            {
                FillCity();
                FillCities();
                FillStates();
            }
        }

        private async void FillCity()
        {
            try
            {
                DataTable dt = null;
                string _apiUrl = "/api/Dealers/GetDealerCities/";
                dt = await BWHttpClient.GetApiResponse<DataTable>(BwHostUrl, _requestType, _apiUrl, dt);

                if (dt != null)
                {
                    drpCity.DataSource = dt;
                    drpCity.DataTextField = "Text";
                    drpCity.DataValueField = "Value";
                    drpCity.DataBind();
                    drpCity.Items.Insert(0, new ListItem("--Select City--", "-1"));

                    //Added By : Sadhana Upadhyay on 5 Oct 2015
                    //To fill city dropdown where dealers are available to copy dealer Price
                    ddlDealerCity.DataSource = dt;
                    ddlDealerCity.DataTextField = "Text";
                    ddlDealerCity.DataValueField = "Value";
                    ddlDealerCity.DataBind();
                    ddlDealerCity.Items.Insert(0, new ListItem("--Select City--", "-1"));
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        protected DataTable GetPQCommonAttrs()
        {
            return dtPQDistinctItems;
        }

        private void GetAllDealers(UInt32 cityId)
        {
            try
            {
                DataTable dt = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objAllDealer = container.Resolve<DealersRepository>();
                    dt = objAllDealer.GetAllDealers(cityId);
                }
                if (dt != null)
                {
                    drpDealer.DataSource = dt;
                    drpDealer.DataTextField = "Text";
                    drpDealer.DataValueField = "Value";
                    drpDealer.DataBind();
                    drpDealer.Items.Insert(0, new ListItem("--Select Dealer--", "-1"));
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        private async void GetCategoryItemsList()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objCategoryNames = container.Resolve<DealerPriceQuoteRepository>();
                    _objCategory = objCategoryNames.GetBikeCategoryItems(string.Empty);
                }
                drpPriceHead.DataSource = _objCategory;
                drpPriceHead.DataTextField = "CategoryName";
                drpPriceHead.DataValueField = "CategoryId";
                drpPriceHead.DataBind();
                Trace.Warn("count : " + _objCategory.Count);

                if (_objCategory != null)
                {
                    _objNewCategory = _objCategory;
                    //drpPriceHead.DataSource = _objCategory;
                    //drpPriceHead.DataTextField = "CategoryName";
                    //drpPriceHead.DataValueField = "CategoryId";
                    //drpPriceHead.DataBind();
                    rptcatItem.DataBind();

                    SetDefaultAttribute();
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        private void SetDefaultAttribute()
        {
            a = selectedChoice.Split(',');

            for (int i = 0; i < a.Length - 1; i++)
            {
                Trace.Warn("Line 376 : a[ " + i + "] = " + a[i]);
            }


            for (int i = 0; i < a.Length - 1; i++)
                drpPriceHead.Items.FindByValue(a[i].ToString()).Selected = true;

            drpPriceHead.Items.FindByValue("5").Selected = true;
            drpPriceHead.Items.FindByValue("3").Selected = true;

        }

        private async void GetDealerPrices(uint cityId, uint DealerId, uint makeId)
        {
            try
            {
                DataSet ds = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                    ds = objPQ.GetDealerPrices(cityId, makeId, dealerId);
                }
                if (ds.Tables.Count > 0 && ds != null)
                {
                    if (ds.Tables[1].Rows.Count > 0 && String.IsNullOrEmpty(categories))
                    {
                        Trace.Warn("inside succ 1");
                        dtVersionsData = ds.Tables[1];
                        dtPQDistinctItems = ds.Tables[1].DefaultView.ToTable(true, "ItemCategoryId", "ItemName");
                        rptModels.DataSource = ds.Tables[0];
                        rptModels.DataBind();
                    }
                    else
                    {
                        List<PQ_Price> objList = null;
                        if (String.IsNullOrEmpty(categories))
                            categories = "3,5";
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                            IDealerPriceQuote objCategoryNames = container.Resolve<DealerPriceQuoteRepository>();
                            objList = objCategoryNames.GetBikeCategoryItems(categories);
                        }
                        if (objList != null)
                        {
                            DataTable table = new DataTable();
                            table.Columns.Add("ItemCategoryId", typeof(int));
                            table.Columns.Add("ItemName", typeof(string));
                            foreach (PQ_Price item in objList)
                            {
                                table.Rows.Add(item.CategoryId, item.CategoryName);
                                selectedChoice += item.CategoryId + ",";
                                a = selectedChoice.Split(',');

                            }
                            dtPQDistinctItems = table;
                            dtVersionsData = ds.Tables[1];
                            rptModels.DataSource = ds.Tables[0];
                            rptModels.DataBind();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            GetCategoryItemsList();
        }
    }
}