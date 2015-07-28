// C# Document
using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using BikeWaleOpr.Common;
using FreeTextBoxControls;
using Ajax;

namespace BikeWaleOpr.Content
{
	public class PriceMonitoring : Page
	{
		protected DropDownList drpMake, drpModel;
		protected Repeater rptModelVersion, rptCity;
		protected Panel pnlPriceMonitoring;
		protected Button btnShow;
	
		public DataSet dsModelVersions = new DataSet();
		public DataSet dsModelVersionsValues = new DataSet();
		
		public string SelectedModel
		{
			get
			{
				if(Request.Form["drpModel"] != null)
					return Request.Form["drpModel"].ToString();
				else
					return "-1";
			}
		}
		
		public string ModelContents
		{
			get
			{
				if(Request.Form["hdn_drpModel"] != null)
					return Request.Form["hdn_drpModel"].ToString();
				else
					return "";
			}
		}
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnShow.Click += new EventHandler(btnShow_Click);
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			CommonOpn op = new CommonOpn();
			
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/PriceMonitoring.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/PriceMonitoring.aspx");
				
            //int pageId = 38;
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");
				
			// Put user code to initialize the page here
			Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
				
			if ( !IsPostBack )
			{
				FillMakes();
			}
			else
			{
				AjaxFunctions aj = new AjaxFunctions();
				aj.UpdateContents(drpModel, ModelContents, SelectedModel);
			}
		}
		
		void btnShow_Click(object Sender, EventArgs e)
		{
			pnlPriceMonitoring.Visible = true;
			GetData();
		}
		
		void FillMakes()
		{
			CommonOpn op = new CommonOpn();
			string sql;
			sql = "SELECT ID, Name FROM BikeMakes WHERE IsDeleted = 0 AND New = 1 ORDER BY NAME";
			try
			{
				op.FillDropDown( sql, drpMake, "Name", "ID" );
			}
			catch( SqlException err )
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
			
			ListItem item = new ListItem( "--Select--", "0" );
			drpMake.Items.Insert( 0, item );
		}
		
		private void GetData()
		{
			CommonOpn op = new CommonOpn();
			DataSet dsBikeCities = new DataSet();
			Database db = new Database();
			
			string sql = "";
			
			try
			{
				sql = " SELECT CV.ID, (CMO.Name + ' ' + CV.Name) AS BikeName, CMO.Id AS ModelId, CMO.BikeMakeId"
					+ " FROM BikeModels AS CMO, BikeVersions AS CV"
					+ " WHERE CMO.ID = CV.BikeModelId AND CMO.New = 1 AND CV.New = 1"
					+ " AND CMO.IsDeleted <> 1 AND CV.IsDeleted <> 1"
					+ " AND CMO.BikeMakeId = " + drpMake.SelectedItem.Value + "";
					
				if(SelectedModel != "-1" && SelectedModel != "" && SelectedModel != "0")
					sql = sql + " AND CMO.Id = " + SelectedModel + "";
				
				sql = sql + " ORDER BY CV.Name";
				
				Trace.Warn("sqlBikes=" + sql);
				dsModelVersions = db.SelectAdaptQry(sql);
				rptModelVersion.DataSource = dsModelVersions;
				rptModelVersion.DataBind();
				
			
				
				sql = " SELECT CityId, BikeVersionId, LastUpdated"
					+ " FROM NewBikeShowroomPrices NSP, BikeMakes AS CMA, BikeModels AS CMO, BikeVersions AS CV"
					+ " WHERE NSP.BikeVersionId = CV.Id AND CV.BikeModelId = CMO.Id AND CMO.BikeMakeId = CMA.Id"
					+ " AND CMA.New = 1 AND CMO.New = 1 AND CV.New = 1"
					+ " AND CMA.IsDeleted <> 1 AND CMO.IsDeleted <> 1 AND CV.IsDeleted <> 1"
					+ " AND CV.New = 1 AND CMA.Id = " + drpMake.SelectedItem.Value + "";
				
				if(SelectedModel != "-1" && SelectedModel != "" && SelectedModel != "0")
					sql = sql + " AND CMO.Id = " + SelectedModel + "";
					
				sql = sql + " ORDER BY CV.Name";
				
				Trace.Warn("sqlBikeValues=" + sql);
				dsModelVersionsValues = db.SelectAdaptQry(sql);
				
				
				sql = " SELECT DISTINCT C.ID, C.Name AS CityName"
					+ " FROM BWCities AS C, Dealer_NewBike AS DN"
					+ " WHERE DN.CityId = C.Id AND C.IsDeleted <> 1 "
					+ " AND DN.MakeId = " + drpMake.SelectedItem.Value + ""
					+ " ORDER BY CityName";
				
				Trace.Warn("sqlCities=" + sql);
				dsBikeCities = db.SelectAdaptQry(sql);
				rptCity.DataSource = dsBikeCities;
				rptCity.DataBind();
				
	
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}
		
		public DataSet GetVersionPrices(string cityId,string CityName)
		{
			int dayDiff = 0;
			int zone = 0;
			string classValue = "";
			
			//prepare the values in the order of the field master
			DataSet ds = new DataSet();
			DataTable dt = ds.Tables.Add();
			
			dt.Columns.Add("Value", typeof(string));
			dt.Columns.Add("Class", typeof(string));
			dt.Columns.Add("CityId", typeof(string));
			dt.Columns.Add("MakeId", typeof(string));
			dt.Columns.Add("ModelId", typeof(string));
			dt.Columns.Add("VersionId", typeof(string));
            dt.Columns.Add("CityName", typeof(string));
            dt.Columns.Add("BikeName", typeof(string));
        
			
			DataRow dr;
			for(int i = 0; i < dsModelVersions.Tables[0].Rows.Count; i++)
			{	
				DataRow [] rows = dsModelVersionsValues.Tables[0].Select("CityId = '" + cityId + "' AND BikeVersionId = '" + dsModelVersions.Tables[0].Rows[i]["Id"] + "'");
				
				dr = dt.NewRow();
				if(rows.Length == 1)		
				{
					dayDiff = ( DateTime.Now - Convert.ToDateTime(rows[0]["LastUpdated"])).Days;
					
					if( dayDiff > 7 )
					{
						if( dayDiff > 15 )
						{
							if( dayDiff > 30 )
							{
								zone = 4;
							}
							else
							{
								zone = 3;
							}
						}
						else
						{
							zone = 2;
						}
					}
					else
					{
						zone = 1;
					}
					
					switch(zone)
					{
						case 1:
							classValue = "sevendays";
							break;
						case 2:
							classValue = "fifteendays";
							break;
						case 3:
							classValue = "onemonth";
							break;
						case 4:
							classValue = "moreThanMonth";
							break;
						default:
							classValue = "na";
							break;
					}
					 
					dr["Value"] = dayDiff;
					dr["Class"] = classValue;
					dr["CityId"] = cityId;
					dr["MakeId"] = dsModelVersions.Tables[0].Rows[i]["BikeMakeId"];
					dr["ModelId"] = dsModelVersions.Tables[0].Rows[i]["ModelId"];
					dr["VersionId"] = dsModelVersions.Tables[0].Rows[i]["Id"];
                    dr["CityName"] = CityName;
                    dr["BikeName"] = dsModelVersions.Tables[0].Rows[i]["BikeName"]; ;

                  
				}
				else
				{
					dr["Value"] = "NA";
					dr["Class"] = "na";
					dr["CityId"] = cityId;
					dr["MakeId"] = dsModelVersions.Tables[0].Rows[i]["BikeMakeId"];
					dr["ModelId"] = dsModelVersions.Tables[0].Rows[i]["ModelId"];
					dr["VersionId"] = dsModelVersions.Tables[0].Rows[i]["Id"];
                    dr["CityName"] = CityName;
                    dr["BikeName"] = dsModelVersions.Tables[0].Rows[i]["BikeName"]; ;
				}
				dt.Rows.Add(dr);
			}
			return ds;
		}
	
	}//class
}// namespace