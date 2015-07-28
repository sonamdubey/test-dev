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
	public class NewBikeSpecification : Page
	{
		protected HtmlGenericControl spnError;

		protected Button btnSubmit;
		protected Label lblBike;
		
		protected DropDownList cmbFuelType, cmbTransmissionType, cmbDrive;
		
		protected TextBox txtDisplacement, txtCylinders, txtMaxPower, txtMaximumTorque, txtBore, txtStroke, txtValvesPerCylinder, txtFuelDeliverySystem,
                          txtFuelType, txtIgnition, txtSparkPlugsPerCylinder, txtCoolingSystem, txtGearboxType, txtNoOfGears, txtTransmissionType,
                          txtClutch, txtPerformance_0_60_kmph, txtPerformance_0_80_kmph, txtPerformance_0_40_m, txtTopSpeed, txtPerformance_60_0_kmph,
                          txtPerformance_80_0_kmph, txtKerbWeight, txtOverallLength, txtOverallWidth, txtOverallHeight, txtWheelbase, txtGroundClearance,
                          txtSeatHeight, txtFuelTankCapacity, txtReserveFuelCapacity, txtFuelEfficiencyOverall, txtFuelEfficiencyRange, txtChassisType,
                          txtFrontSuspension, txtRearSuspension, txtBrakeType, txtFrontDisc_DrumSize, txtRearDisc_DrumSize, txtCalliperType, txtWheelSize,
                          txtFrontTyre, txtRearTyre, txtElectricSystem, txtBattery, txtHeadlightType, txtHeadlightBulbType, txtBrake_Tail_Light, txtTurnsignal,
                          txtSpeedometer, txtTachometerType, txtNoOfTripmeters, txtTripmeterType, txtColors, txtMaxPowerRpm, txtMaximumTorqueRpm;
        
        protected RadioButton yesFrontDisc, noFrontDisc, yesRearDisc, noRearDisc, yesTubelessTyres, noTubelessTyres, yesRadialTyres, noRadialTyres,
                              yesAlloyWheels, noAlloyWheels, yesPassLight, noPassLight, yesTachometer, noTachometer, yesShiftLight, noShiftLight,
                              yesElectricStart, noElectricStart, yesTripmeter, noTripmeter, yesLowFuelIndicator, noLowFuelIndicator, yesLowOilIndicator,
                              noLowOilIndicator, yesLowBatteryIndicator, noLowBatteryIndicator, yesFuelGauge, noFuelGauge, yesDigitalFuelGauge, noDigitalFuelGauge,
                              yesPillionSeat, noPillionSeat, yesPillionFootrest, noPillionFootrest, yesPillionBackrest, noPillionBackrest, yesPillionGrabrail,
                              noPillionGrabrail, yesStandAlarm, noStandAlarm, yesSteppedSeat, noSteppedSeat, yesAntilockBrakingSystem, noAntilockBrakingSystem,
                              yesKillswitch, noKillswitch, yesClock, noClock, frontDiscNotSure, rearDiscNotSure, tubelessTyresNotSure, radialTyresNotSure,
                              alloyWheelsNotSure, passLightNotSure, tachometerNotSure, shiftLightNotSure, electricStartNotSure, tripmeterNotSure, lowFuelIndicatorNotSure,
                              lowOilIndicatorNotSure, lowBatteryIndicatorNotSure, fuelGaugeNotSure, digitalFuelGaugeNotSure, pillionSeatNotSure, pillionFootrestNotSure,
                              pillionBackrestNotSure, pillionGrabrailNotSure, standAlarmNotSure, steppedSeatNotSure, absNotSure, killswitchNotSure, clockNotSure;
		
		private string _versionId = String.Empty;		
						 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSubmit.Click += new EventHandler( btnSubmit_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            Trace.Warn("yesFrontDisk : ", yesFrontDisc.Checked.ToString());

            if (String.IsNullOrEmpty(Request.QueryString["versionId"]))
            { 
                Response.Redirect("BikeVersions.aspx");
            }

            _versionId = Request.QueryString["versionId"];
            Trace.Warn("version id : ", _versionId.ToString());

			if ( !IsPostBack )
			{
				lblBike.Text = GetBikeName( Request.QueryString["versionId"].ToString() );
				
				FillExistingData();
			}
		} // Page_Load
		
		/// <summary>
		///  Function to fill the existing data on the page for current version id.
		/// </summary>
        void FillExistingData()
		{
            Database db = null;
            DataSet ds = null;
            DataTable dt = null;

			string sql = "SELECT * FROM NewBikeSpecifications WHERE BikeVersionId=" + _versionId;
			Trace.Warn("select sql : ", sql);
			
            try 
            { 
                db = new Database();

                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;

                    ds = db.SelectAdaptQry(cmd);

                    if (ds.Tables[0].Rows.Count > 0)
                    { 
                        dt = ds.Tables[0];

                        txtDisplacement.Text            = dt.Rows[0]["Displacement"].ToString();
                        txtCylinders.Text               = dt.Rows[0]["Cylinders"].ToString();
                        txtMaxPower.Text                = dt.Rows[0]["MaxPower"].ToString();
                        txtMaxPowerRpm.Text             = dt.Rows[0]["MaxPowerRpm"].ToString();
                        txtMaximumTorque.Text           = dt.Rows[0]["MaximumTorque"].ToString();
                        txtMaximumTorqueRpm.Text        = dt.Rows[0]["MaximumTorqueRpm"].ToString();
                        txtBore.Text                    = dt.Rows[0]["Bore"].ToString();
                        txtStroke.Text                  = dt.Rows[0]["Stroke"].ToString();
                        txtValvesPerCylinder.Text       = dt.Rows[0]["ValvesPerCylinder"].ToString();
                        txtFuelDeliverySystem.Text      = dt.Rows[0]["FuelDeliverySystem"].ToString();
                        txtFuelType.Text                = dt.Rows[0]["FuelType"].ToString();
                        txtIgnition.Text                = dt.Rows[0]["Ignition"].ToString();
                        txtSparkPlugsPerCylinder.Text   = dt.Rows[0]["SparkPlugsPerCylinder"].ToString();
                        txtCoolingSystem.Text           = dt.Rows[0]["CoolingSystem"].ToString();
                        txtGearboxType.Text             = dt.Rows[0]["GearboxType"].ToString();
                        txtNoOfGears.Text               = dt.Rows[0]["NoOfGears"].ToString();
                        txtTransmissionType.Text        = dt.Rows[0]["TransmissionType"].ToString();
                        txtClutch.Text                  = dt.Rows[0]["Clutch"].ToString();
                        txtPerformance_0_60_kmph.Text   = dt.Rows[0]["Performance_0_60_kmph"].ToString();
                        txtPerformance_0_80_kmph.Text   = dt.Rows[0]["Performance_0_80_kmph"].ToString();
                        txtPerformance_0_40_m.Text      = dt.Rows[0]["Performance_0_40_m"].ToString();
                        txtTopSpeed.Text                = dt.Rows[0]["TopSpeed"].ToString();
                        txtPerformance_60_0_kmph.Text   = dt.Rows[0]["Performance_60_0_kmph"].ToString();
                        txtPerformance_80_0_kmph.Text   = dt.Rows[0]["Performance_80_0_kmph"].ToString();
                        txtKerbWeight.Text              = dt.Rows[0]["KerbWeight"].ToString();
                        txtOverallLength.Text           = dt.Rows[0]["OverallLength"].ToString();
                        txtOverallWidth.Text            = dt.Rows[0]["OverallWidth"].ToString();
                        txtOverallHeight.Text           = dt.Rows[0]["OverallHeight"].ToString();
                        txtWheelbase.Text               = dt.Rows[0]["Wheelbase"].ToString();
                        txtGroundClearance.Text         = dt.Rows[0]["GroundClearance"].ToString();
                        txtSeatHeight.Text              = dt.Rows[0]["SeatHeight"].ToString();
                        txtFuelTankCapacity.Text        = dt.Rows[0]["FuelTankCapacity"].ToString();
                        txtReserveFuelCapacity.Text     = dt.Rows[0]["ReserveFuelCapacity"].ToString();
                        txtFuelEfficiencyOverall.Text   = dt.Rows[0]["FuelEfficiencyOverall"].ToString();
                        txtFuelEfficiencyRange.Text     = dt.Rows[0]["FuelEfficiencyRange"].ToString();
                        txtChassisType.Text             = dt.Rows[0]["ChassisType"].ToString();
                        txtFrontSuspension.Text         = dt.Rows[0]["FrontSuspension"].ToString();
                        txtRearSuspension.Text          = dt.Rows[0]["RearSuspension"].ToString();
                        txtBrakeType.Text               = dt.Rows[0]["BrakeType"].ToString();
                        
                        if (!Convert.IsDBNull(dt.Rows[0]["FrontDisc"]))
                        {
                            yesFrontDisc.Checked        = Convert.ToBoolean(dt.Rows[0]["FrontDisc"]);
                            noFrontDisc.Checked         = !(yesFrontDisc.Checked);
                            frontDiscNotSure.Checked    = false;
                            Trace.Warn("front disc checked : ", yesFrontDisc.Checked.ToString());
                        }
                        
                        txtFrontDisc_DrumSize.Text      = dt.Rows[0]["FrontDisc_DrumSize"].ToString();
                        
                        if (!Convert.IsDBNull(dt.Rows[0]["RearDisc"]))
                        {
                            yesRearDisc.Checked         = Convert.ToBoolean(dt.Rows[0]["RearDisc"]);
                            noRearDisc.Checked          = !(yesRearDisc.Checked);
                            rearDiscNotSure.Checked     = false;
                            Trace.Warn("rear disc checked : ", yesRearDisc.Checked.ToString());
                        }

                        txtRearDisc_DrumSize.Text       = dt.Rows[0]["RearDisc_DrumSize"].ToString();
                        txtCalliperType.Text            = dt.Rows[0]["CalliperType"].ToString();
                        txtWheelSize.Text               = dt.Rows[0]["WheelSize"].ToString();
                        txtFrontTyre.Text               = dt.Rows[0]["FrontTyre"].ToString();
                        txtRearTyre.Text                = dt.Rows[0]["RearTyre"].ToString();
                        
                        if (!Convert.IsDBNull(dt.Rows[0]["TubelessTyres"]))
                        {
                            yesTubelessTyres.Checked    = Convert.ToBoolean(dt.Rows[0]["TubelessTyres"]);
                            noTubelessTyres.Checked     = !(yesTubelessTyres.Checked);
                            tubelessTyresNotSure.Checked = false;
                            Trace.Warn("yesTubelessTyres checked : ", yesTubelessTyres.Checked.ToString());
                        }
                        
                        if (!Convert.IsDBNull(dt.Rows[0]["RadialTyres"]))
                        {
                            yesRadialTyres.Checked      = Convert.ToBoolean(dt.Rows[0]["RadialTyres"]);
                            noRadialTyres.Checked       = !(yesRadialTyres.Checked);
                            radialTyresNotSure.Checked = false;
                            Trace.Warn("yesRadialTyres checked : ", yesRadialTyres.Checked.ToString());
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["AlloyWheels"]))
                        {
                            yesAlloyWheels.Checked      = Convert.ToBoolean(dt.Rows[0]["AlloyWheels"]);
                            noAlloyWheels.Checked       = !(yesAlloyWheels.Checked);
                            alloyWheelsNotSure.Checked  = false;
                            Trace.Warn("yesAlloyWheels checked : ", yesAlloyWheels.Checked.ToString());
                        }

                        txtElectricSystem.Text          = dt.Rows[0]["ElectricSystem"].ToString();
                        txtBattery.Text                 = dt.Rows[0]["Battery"].ToString();
                        txtHeadlightType.Text           = dt.Rows[0]["HeadlightType"].ToString();
                        txtHeadlightBulbType.Text       = dt.Rows[0]["HeadlightBulbType"].ToString();
                        txtBrake_Tail_Light.Text        = dt.Rows[0]["Brake_Tail_Light"].ToString();
                        txtTurnsignal.Text              = dt.Rows[0]["Turnsignal"].ToString();

                        if (!Convert.IsDBNull(dt.Rows[0]["PassLight"]))
                        {
                            yesPassLight.Checked        = Convert.ToBoolean(dt.Rows[0]["PassLight"]);
                            noPassLight.Checked         = !(yesPassLight.Checked);
                            passLightNotSure.Checked    = false;
                            Trace.Warn("passLight checked : ", yesPassLight.Checked.ToString());
                        } 
                        
                        txtSpeedometer.Text             = dt.Rows[0]["Speedometer"].ToString();
                        if (!Convert.IsDBNull(dt.Rows[0]["Tachometer"]))
                        {
                            yesTachometer.Checked       = Convert.ToBoolean(dt.Rows[0]["Tachometer"]);
                            noTachometer.Checked        = !(yesTachometer.Checked);
                            tachometerNotSure.Checked   = false;
                        }
                        
                        txtTachometerType.Text          = dt.Rows[0]["TachometerType"].ToString();

                        if (!Convert.IsDBNull(dt.Rows[0]["ShiftLight"]))
                        {
                            yesShiftLight.Checked       = Convert.ToBoolean(dt.Rows[0]["ShiftLight"]);
                            noShiftLight.Checked        = !(yesShiftLight.Checked);
                            shiftLightNotSure.Checked   = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["ElectricStart"]))
                        {
                            yesElectricStart.Checked    = Convert.ToBoolean(dt.Rows[0]["ElectricStart"]);
                            noElectricStart.Checked     = !(yesElectricStart.Checked);
                            electricStartNotSure.Checked = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["Tripmeter"]))
                        {
                            yesTripmeter.Checked        = Convert.ToBoolean(dt.Rows[0]["Tripmeter"]);
                            noTripmeter.Checked         = !(yesTripmeter.Checked);
                            tripmeterNotSure.Checked    = false;
                        }
                        
                        txtNoOfTripmeters.Text          = dt.Rows[0]["NoOfTripmeters"].ToString();
                        txtTripmeterType.Text           = dt.Rows[0]["TripmeterType"].ToString();

                        if (!Convert.IsDBNull(dt.Rows[0]["LowFuelIndicator"]))
                        {
                            yesLowFuelIndicator.Checked = Convert.ToBoolean(dt.Rows[0]["LowFuelIndicator"]);
                            noLowFuelIndicator.Checked  = !(yesLowFuelIndicator.Checked);
                            lowFuelIndicatorNotSure.Checked = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["LowOilIndicator"]))
                        {
                            yesLowOilIndicator.Checked = Convert.ToBoolean(dt.Rows[0]["LowOilIndicator"]);
                            noLowOilIndicator.Checked = !(yesLowOilIndicator.Checked);
                            lowOilIndicatorNotSure.Checked = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["LowBatteryIndicator"]))
                        {
                            yesLowBatteryIndicator.Checked = Convert.ToBoolean(dt.Rows[0]["LowBatteryIndicator"]);
                            noLowBatteryIndicator.Checked = !(yesLowBatteryIndicator.Checked);
                            lowBatteryIndicatorNotSure.Checked = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["FuelGauge"]))
                        {
                            yesFuelGauge.Checked = Convert.ToBoolean(dt.Rows[0]["FuelGauge"]);
                            noFuelGauge.Checked = !(yesFuelGauge.Checked);
                            fuelGaugeNotSure.Checked = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["DigitalFuelGauge"]))
                        {
                            yesDigitalFuelGauge.Checked = Convert.ToBoolean(dt.Rows[0]["DigitalFuelGauge"]);
                            noDigitalFuelGauge.Checked = !(yesDigitalFuelGauge.Checked);
                            digitalFuelGaugeNotSure.Checked = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["PillionSeat"]))
                        {
                            yesPillionSeat.Checked = Convert.ToBoolean(dt.Rows[0]["PillionSeat"]);
                            noPillionSeat.Checked = !(yesPillionSeat.Checked);
                            pillionSeatNotSure.Checked = false;
                        }

                        if(!Convert.IsDBNull(dt.Rows[0]["PillionFootrest"]))
                        {
                            yesPillionFootrest.Checked      = Convert.ToBoolean(dt.Rows[0]["PillionFootrest"]);
                            noPillionFootrest.Checked       = !(yesPillionFootrest.Checked);
                            pillionFootrestNotSure.Checked  = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["PillionBackrest"]))
                        {
                            yesPillionBackrest.Checked      = Convert.ToBoolean(dt.Rows[0]["PillionBackrest"]);
                            noPillionBackrest.Checked       = !(yesPillionBackrest.Checked);
                            pillionBackrestNotSure.Checked  = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["PillionGrabrail"]))
                        {
                            yesPillionGrabrail.Checked = Convert.ToBoolean(dt.Rows[0]["PillionGrabrail"]);
                            noPillionGrabrail.Checked = !(yesPillionGrabrail.Checked);
                            pillionGrabrailNotSure.Checked = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["StandAlarm"]))
                        {
                            yesStandAlarm.Checked       = Convert.ToBoolean(dt.Rows[0]["StandAlarm"]);
                            noStandAlarm.Checked        = !(yesStandAlarm.Checked);
                            standAlarmNotSure.Checked   = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["SteppedSeat"]))
                        {
                            yesSteppedSeat.Checked      = Convert.ToBoolean(dt.Rows[0]["SteppedSeat"]);
                            noSteppedSeat.Checked       = !(yesSteppedSeat.Checked);
                            steppedSeatNotSure.Checked  = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["AntilockBrakingSystem"]))
                        {
                            yesAntilockBrakingSystem.Checked = Convert.ToBoolean(dt.Rows[0]["AntilockBrakingSystem"]);
                            noAntilockBrakingSystem.Checked = !(yesAntilockBrakingSystem.Checked);
                            absNotSure.Checked = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["Killswitch"]))
                        {
                            yesKillswitch.Checked = Convert.ToBoolean(dt.Rows[0]["Killswitch"]);
                            noKillswitch.Checked = !(yesKillswitch.Checked);
                            killswitchNotSure.Checked = false;
                        }

                        if (!Convert.IsDBNull(dt.Rows[0]["Clock"]))
                        {
                            yesClock.Checked = Convert.ToBoolean(dt.Rows[0]["Clock"]);
                            noClock.Checked = !(yesClock.Checked);
                            clockNotSure.Checked = false;
                        }
                        txtColors.Text                  = dt.Rows[0]["Colors"].ToString();
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
			finally
			{
                db.CloseConnection();
			}
		}   // End of FillExistingData function

        /// <summary>
        ///      Function will insert the specifications data into newbikespecifications table 
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void btnSubmit_Click(object Sender, EventArgs e)
        {
            Database db = null;

            try
            {
                db = new Database();

                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "InsertNewBikeSpecifications";
                    
                    cmd.Parameters.Add("@BikeVersionId", SqlDbType.SmallInt).Value = _versionId;
                    cmd.Parameters.Add("@Displacement", SqlDbType.Float).Value = txtDisplacement.Text.Trim() == "" ? Convert.DBNull : txtDisplacement.Text.Trim();
                    cmd.Parameters.Add("@Cylinders", SqlDbType.SmallInt).Value = txtCylinders.Text.Trim() == "" ? Convert.DBNull : txtCylinders.Text.Trim();
                    cmd.Parameters.Add("@MaxPower", SqlDbType.Float).Value = txtMaxPower.Text.Trim() == "" ? Convert.DBNull : txtMaxPower.Text.Trim();
                    cmd.Parameters.Add("@MaxPowerRpm", SqlDbType.Int).Value = txtMaxPowerRpm.Text.Trim() == "" ? Convert.DBNull : txtMaxPowerRpm.Text.Trim();
                    cmd.Parameters.Add("@MaximumTorque", SqlDbType.Float).Value = txtMaximumTorque.Text.Trim() == "" ? Convert.DBNull : txtMaximumTorque.Text.Trim();
                    cmd.Parameters.Add("@MaximumTorqueRpm", SqlDbType.Int).Value = txtMaximumTorqueRpm.Text.Trim() == "" ? Convert.DBNull : txtMaximumTorqueRpm.Text.Trim();
                    cmd.Parameters.Add("@Bore", SqlDbType.Float).Value = txtBore.Text.Trim() == "" ? Convert.DBNull : txtBore.Text.Trim();
                    cmd.Parameters.Add("@Stroke", SqlDbType.Float).Value = txtStroke.Text.Trim() == "" ? Convert.DBNull : txtStroke.Text.Trim();
                    cmd.Parameters.Add("@ValvesPerCylinder", SqlDbType.SmallInt).Value = txtValvesPerCylinder.Text.Trim() == "" ? Convert.DBNull : txtValvesPerCylinder.Text.Trim();
                    cmd.Parameters.Add("@FuelDeliverySystem", SqlDbType.VarChar).Value = txtFuelDeliverySystem.Text.Trim() == "" ? Convert.DBNull : txtFuelDeliverySystem.Text.Trim();
                    cmd.Parameters.Add("@FuelType", SqlDbType.VarChar).Value = txtFuelType.Text.Trim() == "" ? Convert.DBNull : txtFuelType.Text.Trim();
                    cmd.Parameters.Add("@Ignition", SqlDbType.VarChar).Value = txtIgnition.Text.Trim() == "" ? Convert.DBNull : txtIgnition.Text.Trim();
                    cmd.Parameters.Add("@SparkPlugsPerCylinder", SqlDbType.VarChar).Value = txtSparkPlugsPerCylinder.Text.Trim();
                    cmd.Parameters.Add("@CoolingSystem", SqlDbType.VarChar).Value = txtCoolingSystem.Text.Trim() == "" ? Convert.DBNull : txtCoolingSystem.Text.Trim();
                    cmd.Parameters.Add("@GearboxType", SqlDbType.VarChar).Value = txtGearboxType.Text.Trim() == "" ? Convert.DBNull : txtGearboxType.Text.Trim();
                    cmd.Parameters.Add("@NoOfGears", SqlDbType.SmallInt).Value = txtNoOfGears.Text.Trim() == "" ? Convert.DBNull : txtNoOfGears.Text.Trim();
                    cmd.Parameters.Add("@TransmissionType", SqlDbType.VarChar).Value = txtTransmissionType.Text.Trim() == "" ? Convert.DBNull : txtTransmissionType.Text.Trim();
                    cmd.Parameters.Add("@Clutch", SqlDbType.VarChar).Value = txtClutch.Text.Trim() == "" ? Convert.DBNull : txtClutch.Text.Trim();
                    cmd.Parameters.Add("@Performance_0_60_kmph", SqlDbType.Float).Value = txtPerformance_0_60_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_0_60_kmph.Text.Trim();
                    cmd.Parameters.Add("@Performance_0_80_kmph", SqlDbType.Float).Value = txtPerformance_0_80_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_0_80_kmph.Text.Trim();
                    cmd.Parameters.Add("@Performance_0_40_m", SqlDbType.Float).Value = txtPerformance_0_40_m.Text.Trim() == "" ? Convert.DBNull : txtPerformance_0_40_m.Text.Trim();
                    //changed topspeed data type from small int to Float
                    //Modified By : Sushil Kumar on 15-07-2015
                    cmd.Parameters.Add("@TopSpeed", SqlDbType.Float).Value = txtTopSpeed.Text.Trim() == "" ? Convert.DBNull : txtTopSpeed.Text.Trim();
                    cmd.Parameters.Add("@Performance_60_0_kmph", SqlDbType.VarChar).Value = txtPerformance_60_0_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_60_0_kmph.Text.Trim();
                    cmd.Parameters.Add("@Performance_80_0_kmph", SqlDbType.VarChar).Value = txtPerformance_80_0_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_80_0_kmph.Text.Trim();
                    cmd.Parameters.Add("@KerbWeight", SqlDbType.SmallInt).Value = txtKerbWeight.Text.Trim() == "" ? Convert.DBNull : txtKerbWeight.Text.Trim();
                    cmd.Parameters.Add("@OverallLength", SqlDbType.SmallInt).Value = txtOverallLength.Text.Trim() == "" ? Convert.DBNull : txtOverallLength.Text.Trim();
                    cmd.Parameters.Add("@OverallWidth", SqlDbType.SmallInt).Value = txtOverallWidth.Text.Trim() == "" ? Convert.DBNull : txtOverallWidth.Text.Trim();
                    cmd.Parameters.Add("@OverallHeight", SqlDbType.SmallInt).Value = txtOverallHeight.Text.Trim() == "" ? Convert.DBNull : txtOverallHeight.Text.Trim();
                    cmd.Parameters.Add("@Wheelbase", SqlDbType.SmallInt).Value = txtWheelbase.Text.Trim() == "" ? Convert.DBNull : txtWheelbase.Text.Trim();
                    cmd.Parameters.Add("@GroundClearance", SqlDbType.SmallInt).Value = txtGroundClearance.Text.Trim() == "" ? Convert.DBNull : txtGroundClearance.Text.Trim();
                    cmd.Parameters.Add("@SeatHeight", SqlDbType.SmallInt).Value = txtSeatHeight.Text.Trim() == "" ? Convert.DBNull : txtSeatHeight.Text.Trim();
                    cmd.Parameters.Add("@FuelTankCapacity", SqlDbType.Float).Value = txtFuelTankCapacity.Text.Trim() == "" ? Convert.DBNull : txtFuelTankCapacity.Text.Trim();
                    cmd.Parameters.Add("@ReserveFuelCapacity", SqlDbType.Float).Value = txtReserveFuelCapacity.Text.Trim() == "" ? Convert.DBNull : txtReserveFuelCapacity.Text.Trim();
                    cmd.Parameters.Add("@FuelEfficiencyOverall", SqlDbType.SmallInt).Value = txtFuelEfficiencyOverall.Text.Trim() == "" ? Convert.DBNull : txtFuelEfficiencyOverall.Text.Trim();
                    cmd.Parameters.Add("@FuelEfficiencyRange", SqlDbType.SmallInt).Value = txtFuelEfficiencyRange.Text.Trim() == "" ? Convert.DBNull : txtFuelEfficiencyRange.Text.Trim();
                    cmd.Parameters.Add("@ChassisType", SqlDbType.VarChar).Value = txtChassisType.Text.Trim() == "" ? Convert.DBNull : txtChassisType.Text.Trim();
                    cmd.Parameters.Add("@FrontSuspension", SqlDbType.VarChar).Value = txtFrontSuspension.Text.Trim() == "" ? Convert.DBNull : txtFrontSuspension.Text.Trim();
                    cmd.Parameters.Add("@RearSuspension", SqlDbType.VarChar).Value = txtRearSuspension.Text.Trim() == "" ? Convert.DBNull : txtRearSuspension.Text.Trim();
                    cmd.Parameters.Add("@BrakeType", SqlDbType.VarChar).Value = txtBrakeType.Text.Trim() == "" ? Convert.DBNull : txtBrakeType.Text.Trim();
                    cmd.Parameters.Add("@FrontDisc", SqlDbType.Bit).Value = frontDiscNotSure.Checked == true ? Convert.DBNull : yesFrontDisc.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@FrontDisc_DrumSize", SqlDbType.SmallInt).Value = txtFrontDisc_DrumSize.Text.Trim() == "" ? Convert.DBNull : txtFrontDisc_DrumSize.Text.Trim();
                    cmd.Parameters.Add("@RearDisc", SqlDbType.Bit).Value = rearDiscNotSure.Checked == true ? Convert.DBNull : yesRearDisc.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@RearDisc_DrumSize", SqlDbType.SmallInt).Value = txtRearDisc_DrumSize.Text.Trim() == "" ? Convert.DBNull : txtRearDisc_DrumSize.Text.Trim();
                    cmd.Parameters.Add("@CalliperType", SqlDbType.VarChar).Value = txtCalliperType.Text.Trim() == "" ? Convert.DBNull : txtCalliperType.Text.Trim();
                    cmd.Parameters.Add("@WheelSize", SqlDbType.Float).Value = txtWheelSize.Text.Trim() == "" ? Convert.DBNull : txtWheelSize.Text.Trim();
                    cmd.Parameters.Add("@FrontTyre", SqlDbType.VarChar).Value = txtFrontTyre.Text.Trim() == "" ? Convert.DBNull : txtFrontTyre.Text.Trim();
                    cmd.Parameters.Add("@RearTyre", SqlDbType.VarChar).Value = txtRearTyre.Text.Trim() == "" ? Convert.DBNull : txtRearTyre.Text.Trim();
                    cmd.Parameters.Add("@TubelessTyres", SqlDbType.Bit).Value = tubelessTyresNotSure.Checked == true ? Convert.DBNull : yesTubelessTyres.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@RadialTyres", SqlDbType.Bit).Value = radialTyresNotSure.Checked == true ? Convert.DBNull : yesRadialTyres.Checked == true ? 1 : 0; 
                    cmd.Parameters.Add("@AlloyWheels", SqlDbType.Bit).Value = alloyWheelsNotSure.Checked == true ? Convert.DBNull : yesAlloyWheels.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@ElectricSystem", SqlDbType.VarChar).Value = txtElectricSystem.Text.Trim() == "" ? Convert.DBNull : txtElectricSystem.Text.Trim();
                    cmd.Parameters.Add("@Battery", SqlDbType.VarChar).Value = txtBattery.Text.Trim() == "" ? Convert.DBNull : txtBattery.Text.Trim();
                    cmd.Parameters.Add("@HeadlightType", SqlDbType.VarChar).Value = txtHeadlightType.Text.Trim() == "" ? Convert.DBNull : txtHeadlightType.Text.Trim();
                    cmd.Parameters.Add("@HeadlightBulbType", SqlDbType.VarChar).Value = txtHeadlightBulbType.Text.Trim() == "" ? Convert.DBNull : txtHeadlightBulbType.Text.Trim();
                    cmd.Parameters.Add("@Brake_Tail_Light", SqlDbType.VarChar).Value = txtBrake_Tail_Light.Text.Trim() == "" ? Convert.DBNull : txtBrake_Tail_Light.Text.Trim();
                    cmd.Parameters.Add("@TurnSignal", SqlDbType.VarChar).Value = txtTurnsignal.Text.Trim() == "" ? Convert.DBNull : txtTurnsignal.Text.Trim();
                    cmd.Parameters.Add("@PassLight", SqlDbType.Bit).Value = passLightNotSure.Checked == true ? Convert.DBNull : yesPassLight.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@Speedometer", SqlDbType.VarChar).Value = txtSpeedometer.Text.Trim() == "" ? Convert.DBNull : txtSpeedometer.Text.Trim();
                    cmd.Parameters.Add("@Tachometer", SqlDbType.Bit).Value = tachometerNotSure.Checked == true ? Convert.DBNull : yesTachometer.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@TachometerType", SqlDbType.VarChar).Value = txtTachometerType.Text.Trim() == "" ? Convert.DBNull : txtTachometerType.Text.Trim();
                    cmd.Parameters.Add("@ShiftLight", SqlDbType.Bit).Value = shiftLightNotSure.Checked == true ? Convert.DBNull : yesShiftLight.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@ElectricStart", SqlDbType.Bit).Value = electricStartNotSure.Checked == true ? Convert.DBNull : yesElectricStart.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@Tripmeter", SqlDbType.Bit).Value = tripmeterNotSure.Checked == true ? Convert.DBNull : yesTripmeter.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@NoOfTripmeters", SqlDbType.VarChar).Value = txtNoOfTripmeters.Text.Trim() == "" ? Convert.DBNull : txtNoOfTripmeters.Text.Trim();
                    cmd.Parameters.Add("@TripmeterType", SqlDbType.VarChar).Value = txtTripmeterType.Text.Trim() == "" ? Convert.DBNull : txtTripmeterType.Text.Trim();
                    cmd.Parameters.Add("@LowFuelIndicator", SqlDbType.Bit).Value = lowFuelIndicatorNotSure.Checked == true ? Convert.DBNull : yesLowFuelIndicator.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@LowOilIndicator", SqlDbType.Bit).Value = lowOilIndicatorNotSure.Checked == true ? Convert.DBNull : yesLowOilIndicator.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@LowBatteryIndicator", SqlDbType.Bit).Value = lowBatteryIndicatorNotSure.Checked ? Convert.DBNull : yesLowBatteryIndicator.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@FuelGauge", SqlDbType.Bit).Value = fuelGaugeNotSure.Checked == true ? Convert.DBNull : yesFuelGauge.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@DigitalFuelGauge", SqlDbType.Bit).Value = digitalFuelGaugeNotSure.Checked == true ? Convert.DBNull : yesDigitalFuelGauge.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@PillionSeat", SqlDbType.Bit).Value = pillionSeatNotSure.Checked == true ? Convert.DBNull : yesPillionSeat.Checked == true ? 1 :0;
                    cmd.Parameters.Add("@PillionFootrest", SqlDbType.Bit).Value = pillionFootrestNotSure.Checked == true ? Convert.DBNull : yesPillionFootrest.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@PillionBackrest", SqlDbType.Bit).Value = pillionBackrestNotSure.Checked == true ? Convert.DBNull : yesPillionBackrest.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@PillionGrabrail", SqlDbType.Bit).Value = pillionGrabrailNotSure.Checked == true ? Convert.DBNull : yesPillionGrabrail.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@StandAlarm", SqlDbType.Bit).Value = standAlarmNotSure.Checked == true ? Convert.DBNull : yesStandAlarm.Checked == true ? 1 :0;
                    cmd.Parameters.Add("@SteppedSeat", SqlDbType.Bit).Value = steppedSeatNotSure.Checked == true ? Convert.DBNull : yesSteppedSeat.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@AntilockBrakingSystem", SqlDbType.Bit).Value = absNotSure.Checked == true ? Convert.DBNull : yesAntilockBrakingSystem.Checked ? 1 : 0;
                    cmd.Parameters.Add("@Killswitch", SqlDbType.Bit).Value = killswitchNotSure.Checked == true ? Convert.DBNull : yesKillswitch.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@Clock", SqlDbType.Bit).Value = clockNotSure.Checked ? Convert.DBNull : yesClock.Checked == true ? 1 : 0;
                    cmd.Parameters.Add("@Colors", SqlDbType.VarChar).Value = txtColors.Text.Trim() == "" ? Convert.DBNull : txtColors.Text.Trim();

                    bool status = db.InsertQry(cmd);

                    if (status)
                    {
                        spnError.InnerText = "Data Saved Successfully.";
                    }
                    else
                    {
                        spnError.InnerText = "Problem occered while saving data";
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn("btnSubmit_Click sql exception : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn("btnSubmit_Click exception : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally 
            { 
                db.CloseConnection();    
            }

        }   // End of btn_Submit_click function

        /// <summary>
        ///     Written By : Ashish G. Kamble on 20/7/2012
        ///     Function to check whether current version exists in the NewBikeSpecifications.
        ///     So procedure will update or insert the record accordinglly.
        /// </summary>
        /// <returns></returns>
        protected bool IsVersionExists()
        {
            Database db = null;
            bool versionExists = false;
            string sqlQuery = "SELECT BikeVersionId FROM NewBikeSpecifications where BikeVersionId = " + _versionId;

            Trace.Warn("isversion exists : ", sqlQuery);

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;

                    DataSet ds = db.SelectAdaptQry(cmd);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        versionExists = true;
                        Trace.Warn("version exists...");
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn("btnSubmit_Click sql exception : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn("btnSubmit_Click exception : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally 
            {
                db.CloseConnection();
            }
            return versionExists;
        } // End of IsVersionExists function
		
		private string GetBikeName( string VersionId )
		{
			string bikeName = "";
			Database db = new Database();
            SqlDataReader dr = null;
			string logoPath = CommonOpn.AppPath + "images/bikeMakes/";
			string sql;
			
			sql = "SELECT ( Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name ) BikeMake"
				+ " FROM BikeMakes Ma, BikeModels Mo, BikeVersions Ve "
				+ " WHERE Ma.ID=Mo.BikeMakeId AND Mo.ID=Ve.BikeModelId "
				+ " AND Ve.Id = " + VersionId;
			
            Trace.Warn("getbikename : ",sql);

			try
			{
				dr = db.SelectQry( sql );
				while ( dr.Read() )
				{
					bikeName = dr[ 0 ].ToString();
				}
				
			}
			catch ( SqlException ex )
			{
				Trace.Warn( ex.Message );
			}
			finally
			{
                if (dr != null)
                {
                    dr.Close();
                }
				db.CloseConnection();
			}
			return bikeName;
		} // GetBikeName
	} // class
} // namespace
