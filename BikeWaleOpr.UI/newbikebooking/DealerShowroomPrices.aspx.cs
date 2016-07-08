
using BikewaleOpr.DALs;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using BikeWaleOpr.Common;
using BikeWaleOpr.Entities;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.NewBikeBooking
{
    public class DealerShowroomPrices : System.Web.UI.Page
    {
        protected HtmlGenericControl spnError;
        protected Button btnSave, btnShow, btnRemove, btnAddCat;
        protected Repeater rptPrices, rptcatItem, rptPrice, rptHeader, rptVersions;
        protected DropDownList cmbMake, ddlStates;
        protected DropDownList drpCity;

        protected HiddenField hdnSelectedCityId, hdnCmbModel;

        public string qryStrMake = "";
        public string qryStrModel = "";
        public string qryStrVersion = "";
        public string qryStrCity = "";
        protected string drpCity_Id = string.Empty;
        protected List<PQ_Price> _objCategory = null;
        protected List<DealerPriceEntity> _objPQ = null;
        protected List<DealerPriceEntity> _objPQE = null;
        protected DataTable dtPQDistinctItems = null;
        protected DataTable dtVersionsData = null;
        protected List<PQ_Price> _objNewCategory = null;
        protected string categories = string.Empty;
        private string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
        private string _requestType = "application/json";
        private string _dealerId = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnShow.Click += new EventHandler(btnShow_Click);
            this.btnAddCat.Click += new EventHandler(AddCategories);
            this.btnSave.Click += new EventHandler(SaveVersionPrices);
            this.btnRemove.Click += new EventHandler(btnRemove_Click);
        }

        private void SaveVersionPrices(object sender, EventArgs e)
        {
            Trace.Warn("inside save load : delar id = " + _dealerId + "city : " + hdnSelectedCityId.Value);

            SavePrices(rptVersions, Convert.ToUInt32(_dealerId), Convert.ToUInt32(hdnSelectedCityId.Value));
            GetDealerPrices(Convert.ToUInt32(hdnSelectedCityId.Value), Convert.ToUInt32(hdnCmbModel.Value), Convert.ToUInt32(_dealerId));
        }


        private void AddCategories(object sender, EventArgs e)
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

            //Trace.Warn("add categories : " + )
            GetDealerPrices(Convert.ToUInt32(hdnSelectedCityId.Value), Convert.ToUInt32(hdnCmbModel.Value), Convert.ToUInt32(_dealerId));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Warn("inside show");
            _dealerId = Request.QueryString["dealerId"];

            CommonOpn op = new CommonOpn();
            string sql;

            //register the ajax library and emits corresponding javascript code
            //for this page
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
            AjaxFunctions aj = new AjaxFunctions();

            if (!IsPostBack)
            {

                GetCategoryItemsList();

                getStates();

                sql = "SELECT ID, Name FROM BikeMakes WHERE IsDeleted <> 1 AND New = 1 AND Futuristic = 0 ORDER BY NAME";
                op.FillDropDown(sql, cmbMake, "Name", "ID");
                ListItem item = new ListItem("--Select--", "0");
                cmbMake.Items.Insert(0, item);

            }

            sql = "SELECT ID, Name, BikeMakeId FROM BikeModels WHERE IsDeleted <> 1 AND New = 1 AND Futuristic = 0 ORDER BY Name";
            string Script = op.GenerateChainScript("cmbMake", "cmbModel", sql, hdnCmbModel.Value);
            ClientScript.RegisterStartupScript(this.GetType(), "ChainScript", Script);
        }

        private void SetDefaultAttribute()
        {
            foreach (RepeaterItem ri in rptcatItem.Items)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)ri.FindControl("chkCat");
                List<PQ_Price> _newList = new List<PQ_Price>();

                if (Convert.ToUInt32(chk.Attributes["categoryid"]) == 3)
                    chk.Checked = true;
                if (Convert.ToUInt32(chk.Attributes["categoryid"]) == 5)
                    chk.Checked = true;

                //for(uint i = 1 ; i <= 4 ; i++)
                //{
                //    if(Convert.ToUInt32(chk.Attributes["categoryid"]) == 2)
                //        chk.Checked = false;
                //    if(Convert.ToUInt32(chk.Attributes["categoryid"]) == i)
                //        chk.Checked = true;
                //}
                //if (chk.Checked)
                //   categories = categories + chk.Attributes["categoryid"].ToString() + ",";
            }
        }

        // Page_Load

        void btnShow_Click(object Sender, EventArgs e)
        {
            Trace.Warn("hdnSelectedCityId : " + hdnSelectedCityId.Value + "model : " + hdnCmbModel.Value);
            BindCityDropDown();
            GetDealerPrices(Convert.ToUInt32(hdnSelectedCityId.Value), Convert.ToUInt32(hdnCmbModel.Value), Convert.ToUInt32(_dealerId));
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            string versionIds = GetSelectedIds(rptVersions);

            if (!String.IsNullOrEmpty(versionIds))
                RemovePrices(Convert.ToUInt32(hdnSelectedCityId.Value), Convert.ToUInt32(_dealerId), versionIds);
            //else
            //    RemovePrices(Convert.ToUInt32(hdnSelectedCityId.Value),Convert.ToUInt32(_dealerId),"1,2");
        }

        private void RemovePrices(uint cityId, uint dealerId, string versionIds)
        {
            bool isRemoved = false;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                isRemoved = objPQ.DeleteVersionPrices(dealerId, cityId, versionIds);
            }
            GetDealerPrices(cityId, Convert.ToUInt32(hdnCmbModel.Value), dealerId);
        }

        protected void BindCityDropDown()
        {
            DataSet ds = null;

            try
            {
                ManageCities objMC = new ManageCities();
                ds = objMC.GetCities(Convert.ToInt32(ddlStates.SelectedValue), "7");
                drpCity.DataSource = ds;
                drpCity.DataTextField = "Text";
                drpCity.DataValueField = "Value";
                drpCity.DataBind();
                drpCity.Items.Insert(0, new ListItem("--Select City--", "0"));
                drpCity.SelectedValue = hdnSelectedCityId.Value;
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public bool SavePrices(Repeater rptVersions, uint dealerId, uint cityId)
        {
            bool isPriceSaved = false;

            DataTable table = new DataTable();

            table.Columns.Add("DealerId", typeof(int));
            table.Columns.Add("BikeVersionId", typeof(int));
            table.Columns.Add("CityId", typeof(int));
            table.Columns.Add("ItemId", typeof(Int16));
            table.Columns.Add("Itemvalue", typeof(int));

            for (int i = 0; i < rptVersions.Items.Count; i++)
            {
                Label lbVersionId = (Label)rptVersions.Items[i].FindControl("lblVersionId");
                HtmlInputCheckBox chkUpdate = (HtmlInputCheckBox)rptVersions.Items[i].FindControl("chkUpdate");
                Repeater rptValues = (Repeater)rptVersions.Items[i].FindControl("rptValues");
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

        private void SaveVersionPrice(DataTable table)
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                objPQ.SaveDealerPrice(table);
            }
        }

        protected void getStates()
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

                }
            }
            catch (Exception ex)
            {
                Trace.Warn("objMS.FillStates  ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        protected string GetItemValue(string versionID, string categoryId)
        {
            if (dtVersionsData != null && dtVersionsData.Rows.Count > 0)
            {
                DataRow[] row = dtVersionsData.Select("VersionId=" + versionID + " and ItemCategoryId=" + categoryId);
                if (row.Length > 0)
                {
                    Trace.Warn("ItemCategoryId :" + row[0]["ItemCategoryId"].ToString());
                    Trace.Warn("ItemValue" + row[0]["ItemValue"].ToString());
                    return row[0]["ItemValue"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return String.Empty;
            }
        }

        protected DataTable GetPQCommonAttrs()
        {
            return dtPQDistinctItems;
        }

        private void GetCategoryItemsList()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objCategoryNames = container.Resolve<DealerPriceQuoteRepository>();
                    _objCategory = objCategoryNames.GetBikeCategoryItems(string.Empty);
                }
                if (_objCategory != null)
                {
                    _objNewCategory = _objCategory;
                    rptcatItem.DataSource = _objCategory;
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


        private void GetDealerPrices(uint cityId, uint modelId, uint dealerId)
        {
            try
            {
                DataSet ds = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPQ = container.Resolve<DealerPriceQuoteRepository>();
                    ds = objPQ.GetDealerPrices(cityId, modelId, dealerId);
                }
                if (ds.Tables.Count > 0 && ds != null)
                {
                    if (ds.Tables[1].Rows.Count > 0 && String.IsNullOrEmpty(categories))
                    {
                        dtVersionsData = ds.Tables[1];
                        dtPQDistinctItems = ds.Tables[1].DefaultView.ToTable(true, "ItemCategoryId", "ItemName");
                        rptVersions.DataSource = ds.Tables[0];
                        rptVersions.DataBind();
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
                            }
                            dtVersionsData = ds.Tables[1];
                            dtPQDistinctItems = table;
                            rptVersions.DataSource = ds.Tables[0];
                            rptVersions.DataBind();
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
        }

        private string GetSelectedIds(Repeater rptVersions)
        {
            StringBuilder sb = null;

            try
            {
                sb = new StringBuilder();
                foreach (RepeaterItem item in rptVersions.Items)
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

    }
}
