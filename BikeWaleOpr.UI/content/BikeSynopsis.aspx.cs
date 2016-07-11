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
using System.Data.Common;
using BikeWaleOPR.Utilities;
using MySql.CoreDAL;

namespace BikeWaleOpr.Content
{
	public class AddBikeSynopsis : Page
	{
		protected Button btnSave;
		protected FreeTextBox ftbDescription;
        protected Label lblMessage, lbl_success_msg;
		protected TextBox txtPros, txtCons, txtSmallDesc;
		protected DropDownList drpLooks, drpPerformance, drpFuel, drpComfort, drpSafety, drpInteriors, 
					drpRide, drpHandling, drpBraking, drpOverall;
					
		public string qryStrModel = "";
		public string bikeName = "";
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			this.btnSave.Click += new System.EventHandler(btnSave_OnClick);
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			
			if( Request.QueryString["model"] != null && Request.QueryString["model"].ToString() != "")
			{
				qryStrModel = Request.QueryString["model"].ToString();
				if( !CommonOpn.CheckId(qryStrModel) )
				{
					Response.Redirect("bikemodels.aspx");
				}
			}
			else
			{
				Response.Redirect("bikemodels.aspx");
			}

            GetBikeName(qryStrModel);
			
			if ( !IsPostBack )
			{
				FillRatings(drpLooks);
				FillRatings(drpPerformance);
				FillRatings(drpFuel);
				FillRatings(drpComfort);
				FillRatings(drpSafety);
				FillRatings(drpInteriors);
				FillRatings(drpRide);
				FillRatings(drpHandling);
				FillRatings(drpBraking);
				FillRatings(drpOverall);
				
				//Fill Existing data if Exist
				//GetBikeName(qryStrModel);
                FillExistingData(qryStrModel);
			}
         
		}
		
		void btnSave_OnClick( object sender, EventArgs e )
		{
			string saveId = "";
		
			if( lblMessage.Text.Trim()	!= "" )
			{
				saveId = SaveData(lblMessage.Text);
			}
			else
			{
				saveId = SaveData("-1");
			}
			
			if(saveId != "" &&  saveId != "0")
			{

                lbl_success_msg.Text = "Data Saved Successfully";
                lbl_success_msg.Visible = true;
			}
            FillExistingData(Request.QueryString["model"]);
		}
	
		void FillRatings( DropDownList drpName )
		{
			for( int i=10; i>0; i-- )
			{
				drpName.Items.Insert( 0, i.ToString() );
			}
			
			drpName.SelectedValue = "5";
		}
		
		//Fill the data if exist for the current model
		void FillExistingData(string modelId)
		{
			string sql = "";

            int _modelid = default(int);
            if (!string.IsNullOrEmpty(modelId) && int.TryParse(modelId, out _modelid))
            {
                sql = " select * from bikesynopsis where modelid = " + _modelid + " and isactive = 1";

            }

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    using (IDataReader dr = MySqlDatabase.SelectQuery(sql, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            ftbDescription.Text = dr["FullDescription"].ToString();
                            txtSmallDesc.Text = dr["SmallDescription"].ToString();
                            txtPros.Text = dr["Pros"].ToString();
                            txtCons.Text = dr["Cons"].ToString();
                            drpLooks.SelectedValue = dr["Looks"].ToString();
                            drpPerformance.SelectedValue = dr["Performance"].ToString();
                            drpFuel.SelectedValue = dr["FuelEfficiency"].ToString();
                            drpComfort.SelectedValue = dr["Comfort"].ToString();
                            drpSafety.SelectedValue = dr["Safety"].ToString();
                            drpInteriors.SelectedValue = dr["Interiors"].ToString();
                            drpRide.SelectedValue = dr["RideQuality"].ToString();
                            drpHandling.SelectedValue = dr["Handling"].ToString();
                            drpBraking.SelectedValue = dr["Braking"].ToString();
                            drpOverall.SelectedValue = dr["Overall"].ToString();
                            lblMessage.Text = dr["Id"].ToString();
                        }
                    } 
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
		}
		
		//Function To Get the Bike Name
		void GetBikeName(string modelId)
		{
			string sql = "";
            int _modelid = default(int);
            if (!string.IsNullOrEmpty(modelId) && int.TryParse(modelId, out _modelid))
            {
                sql = @" select concat(cma.name,' ',cmo.name) as bikename
				 from bikemakes as cma, bikemodels as cmo
				 where cma.id = cmo.bikemakeid and cmo.id = " + _modelid + " and cma.isdeleted = 0";
                
            }
            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    using (IDataReader dr = MySqlDatabase.SelectQuery(sql, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            bikeName = dr["BikeName"].ToString();
                        }
                    } 
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
		}
		
		//Function to read features and descriptions from the saved text file
		string ReadOtherDetails(string filePath)
		{
			string strRead = "";
			
			StreamReader sr = new StreamReader(filePath);
			strRead = sr.ReadToEnd();
			sr.Close();
			
			return strRead;
		}
		
		string SaveData( string updateId )
		{
			string lastSavedId = "";

			try
			{
                using (DbCommand cmd = DbFactory.GetDBCommand("con_addbikesynopsis"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], updateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], qryStrModel));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fulldescription", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], ftbDescription.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_smalldescription", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 8000, txtSmallDesc.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pros", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 500, txtPros.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cons", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 500, txtCons.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_looks", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], drpLooks.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], drpPerformance.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fuel", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], drpFuel.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comfort", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], drpComfort.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_safety", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], drpSafety.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_interiors", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], drpInteriors.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ride", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], drpRide.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_handling", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], drpHandling.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_braking", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], drpBraking.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overall", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], drpOverall.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbParamTypeMapper.GetInstance[SqlDbType.Bit], 1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_entrydatetime", DbParamTypeMapper.GetInstance[SqlDbType.DateTime], DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastupdated", DbParamTypeMapper.GetInstance[SqlDbType.DateTime], DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastsavedid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.Int], CurrentUser.Id));


                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    if (cmd.Parameters["par_lastsavedid"].Value.ToString() != "")
                        lastSavedId = cmd.Parameters["par_lastsavedid"].Value.ToString(); 
                }
								
			}
			catch(SqlException err)
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			} 

			return lastSavedId;
		}
		
		//Function to save description in the seperate file for each new inserted part.
		bool SaveDescription(string itemId)
		{
			bool isSaved = false;
			string fullPath = "";
			string mainDir = "";
			
			try
			{
				//string mainDir = CommonOpn.ResolvePhysicalPath("/Contents/Modeldescriptions/");
				if(HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower().IndexOf( "carwale.com" ) >= 0)
				{
					mainDir = CommonOpn.ResolvePhysicalPath("/CarSynopsis/ModelDescriptions/");
				}
                else if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower().IndexOf("localhost") >= 0)
                {
                    mainDir = CommonOpn.ResolvePhysicalPath("/content/ModelDescription/");
                }
                else
				{
					mainDir = CommonOpn.ResolvePhysicalPath("/content/ModelDescription/").Replace("carwale", "bikewaleopr");
				}
			
				//check whether the directory for the make exists or not, if not then create the directory
				if(Directory.Exists(mainDir) == false)
					Directory.CreateDirectory(mainDir);
										
				//create file to store description
				fullPath = mainDir + "\\" + itemId + ".txt";
			
				Trace.Warn(fullPath);
				StreamWriter sw = File.CreateText(fullPath);
				sw.Write(ftbDescription.Text.Trim());
				sw.Flush();
				sw.Close();
				isSaved = true;
			}
			catch(Exception err)
			{
				//catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
				isSaved = false;
			} // catch Exception
			
			return isSaved;
		}
	
	}//class
}// namespace