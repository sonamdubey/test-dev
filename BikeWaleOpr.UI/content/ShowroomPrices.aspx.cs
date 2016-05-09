/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
using Ajax;
using BikeWaleOPR.DAL.CoreDAL;
using System.Data.Common;
using BikeWaleOPR.Utilities;

namespace BikeWaleOpr.Content
{
	public class ShowroomPrices : Page
	{
		protected HtmlGenericControl spnError;
		protected Button btnSave, btnShow, btnRemove;
		protected Repeater rptPrices;
        protected DropDownList cmbMake, ddlStates, cmbModel;
		protected RadioButton optNew, optUsed;
		protected DropDownList drpCity;

        protected HiddenField hdnSelectedCityId;
		
		public string qryStrMake = "";
		public string qryStrModel = "";
		public string qryStrVersion = "";
		public string qryStrCity = "";

        protected string drpCity_Id = string.Empty;
										 
		protected override void OnInit(EventArgs e)
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
			btnShow.Click += new EventHandler( btnShow_Click );
			btnRemove.Click += new EventHandler( btnRemove_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			CommonOpn op = new CommonOpn();
			string sql;
				
			if( Request.QueryString["make"] != null && Request.QueryString["make"].ToString() != "")
			{
				if( Request.QueryString["model"] != null && Request.QueryString["model"].ToString() != "")
				{
					if( Request.QueryString["version"] != null && Request.QueryString["version"].ToString() != "")
					{
						if( Request.QueryString["city"] != null && Request.QueryString["city"].ToString() != "")
						{
							qryStrMake = Request.QueryString["make"].ToString();
							qryStrModel = Request.QueryString["model"].ToString();
							qryStrVersion = Request.QueryString["version"].ToString();
							qryStrCity = Request.QueryString["city"].ToString();

                            Trace.Warn("make : ", qryStrMake);
                            Trace.Warn("qryStrModel : ", qryStrModel);
                            Trace.Warn("qryStrVersion : ", qryStrVersion);
                            Trace.Warn("qryStrCity : ", qryStrCity);
							
							if( !CommonOpn.CheckId(qryStrMake) && !CommonOpn.CheckId(qryStrModel) && !CommonOpn.CheckId(qryStrVersion) && !CommonOpn.CheckId(qryStrCity))
							{
								return;
							}
						}
					}
				}
			}
			
			
			//register the ajax library and emits corresponding javascript code
			//for this page
			Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
			AjaxFunctions aj = new AjaxFunctions();
			
            if ( !IsPostBack )
            { 
                getStates();

				sql = "SELECT ID, Name FROM bikemakes where isdeleted <> 1 order by name";
				op.FillDropDown( sql, cmbMake, "Name", "ID" );
				ListItem item = new ListItem( "--Select--", "0" );
				cmbMake.Items.Insert( 0, item );
				
		    }

            sql = "SELECT ID, Name, BikeMakeId FROM bikemodels WHERE IsDeleted <> 1 ORDER BY Name";
            string Script = op.GenerateChainScript("cmbMake", "cmbModel", sql, Request["cmbModel"]);
            //RegisterStartupScript( "ChainScript", Script );
            ClientScript.RegisterStartupScript(this.GetType(), "ChainScript", Script);
			
            Trace.Warn("make : ", qryStrMake);
            Trace.Warn("qryStrModel : ", qryStrModel);
            Trace.Warn("qryStrVersion : ", qryStrVersion);
            Trace.Warn("qryStrCity : ", qryStrCity);
							

			if(qryStrMake != "" && qryStrModel != "" && qryStrCity != "")
			{
				if(!IsPostBack)
				{
					BindRepeater(qryStrCity, qryStrModel);
				}
			}
		} // Page_Load
	
		void btnShow_Click( object Sender, EventArgs e )
		{
			BindRepeater("", "");
            BindCityDropDown();
		}
		
		void btnSave_Click( object Sender, EventArgs e )
		{
            BindCityDropDown();
			for ( int i = 0; i < rptPrices.Items.Count; i++ )
			{
				Label ltId = (Label) rptPrices.Items[i].FindControl( "lblVersionId" );
				
				TextBox txtMumbaiPrice 			= (TextBox) rptPrices.Items[i].FindControl( "txtMumbaiPrice" );
				TextBox txtMumbaiInsurance 		= (TextBox) rptPrices.Items[i].FindControl( "txtMumbaiInsurance" );
				TextBox txtMumbaiRTO 			= (TextBox) rptPrices.Items[i].FindControl( "txtMumbaiRTO" );
				TextBox txtMumbaiCorporateRTO 	= (TextBox) rptPrices.Items[i].FindControl( "txtMumbaiCorporateRTO" );
				
				TextBox txtMumbaiMetPrice 			= (TextBox) rptPrices.Items[i].FindControl( "txtMumbaiMetPrice" );
				TextBox txtMumbaiMetInsurance 		= (TextBox) rptPrices.Items[i].FindControl( "txtMumbaiMetInsurance" );
				TextBox txtMumbaiMetRTO 			= (TextBox) rptPrices.Items[i].FindControl( "txtMumbaiMetRTO" );
				TextBox txtMumbaiMetCorporateRTO 	= (TextBox) rptPrices.Items[i].FindControl( "txtMumbaiMetCorporateRTO" );
				
				CheckBox chkUpdate = (CheckBox) rptPrices.Items[i].FindControl("chkUpdate");
				
				if(chkUpdate.Checked)
				{	
					Trace.Warn( "Saving All Prices..." );
					if(ltId.Text.Length > 0 && txtMumbaiPrice.Text.Trim().Length > 0)
						SavePricing( 
							ltId.Text.Trim(), 
							
							txtMumbaiPrice.Text.Trim(), 
							txtMumbaiInsurance.Text.Trim(), 
							txtMumbaiRTO.Text.Trim(),
							txtMumbaiCorporateRTO.Text.Trim(), 
							
							txtMumbaiMetPrice.Text.Trim(), 
							txtMumbaiMetInsurance.Text.Trim(), 
							txtMumbaiMetRTO.Text.Trim(),
							txtMumbaiMetCorporateRTO.Text.Trim()
						);	
				}			
			}
			
			spnError.InnerHtml = "Data saved successfully...";
			BindRepeater("", "");
		}	
		
		
		void btnRemove_Click(object sender, EventArgs e)
		{
            BindCityDropDown();
			string sql = "";
			string verIds = PrepareList();
			
			if(verIds != "")
			{
				//remove all the prices for this city and this model
				try
				{
                    sql = " delete from newbikeshowroomprices where cityid= " + hdnSelectedCityId.Value + " and  bikeversionid in(" + verIds + ")";

					MySqlDatabase.UpdateQuery(sql);
					
				}
				catch(Exception err)
				{
					Trace.Warn(err.Message);
					ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
					objErr.SendMail();
				} // catch Exception
			}
			BindRepeater("", "");
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
		//this function adds the selected ids as comma separated values,
		//and returns it to the calling function 
		private string PrepareList()
		{
			string strRet;		//the concated values to be returned
			strRet = "";		//initializes to blank
			CheckBox objChkControl;
			
			foreach(RepeaterItem item in rptPrices.Items)
			{ 
				objChkControl = (CheckBox)item.FindControl("chkUpdate");
				Label lblVersionId = (Label)item.FindControl("lblVersionId");
							
				if(objChkControl.Checked == true)
				{
					string id = lblVersionId.Text;
					
					//concat the id
					if(strRet == "")
						strRet = id;
					else
						strRet += "," + id ;	
				}
			}	
			return strRet;
		}
		
		void SavePricing(
			string versionId, 
			
			string mumPrice, 
			string mumIns, 
			string mumRTO,
			string mumCorpRTO,
			
			string mumMetPrice, 
			string mumMetIns, 
			string mumMetRTO,
			string mumMetCorpRTO
		)
		{
			
			try
			{
                using (DbCommand cmd = DbFactory.GetDBCommand("insertshowroomprices"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaiprice", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], mumPrice.Length == 0 ? Convert.DBNull : mumPrice));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaiinsurance", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], mumIns.Length == 0 ? Convert.DBNull : mumIns));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbairto", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], mumRTO.Length == 0 ? Convert.DBNull : mumRTO));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaicorporaterto", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], mumCorpRTO.Length == 0 ? Convert.DBNull : mumCorpRTO));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetprice", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], mumMetPrice.Length == 0 ? Convert.DBNull : mumMetPrice));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetinsurance", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], mumMetIns.Length == 0 ? Convert.DBNull : mumMetIns));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetrto", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], mumMetRTO.Length == 0 ? Convert.DBNull : mumMetRTO));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetcorporaterto", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], mumMetCorpRTO.Length == 0 ? Convert.DBNull : mumMetCorpRTO));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], hdnSelectedCityId.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastupdated", DbParamTypeMapper.GetInstance[SqlDbType.DateTime], DateTime.Now));
                    //run the command
                    MySqlDatabase.ExecuteNonQuery(cmd); 
                }				
			}
			catch(SqlException err)
			{
				//catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			
				
			} // catch SqlException
			catch(Exception err)
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		} // SavePricing
		
		///<summary>
		///This function gets the list of the sell inquiries made according to the 
		///model
		///</summary>
		void BindRepeater(string qryCity, string qryModel)
		{
			string sql = "";
			string modelId = "";
            string cityId = hdnSelectedCityId.Value == "0" ? "" : hdnSelectedCityId.Value;

            Trace.Warn("cityId", cityId);
            //drpCity.SelectedValue = cityId;
            //drpCity.Items.FindByValue(cityId).Selected = true;
             
            Trace.Warn("hidden city id :", hdnSelectedCityId.Value);
          
			if( qryCity != "" && qryModel != "")
			{
				cityId = qryCity;
				modelId = qryModel;
			}
			else
			{
				//cityId = drpCity.SelectedItem.Value;
				modelId =  Request["cmbModel"];
			}
      

            //Trace.Warn("city " + cityId + "modelId" + modelId);
			
			if(modelId != null && modelId.Trim() != string.Empty && modelId.Length != 0 && modelId.Trim() != "")
			{			
				sql = @" select ve.ID, ve.Name,
					ncs.price AS MumPrice, ncs.insurance AS MumInsurance, ncs.rto AS MumRTO, ncs.corporaterto AS MumCorporateRTO, 
					ncs.metprice AS MumMetPrice, ncs.metinsurance AS MumMetInsurance, ncs.metrto AS MumMetRTO, ncs.metcorporaterto as MumMetCorporateRTO	
					from (bikeversions ve left join newbikeshowroomprices ncs on ve.id = ncs.bikeversionid and ncs.cityid= " + cityId + ") where ve.isdeleted=0 and ve.bikemodelid=" + modelId;
				
				if (optNew.Checked) sql += " and ve.new=1";
				else  sql += " and ve.new=0";
				
				sql	+= " order by ve.name";
				
				Trace.Warn(sql);
				
				CommonOpn op = new CommonOpn();					
							
				try
				{
					op.BindRepeaterReader( sql, rptPrices );	
				}
				catch(Exception err)
				{
					Trace.Warn(err.Message + err.Source);
					ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"] + sql);
					objErr.SendMail();
				}
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

        protected void BindModelDropDown()
        {
            DataTable dt = null;
            string makeId=cmbMake.SelectedValue;
            try
            {
                MakeModelVersion objMMV = new MakeModelVersion();
                dt = objMMV.GetModels(makeId, "ALL");

            }
            catch(Exception ex)
            {
                Trace.Warn("ShowroomPrices.BindModelDropDown  ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
	} // class
} // namespace