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
			//CommonOpn op = new CommonOpn();
			
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/CarSynopsisStep1.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/CarSynopsisStep1.aspx");
				
            //int pageId = 38;
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");	
			
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
				GetBikeName(qryStrModel);
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
				//if(SaveDescription(saveId))
				//{					
				//}
                //Response.Redirect("CarSynopsisStep1.aspx?msg=Data Saved Successfully");
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
            //string mainDir = "";Commented by Dilip V.
            //string fullPath = "";Commented by Dilip V.
			
			sql = " SELECT * FROM BikeSynopsis WHERE ModelId = "+ modelId +" AND IsActive = 1";
			
			Database db = new Database();
			SqlDataReader dr = null;
			
			Trace.Warn("sql=" + sql);

            try
            {
                dr = db.SelectQry(sql);

                if (dr.Read())
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

                    //string mainDir = CommonOpn.ResolvePhysicalPath("/Contents/Modeldescriptions/");

                    /*Commented by Dilip V.
                    if(HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower().IndexOf( "carwale.com" ) >= 0)
                    {
                        mainDir = CommonOpn.ResolvePhysicalPath("/CarSynopsis/ModelDescriptions/");
                    }
                    else
                    {
                        mainDir = CommonOpn.ResolvePhysicalPath("/Contents/ModelDescription/").Replace("carwale", "carwaleopr");
                    }
				
                    //check whether the directory for the make exists or not, if not then create the directory
                    if(Directory.Exists(mainDir) != false)
                    {
		
                        fullPath = mainDir + "\\" + lblMessage.Text  + ".txt";
                        Trace.Warn("fullPath=" + fullPath);
						
                        if(File.Exists(fullPath) != false)						
                        {
                            ftbDescription.Text	= ReadOtherDetails(fullPath);
                        }
                    }
                    Commented by Dilip V.*/
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
            finally
            {
                if (dr != null)
                    dr.Close();
                db.CloseConnection();
            }
		}
		
		//Function To Get the Bike Name
		void GetBikeName(string modelId)
		{
			string sql = "";
			
			sql = " SELECT (CMA.Name + ' ' + CMO.Name) AS BikeName"
				+ " FROM BikeMakes AS CMA, BikeModels AS CMO"
				+ " WHERE CMA.Id = CMO.BikeMakeId AND CMO.Id = "+ modelId +" AND CMA.IsDeleted = 0";
			
			Database db = new Database();
			SqlDataReader dr = null;
			
			Trace.Warn("sql=" + sql);

            try
            {
                dr = db.SelectQry(sql);

                if (dr.Read())
                {
                    bikeName = dr["BikeName"].ToString();
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
            finally
            { 
                if(dr != null)
                    dr.Close();
                db.CloseConnection();
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
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
			string conStr = db.GetConString();
			string lastSavedId = "";
			
			con = new SqlConnection( conStr );

			try
			{
				Trace.Warn( "Submitting Data" );
				
				cmd = new SqlCommand("CON_AddBikeSynopsis", con);
				cmd.CommandType = CommandType.StoredProcedure;
				
				prm = cmd.Parameters.Add("@Id", SqlDbType.BigInt);
				prm.Value = updateId;
				
				prm = cmd.Parameters.Add("@ModelId", SqlDbType.BigInt);
				prm.Value = qryStrModel;

                prm = cmd.Parameters.Add("@FullDescription", SqlDbType.VarChar);
                prm.Value = ftbDescription.Text.Trim();

				prm = cmd.Parameters.Add("@SmallDescription", SqlDbType.VarChar,8000);
                prm.Value = txtSmallDesc.Text.Trim();
				
				prm = cmd.Parameters.Add("@Pros", SqlDbType.VarChar,500);
                prm.Value = txtPros.Text.Trim();
				
				prm = cmd.Parameters.Add("@Cons", SqlDbType.VarChar,500);
                prm.Value = txtCons.Text.Trim();
				
				prm = cmd.Parameters.Add("@Looks", SqlDbType.SmallInt);
				prm.Value = drpLooks.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@Performance", SqlDbType.SmallInt);
				prm.Value = drpPerformance.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@Fuel", SqlDbType.SmallInt);
				prm.Value = drpFuel.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@Comfort", SqlDbType.SmallInt);
				prm.Value = drpComfort.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@Safety", SqlDbType.SmallInt);
				prm.Value = drpSafety.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@Interiors", SqlDbType.SmallInt);
				prm.Value = drpInteriors.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@Ride", SqlDbType.SmallInt);
				prm.Value = drpRide.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@Handling", SqlDbType.SmallInt);
				prm.Value = drpHandling.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@Braking", SqlDbType.SmallInt);
				prm.Value = drpBraking.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@Overall", SqlDbType.SmallInt);
				prm.Value = drpOverall.SelectedItem.Value;
				
				prm = cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
				prm.Value = 1;
				
				prm = cmd.Parameters.Add("@EntryDateTime", SqlDbType.DateTime);
				prm.Value = DateTime.Now;
				
				prm = cmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime);
				prm.Value = DateTime.Now;
				
				prm = cmd.Parameters.Add("@LastSavedId", SqlDbType.BigInt);
				prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@UserId", SqlDbType.Int);
                prm.Value = CurrentUser.Id;

				con.Open();
    			cmd.ExecuteNonQuery();

                Trace.Warn(cmd.Parameters["@LastSavedId"].Value.ToString());
                Trace.Warn("Current User Id :" + cmd.Parameters["@UserId"].Value.ToString());
				if ( cmd.Parameters["@LastSavedId"].Value.ToString() != "" ) 
					lastSavedId = cmd.Parameters["@LastSavedId"].Value.ToString();
								
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
			finally
			{
				//close the connection	
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
                db.CloseConnection();
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