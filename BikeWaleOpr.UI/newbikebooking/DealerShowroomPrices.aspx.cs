
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
using System.Configuration;
using BikeWaleOpr.Entities;
using Newtonsoft.Json;

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
        protected   List<PQ_Price> _objCategory = null;
        protected  List<DealerPriceEntity> _objPQ = null;
        protected  List<DealerPriceEntity>  _objPQE = null;
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
            Trace.Warn("inside save load : delar id = "  + _dealerId + "city : " + hdnSelectedCityId.Value);
             
 	        SavePrices(rptVersions, Convert.ToUInt32(_dealerId), Convert.ToUInt32(hdnSelectedCityId.Value));
            GetDealerPrices(Convert.ToUInt32(hdnSelectedCityId.Value),Convert.ToUInt32(hdnCmbModel.Value),Convert.ToUInt32(_dealerId));
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

            if(!String.IsNullOrEmpty(categories))
                categories = categories.Substring(0, categories.Length - 1);

            //Trace.Warn("add categories : " + )
            GetDealerPrices(Convert.ToUInt32(hdnSelectedCityId.Value),Convert.ToUInt32(hdnCmbModel.Value),Convert.ToUInt32(_dealerId));
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

			if ( !IsPostBack )
			{   
               
                GetCategoryItemsList();
              
                getStates();

				sql = "SELECT ID, Name FROM BikeMakes WHERE IsDeleted <> 1 AND New = 1 AND Futuristic = 0 ORDER BY NAME";
				op.FillDropDown( sql, cmbMake, "Name", "ID" );
				ListItem item = new ListItem( "--Select--", "0" );
				cmbMake.Items.Insert( 0, item );
				
			}
			
			sql = "SELECT ID, Name, BikeMakeId FROM BikeModels WHERE IsDeleted <> 1 AND New = 1 AND Futuristic = 0 ORDER BY Name";
            string Script = op.GenerateChainScript("cmbMake", "cmbModel", sql, hdnCmbModel.Value); 	
			ClientScript.RegisterStartupScript(this.GetType(), "ChainScript", Script );
		}

        private void SetDefaultAttribute()
        {
 	       foreach (RepeaterItem ri in rptcatItem.Items)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)ri.FindControl("chkCat");
                List<PQ_Price> _newList = new List<PQ_Price>();
                
                if(Convert.ToUInt32(chk.Attributes["categoryid"]) == 3)
                       chk.Checked = true;
                 if(Convert.ToUInt32(chk.Attributes["categoryid"]) == 5)
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
	
		void btnShow_Click( object Sender, EventArgs e )
		{
            Trace.Warn("hdnSelectedCityId : " + hdnSelectedCityId.Value + "model : " + hdnCmbModel.Value);
		    BindCityDropDown();
            GetDealerPrices(Convert.ToUInt32(hdnSelectedCityId.Value), Convert.ToUInt32(hdnCmbModel.Value), Convert.ToUInt32(_dealerId));
		}
		
		void btnRemove_Click(object sender, EventArgs e)
		{
           string versionIds =  GetSelectedIds(rptVersions);

            if(!String.IsNullOrEmpty(versionIds))
                RemovePrices(Convert.ToUInt32(hdnSelectedCityId.Value),Convert.ToUInt32(_dealerId),versionIds);
            //else
            //    RemovePrices(Convert.ToUInt32(hdnSelectedCityId.Value),Convert.ToUInt32(_dealerId),"1,2");
		}

        private void RemovePrices(uint cityId, uint dealerId, string versionIds)
        {

            string _apiUrl = "api/DealerPriceQuote/DeletePrices/?dealerId="+ dealerId + "&cityId=" + cityId + "&versionIdList=" + versionIds ;

            bool isRemoved = BWHttpClient.DeleteSync(_abHostUrl,_requestType,_apiUrl);

            GetDealerPrices(cityId,Convert.ToUInt32(hdnCmbModel.Value),dealerId);
            Trace.Warn("removed : " + isRemoved);
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

        public bool SavePrices(Repeater rptVersions,  uint dealerId, uint cityId)
        {
            bool isPriceSaved = false;
            
            DataTable table = new DataTable();

            table.Columns.Add("DealerId", typeof(int));
            table.Columns.Add("BikeVersionId", typeof(int));
            table.Columns.Add("CityId", typeof(int));
            table.Columns.Add("ItemId", typeof(Int16));
            table.Columns.Add("Itemvalue", typeof(int));

            Trace.Warn("reapeater count : " + rptVersions.Items.Count);

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
                             Trace.Warn("checked : " + lbVersionId.Text + " lblCategoryId : " + lblCategoryId.Text + " txtvalue : " + txtValue.Text);
                            if (lbVersionId.Text.Length > 0 && txtValue.Text.Trim().Length > 0)
                            {
                                table.Rows.Add(dealerId,lbVersionId.Text.Trim(),cityId,lblCategoryId.Text.Trim(),txtValue.Text.Trim());
                                Trace.Warn("dealer id : " + dealerId + " categiry id : "+ lblCategoryId.Text.Trim() + " txt value : " + txtValue.Text.Trim()); 
                            }
                        }
                    }
                }
            }

            if(table.Rows.Count > 0)
            {
                for(int i=0 ;i<table.Rows.Count ; i++)
                {
                    Trace.Warn( i + " th row :DealerId : " +table.Rows[i]["DealerId"] + " BikeVersionId: " + table.Rows[i]["BikeVersionId"] + " cityId : " + table.Rows[i]["CityId"] + " Item id :" + table.Rows[i]["ItemId"] + " itm value : " + table.Rows[i]["Itemvalue"]);
        
                }

                SaveVersionPrice(table);
            }
                //Trace.Warn("datatable row : " + table.Rows[0]["BikeVersionId"] + " count : " + table.Rows.Count);
            
            return isPriceSaved;
        }

        private void SaveVersionPrice(DataTable table)
        {
             string result = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
 	        Trace.Warn("inside save prices ");
            Trace.Warn("selected rows count :  " +  table.Rows.Count);
            Trace.Warn("resulted json : " + result);
            string _apiUrl = "api/DealerPriceQuote/SaveDealerPrices/";
            Trace.Warn("Save api url : " + _abHostUrl+_apiUrl );
           //  DataTable table = null;
            bool isSaved = BWHttpClient.PostSync<string>(_abHostUrl,_requestType,_apiUrl, result);
            Trace.Warn("isSaved : " + isSaved);
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
            Trace.Warn("Inside GetItemValues ");

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
            return dtPQDistinctItems ;
        }

        //protected List<PQ_Price> GetCategoryValues( uint versionId)
        //{
        // //var filteredItems = results.Where( p => p.Name != null && p.Name.ToUpper().Contains(queryString.ToUpper());
        //    List<PQ_Price> filteredPrices = new List<PQ_Price>();
        //    //return 

        //     //foreach(DealerPriceEntity item in _objPQ)
        //     //{
        //     //    Trace.Warn("Prices : " + item.Price);
        //     //}
        //    if(_objPQE == null)
        //    {
        //        Trace.Warn("inside get values : " + versionId);
        //    }
            
        //    foreach(DealerPriceEntity item in _objPQE)
        //    {
        //        if(item.Version.VersionId == versionId)
        //        {
        //            PQ_Price _objP = new PQ_Price();
        //            _objP.Price =  item.Price.Price;
        //            Trace.Warn("prices : "+ _objP.Price);
        //            filteredPrices.Add(_objP);
        //        }
        //    }

        //    return filteredPrices;
        //}

        private async void GetCategoryItemsList()
        {
            try
            {
                string _apiUrl = "api/DealerPriceQuote/GetBikeCategoryNames/?categoryList=";
                // Send HTTP GET requests 
                 Trace.Warn("url : " + _abHostUrl + _apiUrl);
                _objCategory = await BWHttpClient.GetApiResponse<List<PQ_Price>>(_abHostUrl, _requestType, _apiUrl, _objCategory);
                
                Trace.Warn( "count : " +_objCategory.Count);

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

       
        private async void GetDealerPrices(uint CityId, uint modelId, uint DealerId)
        {
            Trace.Warn("inside show getdealer");
            try
            {
                DataSet ds = null ;

                string _apiUrl = "api/DealerPriceQuote/GetDealerPrices/?cityId=" + CityId + "&modelId=" + modelId + "&dealerId=" + DealerId;
                // Send HTTP GET requests 
                //Trace.Warn("get url : " + _abHostUrl );
                  Trace.Warn("url : " + _abHostUrl + _apiUrl);
                ds = await BWHttpClient.GetApiResponse<DataSet>(_abHostUrl, _requestType, _apiUrl, ds);
               
               Trace.Warn("categories : " + categories);

                if (ds.Tables.Count > 0 && ds != null)
                {
                    if(ds.Tables[1].Rows.Count > 0 && String.IsNullOrEmpty(categories))
                    {
                        dtVersionsData = ds.Tables[1];
                        dtPQDistinctItems = ds.Tables[1].DefaultView.ToTable(true,"ItemCategoryId","ItemName");           
           
                         rptVersions.DataSource = ds.Tables[0];
                         rptVersions.DataBind();
                    }
                    else
                    {
                     
                        List<PQ_Price> objList = null;

                        if(String.IsNullOrEmpty(categories))
                            categories = "3,5";

                        _apiUrl = "api/DealerPriceQuote/GetBikeCategoryNames/?categoryList=" + categories;
                          Trace.Warn("url : " + _abHostUrl + _apiUrl);
                        objList = await BWHttpClient.GetApiResponse<List<PQ_Price>>(_abHostUrl, _requestType, _apiUrl, objList);

                        if(objList != null)
                        {
                            DataTable table = new DataTable();

	                        table.Columns.Add("ItemCategoryId", typeof(int));
	                        table.Columns.Add("ItemName", typeof(string));

                            foreach(PQ_Price item in objList)
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
