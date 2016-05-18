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

namespace BikeWaleOpr.Content
{
	public class NewBikeStandardFeaturesStep2 : Page
	{
		protected HtmlGenericControl spnError;
		protected Button btnSave;
		protected Label lblBike;
		protected DropDownList cmbAC, cmbPW, cmbPowerDoorLocks, cmbPS,
					cmbABS, cmbSteeringAdjustment, cmbTacho,
					cmbChildSafetyLocks, cmbFogLights, cmbDefroster,
					cmbDefogger, cmbSeats, cmbPowerSeats, cmbRadio,
					cmbCassettePlayer, cmbCD, cmbSun, cmbMoon;
		
		// Added 17th Feb
		protected DropDownList cmbTractionControl, cmbImmobilizer,
					cmbDriverAirBags, cmbPassengerAirBags, cmbRemoteBootFuelLid,
					cmbCupHolder, cmbSplitFoldingRearSeats, cmbRearWashWiper,
					cmbCentralLocking, cmbAlloyWheels, cmbTubelessTyres;
										 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            //CommonOpn op = new CommonOpn();
			
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/NewCarStandardFeaturesStep2.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/NewCarStandardFeaturesStep2.aspx");
				
            //int pageId = 38;
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");
				
				
			if ( Request.QueryString["versionId"] == null ) return;		
			
			if ( !IsPostBack )
			{
				lblBike.Text = GetBikeName( Request.QueryString["versionId"].ToString() );
				
				FillExistingData();
			}
			
		} // Page_Load
				
		void btnSave_Click( object Sender, EventArgs e )
		{
            throw new Exception("Method not used/commented");


            //int versionId = 0;
			
            //versionId = int.Parse( Request.QueryString["versionId"] );
			
            //string sql = "";
			
            //sql = " INSERT INTO NewBikeStandardFeatures( BikeVersionId, PowerWindows, PowerDoorLocks, "
            //    + " PowerSteering, AirConditioner, ABS, SteeringAdjustment, Tachometer, ChildSafetyLocks, "
            //    + " FrontFogLights, RearDefroster, Defogger, LeatherSeats, PowerSeats, "
            //    + " Radio, CassettePlayer, CDPlayer, SunRoof, MoonRoof,TractionControl, "
            //    + " Immobilizer, DriverAirBags, PassengerAirBags, RemoteBootFuelLid, "
            //    + " CupHolder, SplitFoldingRearSeats, RearWashWiper, CentralLocking, "
            //    + " AlloyWheels, TubelessTyres ) "
            //    + " VALUES(" + versionId + ", '" 
            //    + cmbPW.SelectedValue + "','"
            //    + cmbPowerDoorLocks.SelectedValue + "','"
            //    + cmbPS.SelectedValue + "','"
            //    + cmbAC.SelectedValue + "','"
            //    + cmbABS.SelectedValue + "','"
            //    + cmbSteeringAdjustment.SelectedValue + "','"
            //    + cmbTacho.SelectedValue + "','"
            //    + cmbChildSafetyLocks.SelectedValue + "','"
            //    + cmbFogLights.SelectedValue + "','"
            //    + cmbDefroster.SelectedValue + "','"
            //    + cmbDefogger.SelectedValue + "','"
            //    + cmbSeats.SelectedValue + "','"
            //    + cmbPowerSeats.SelectedValue + "','"
            //    + cmbRadio.SelectedValue + "','"
            //    + cmbCassettePlayer.SelectedValue + "','"
            //    + cmbCD.SelectedValue + "','"
            //    + cmbSun.SelectedValue + "','"
            //    + cmbMoon.SelectedValue + "','"
            //    + cmbTractionControl.SelectedValue + "','"
            //    + cmbImmobilizer.SelectedValue  + "','"
            //    + cmbDriverAirBags.SelectedValue + "','"
            //    + cmbPassengerAirBags.SelectedValue + "','" 
            //    + cmbRemoteBootFuelLid.SelectedValue + "','"
            //    + cmbCupHolder.SelectedValue + "','" 			
            //    + cmbSplitFoldingRearSeats.SelectedValue + "','"
            //    + cmbRearWashWiper.SelectedValue + "','" 			
            //    + cmbCentralLocking.SelectedValue + "','" 		
            //    + cmbAlloyWheels.SelectedValue + "','" 			
            //    + cmbTubelessTyres.SelectedValue + "' )";
							
            //Database db = new Database();
			
            //try
            //{	
            //    string sql1 = "DELETE FROM NewBikeStandardFeatures WHERE BikeVersionId=" + versionId ;

            //    Trace.Warn("sql 1 : ", sql1);
            //    Trace.Warn("sql : ", sql);

            //    db.InsertQry( sql1 );
            //    db.InsertQry( sql );
				
            //    spnError.InnerText = "Data saved successfully.";
            //    spnError.Visible = true;
            //}
            //catch ( SqlException err )
            //{
            //    Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
		}
		
		void FillExistingData()
		{
            throw new Exception("Method not used/commented");

            //int versionId;
			
            //if ( Request["fromId"] != null ) versionId = Convert.ToInt32( Request["fromId"] );
            //else versionId = Convert.ToInt32( Request["versionId"] );
			
            //string sql;
            //sql = "SELECT * FROM NewBikeStandardFeatures WHERE BikeVersionId=" + versionId;
			
            //Database db = new Database();
            //SqlDataReader reader = null;
			
            //try
            //{
            //    reader = db.SelectQry( sql );
            //    while( reader.Read() )
            //    {
            //        cmbPW.SelectedValue					= reader["PowerWindows"].ToString();
            //        cmbPowerDoorLocks.SelectedValue		= reader["PowerDoorLocks"].ToString();
            //        cmbPS.SelectedValue					= reader["PowerSteering"].ToString();
            //        cmbAC.SelectedValue					= reader["AirConditioner"].ToString();
            //        cmbABS.SelectedValue				= reader["ABS"].ToString();
            //        cmbSteeringAdjustment.SelectedValue	= reader["SteeringAdjustment"].ToString();
            //        cmbTacho.SelectedValue				= reader["Tachometer"].ToString();	
            //        cmbChildSafetyLocks.SelectedValue	= reader["ChildSafetyLocks"].ToString();
            //        cmbFogLights.SelectedValue			= reader["FrontFogLights"].ToString();
            //        cmbDefroster.SelectedValue			= reader["RearDefroster"].ToString();
            //        cmbDefogger.SelectedValue			= reader["Defogger"].ToString();
            //        cmbSeats.SelectedValue				= reader["LeatherSeats"].ToString();
            //        cmbPowerSeats.SelectedValue			= reader["PowerSeats"].ToString();
            //        cmbRadio.SelectedValue				= reader["Radio"].ToString();
            //        cmbCassettePlayer.SelectedValue		= reader["CassettePlayer"].ToString();	
            //        cmbCD.SelectedValue 				= reader["CDPlayer"].ToString();	
            //        cmbSun.SelectedValue				= reader["SunRoof"].ToString();	
            //        cmbMoon.SelectedValue 				= reader["MoonRoof"].ToString();	
					
            //        // Added 17th Feb
            //        cmbTractionControl.SelectedValue 			= reader["TractionControl"].ToString();	
            //        cmbImmobilizer.SelectedValue 				= reader["Immobilizer"].ToString();	
            //        cmbDriverAirBags.SelectedValue 				= reader["DriverAirBags"].ToString();	
            //        cmbPassengerAirBags.SelectedValue 			= reader["PassengerAirBags"].ToString();	
            //        cmbRemoteBootFuelLid.SelectedValue 			= reader["RemoteBootFuelLid"].ToString();	
            //        cmbCupHolder.SelectedValue 					= reader["CupHolder"].ToString();	
            //        cmbSplitFoldingRearSeats.SelectedValue 		= reader["SplitFoldingRearSeats"].ToString();	
            //        cmbRearWashWiper.SelectedValue 				= reader["RearWashWiper"].ToString();	
            //        cmbCentralLocking.SelectedValue 			= reader["CentralLocking"].ToString();	
            //        cmbAlloyWheels.SelectedValue 				= reader["AlloyWheels"].ToString();	
            //        cmbTubelessTyres.SelectedValue 				= reader["TubelessTyres"].ToString();	
            //        //17th feb additions finish here.
            //    }
            //}
            //catch ( SqlException err )
            //{
            //    Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if(reader != null)
            //    {
            //        reader.Close();
            //    }
            //    db.CloseConnection();
            //}
		}
		
		private string GetBikeName( string VersionId )
		{
            throw new Exception("Method not used/commented");

            //string bikeName = "";
            //Database db = new Database();
            //SqlDataReader dr = null;
            ////string logoPath = CommonOpn.AppPath + "images/bikeMakes/";
            //string sql;
			
            //sql = "SELECT ( Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name ) BikeMake"
            //    + " FROM BikeMakes Ma, BikeModels Mo, BikeVersions Ve "
            //    + " WHERE Ma.ID=Mo.BikeMakeId AND Mo.ID=Ve.BikeModelId "
            //    + " AND Ve.Id = " + VersionId;
			
            //try
            //{
            //    dr = db.SelectQry( sql );
            //    while ( dr.Read() )
            //    {
            //        bikeName = dr[ 0 ].ToString();
            //    }
				
            //}
            //catch ( SqlException ex )
            //{
            //    Trace.Warn( ex.Message );
            //}
            //finally
            //{
            //    if(dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
            //return bikeName;
		} // GetBikeName		
	} // class
} // namespace