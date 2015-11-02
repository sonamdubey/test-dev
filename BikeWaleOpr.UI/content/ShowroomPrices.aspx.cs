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

namespace BikeWaleOpr.Content
{
	public class ShowroomPrices : Page
	{
		protected HtmlGenericControl spnError;
		protected Button btnSave, btnShow, btnRemove;
		protected Repeater rptPrices;
        protected DropDownList cmbMake, ddlStates;
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

            //drpCity_Id = drpCity.ClientID.ToString();
            //hdn_SelectedState_Id = hdn_SelectedState.ClientID.ToString();

            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/ShowroomPrices.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/ShowroomPrices.aspx");
				
            //int pageId = 38;
            //int pageId1 = 41;
            //if ( op.verifyPrivilege( pageId ) == false && op.verifyPrivilege( pageId1 ) == false )
            //    Response.Redirect("../NotAuthorized.aspx");
				
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

				sql = "SELECT ID, Name FROM BikeMakes WHERE IsDeleted <> 1 ORDER BY NAME";
				op.FillDropDown( sql, cmbMake, "Name", "ID" );
				ListItem item = new ListItem( "--Select--", "0" );
				cmbMake.Items.Insert( 0, item );
				
				//string cityIds = "1, 2, 3, 4, 5, 7, 9, 10, 12, 14, 15, 17, 22, 28, 30, 31, 35, 37, 40, 52, 57, 67, 73, 85, 88, 100, 105, 112, 117, 119, 125, 128, 133, 141, 143, 145, 147, 148, 152, 153, 160, 165, 166, 174, 176, 177, 184, 191, 194, 198, 201, 204, 206, 207, 208, 209, 210, 211, 212, 214, 215, 219, 220, 221, 222, 224, 225, 226, 227, 231, 232, 233, 234, 236, 237, 240, 243, 244, 246, 250, 253, 255, 256, 258, 259, 260, 261, 262, 264, 266, 267, 271, 273, 274, 275, 277, 280, 281, 282, 285, 287, 290, 295, 296, 302, 309, 330, 342, 348, 349, 363, 394, 481, 586";
				//" + cityIds + "
                //sql = "SELECT ID, Name FROM Cities ORDER BY NAME";
                //op.FillDropDown( sql, drpCity, "Name", "ID" );
                //drpCity.Items.Insert( 0, new ListItem( "--Select City--", "0" ));
				//drpCity.Items.Insert( 1, new ListItem( "Mumbai", "1" ));
		    }
			
			sql = "SELECT ID, Name, BikeMakeId FROM BikeModels WHERE IsDeleted <> 1 ORDER BY Name";
			string Script = op.GenerateChainScript( "cmbMake", "cmbModel", sql, "Select Model" );
			//RegisterStartupScript( "ChainScript", Script );
			ClientScript.RegisterStartupScript(this.GetType(), "ChainScript", Script );


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
						
						//Update Changes Log
                        //if(ltId.Text.Trim() != "")
                        //{
                        //    ContentCommon cc = new ContentCommon();
                        //    cc.LogUpdates("Showroom Prices Add/Update", "VersionId", ltId.Text.Trim());
                        //}
				}			
			}
			
			spnError.InnerHtml = "Data saved successfully...";
			BindRepeater("", "");
		}	
		
		
		void btnRemove_Click(object sender, EventArgs e)
		{
            BindCityDropDown();
			string sql = "";
			Database db = new Database();
			string verIds = PrepareList();
			
			if(verIds != "")
			{
				//remove all the prices for this city and this model
				try
				{
                    sql = " DELETE From NewBikeShowroomPrices WHERE CityId= " + hdnSelectedCityId.Value + " AND "
						+ " BikeVersionId IN(" + verIds + ")";
					
					Trace.Warn(sql);
					db.DeleteQry(sql);
					
					//Update Changes Log
                    //ContentCommon cc = new ContentCommon();
                    //cc.LogUpdates("Showroom Prices Deleted", "VersionId", verIds);	
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
			Trace.Warn("DELETE Ids ....................." + strRet);
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
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
			CommonOpn op = new CommonOpn();
			
			string conStr = db.GetConString();
			
			con = new SqlConnection( conStr );
			
			try
			{
                Trace.Warn("pricing city id", drpCity.SelectedItem.Value);
				Trace.Warn( "Submitting Data" );
				cmd = new SqlCommand("InsertShowroomPrices", con);
				cmd.CommandType = CommandType.StoredProcedure;
				
				prm = cmd.Parameters.Add("@BikeVersionId", SqlDbType.BigInt);
				prm.Value = versionId;
				
				//Non-Metalic price
				prm = cmd.Parameters.Add("@MumbaiPrice", SqlDbType.BigInt);
				prm.Value = mumPrice.Length == 0 ? Convert.DBNull : mumPrice;
				
				prm = cmd.Parameters.Add("@MumbaiInsurance", SqlDbType.BigInt);
				prm.Value = mumIns.Length == 0 ? Convert.DBNull : mumIns;
				
				prm = cmd.Parameters.Add("@MumbaiRTO", SqlDbType.BigInt);
				prm.Value = mumRTO.Length == 0 ? Convert.DBNull : mumRTO;
				
				prm = cmd.Parameters.Add("@MumbaiCorporateRTO", SqlDbType.BigInt);
				prm.Value = mumCorpRTO.Length == 0 ? Convert.DBNull : mumCorpRTO;
				
				//Metalic price
				prm = cmd.Parameters.Add("@MumbaiMetPrice", SqlDbType.BigInt);
				prm.Value = mumMetPrice.Length == 0 ? Convert.DBNull : mumMetPrice;
				
				prm = cmd.Parameters.Add("@MumbaiMetInsurance", SqlDbType.BigInt);
				prm.Value = mumMetIns.Length == 0 ? Convert.DBNull : mumMetIns;
				
				prm = cmd.Parameters.Add("@MumbaiMetRTO", SqlDbType.BigInt);
				prm.Value = mumMetRTO.Length == 0 ? Convert.DBNull : mumMetRTO;
				
				prm = cmd.Parameters.Add("@MumbaiMetCorporateRTO", SqlDbType.BigInt);
				prm.Value = mumMetCorpRTO.Length == 0 ? Convert.DBNull : mumMetCorpRTO;
				
				prm = cmd.Parameters.Add("@CityId", SqlDbType.BigInt);
                prm.Value = hdnSelectedCityId.Value;
                //prm.Value = drpCity.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime);
				prm.Value = DateTime.Now;
				
				con.Open();
				//run the command
    			cmd.ExecuteNonQuery();				
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
			finally
			{
				//close the connection	
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
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
				sql = " SELECT VE.ID, VE.Name,"
					+ " NCS.Price AS MumPrice, NCS.Insurance AS MumInsurance, NCS.RTO AS MumRTO, NCS.CorporateRTO AS MumCorporateRTO, "
					+ " NCS.MetPrice AS MumMetPrice, NCS.MetInsurance AS MumMetInsurance, NCS.MetRTO AS MumMetRTO, NCS.MetCorporateRTO AS MumMetCorporateRTO"
	
					+ " FROM (BikeVersions Ve LEFT JOIN NewBikeShowroomPrices NCS ON VE.ID = NCS.BikeVersionId AND NCS.CityId= " + cityId + ")"
					+ " WHERE VE.IsDeleted=0 AND Ve.BikeModelId=" + modelId;
				
				if (optNew.Checked) sql += " AND Ve.New=1";
				else  sql += " AND Ve.New=0";
				
				sql	+= " ORDER BY VE.Name";
				
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
                    //ddlStates.Items[0].Attributes.Add("disabled", "disabled");
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