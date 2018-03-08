using BikewaleOpr.common;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Interface.BikeData;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            Trace.Warn("yesFrontDisk : ", yesFrontDisc.Checked.ToString());

            if (String.IsNullOrEmpty(Request.QueryString["versionId"]))
            {
                Response.Redirect("BikeVersions.aspx");
            }

            _versionId = Request.QueryString["versionId"];
            Trace.Warn("version id : ", _versionId.ToString());

            if (!IsPostBack)
            {
                lblBike.Text = GetBikeName(Request.QueryString["versionId"].ToString());

                FillExistingData();
            }
        } // Page_Load

        /// <summary>
        ///  Function to fill the existing data on the page for current version id.
        /// </summary>
        void FillExistingData()
        {
            DataTable dt = null;


            string sql = string.Empty;
            int _vid = default(int);

            if (!string.IsNullOrEmpty(_versionId) && int.TryParse(_versionId, out _vid))
                sql = "select * from newbikespecifications where bikeversionid=" + _versionId;

            try
            {

                if (string.IsNullOrEmpty(sql)) return;

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            dt = ds.Tables[0];

                            txtDisplacement.Text = dt.Rows[0]["Displacement"].ToString();
                            txtCylinders.Text = dt.Rows[0]["Cylinders"].ToString();
                            txtMaxPower.Text = dt.Rows[0]["MaxPower"].ToString();
                            txtMaxPowerRpm.Text = dt.Rows[0]["MaxPowerRpm"].ToString();
                            txtMaximumTorque.Text = dt.Rows[0]["MaximumTorque"].ToString();
                            txtMaximumTorqueRpm.Text = dt.Rows[0]["MaximumTorqueRpm"].ToString();
                            txtBore.Text = dt.Rows[0]["Bore"].ToString();
                            txtStroke.Text = dt.Rows[0]["Stroke"].ToString();
                            txtValvesPerCylinder.Text = dt.Rows[0]["ValvesPerCylinder"].ToString();
                            txtFuelDeliverySystem.Text = dt.Rows[0]["FuelDeliverySystem"].ToString();
                            txtFuelType.Text = dt.Rows[0]["FuelType"].ToString();
                            txtIgnition.Text = dt.Rows[0]["Ignition"].ToString();
                            txtSparkPlugsPerCylinder.Text = dt.Rows[0]["SparkPlugsPerCylinder"].ToString();
                            txtCoolingSystem.Text = dt.Rows[0]["CoolingSystem"].ToString();
                            txtGearboxType.Text = dt.Rows[0]["GearboxType"].ToString();
                            txtNoOfGears.Text = dt.Rows[0]["NoOfGears"].ToString();
                            txtTransmissionType.Text = dt.Rows[0]["TransmissionType"].ToString();
                            txtClutch.Text = dt.Rows[0]["Clutch"].ToString();
                            txtPerformance_0_60_kmph.Text = dt.Rows[0]["Performance_0_60_kmph"].ToString();
                            txtPerformance_0_80_kmph.Text = dt.Rows[0]["Performance_0_80_kmph"].ToString();
                            txtPerformance_0_40_m.Text = dt.Rows[0]["Performance_0_40_m"].ToString();
                            txtTopSpeed.Text = dt.Rows[0]["TopSpeed"].ToString();
                            txtPerformance_60_0_kmph.Text = dt.Rows[0]["Performance_60_0_kmph"].ToString();
                            txtPerformance_80_0_kmph.Text = dt.Rows[0]["Performance_80_0_kmph"].ToString();
                            txtKerbWeight.Text = dt.Rows[0]["KerbWeight"].ToString();
                            txtOverallLength.Text = dt.Rows[0]["OverallLength"].ToString();
                            txtOverallWidth.Text = dt.Rows[0]["OverallWidth"].ToString();
                            txtOverallHeight.Text = dt.Rows[0]["OverallHeight"].ToString();
                            txtWheelbase.Text = dt.Rows[0]["Wheelbase"].ToString();
                            txtGroundClearance.Text = dt.Rows[0]["GroundClearance"].ToString();
                            txtSeatHeight.Text = dt.Rows[0]["SeatHeight"].ToString();
                            txtFuelTankCapacity.Text = dt.Rows[0]["FuelTankCapacity"].ToString();
                            txtReserveFuelCapacity.Text = dt.Rows[0]["ReserveFuelCapacity"].ToString();
                            txtFuelEfficiencyOverall.Text = dt.Rows[0]["FuelEfficiencyOverall"].ToString();
                            txtFuelEfficiencyRange.Text = dt.Rows[0]["FuelEfficiencyRange"].ToString();
                            txtChassisType.Text = dt.Rows[0]["ChassisType"].ToString();
                            txtFrontSuspension.Text = dt.Rows[0]["FrontSuspension"].ToString();
                            txtRearSuspension.Text = dt.Rows[0]["RearSuspension"].ToString();
                            txtBrakeType.Text = dt.Rows[0]["BrakeType"].ToString();

                            if (!Convert.IsDBNull(dt.Rows[0]["FrontDisc"]))
                            {
                                yesFrontDisc.Checked = Convert.ToBoolean(dt.Rows[0]["FrontDisc"]);
                                noFrontDisc.Checked = !(yesFrontDisc.Checked);
                                frontDiscNotSure.Checked = false;
                                Trace.Warn("front disc checked : ", yesFrontDisc.Checked.ToString());
                            }

                            txtFrontDisc_DrumSize.Text = dt.Rows[0]["FrontDisc_DrumSize"].ToString();

                            if (!Convert.IsDBNull(dt.Rows[0]["RearDisc"]))
                            {
                                yesRearDisc.Checked = Convert.ToBoolean(dt.Rows[0]["RearDisc"]);
                                noRearDisc.Checked = !(yesRearDisc.Checked);
                                rearDiscNotSure.Checked = false;
                                Trace.Warn("rear disc checked : ", yesRearDisc.Checked.ToString());
                            }

                            txtRearDisc_DrumSize.Text = dt.Rows[0]["RearDisc_DrumSize"].ToString();
                            txtCalliperType.Text = dt.Rows[0]["CalliperType"].ToString();
                            txtWheelSize.Text = dt.Rows[0]["WheelSize"].ToString();
                            txtFrontTyre.Text = dt.Rows[0]["FrontTyre"].ToString();
                            txtRearTyre.Text = dt.Rows[0]["RearTyre"].ToString();

                            if (!Convert.IsDBNull(dt.Rows[0]["TubelessTyres"]))
                            {
                                yesTubelessTyres.Checked = Convert.ToBoolean(dt.Rows[0]["TubelessTyres"]);
                                noTubelessTyres.Checked = !(yesTubelessTyres.Checked);
                                tubelessTyresNotSure.Checked = false;
                                Trace.Warn("yesTubelessTyres checked : ", yesTubelessTyres.Checked.ToString());
                            }

                            if (!Convert.IsDBNull(dt.Rows[0]["RadialTyres"]))
                            {
                                yesRadialTyres.Checked = Convert.ToBoolean(dt.Rows[0]["RadialTyres"]);
                                noRadialTyres.Checked = !(yesRadialTyres.Checked);
                                radialTyresNotSure.Checked = false;
                                Trace.Warn("yesRadialTyres checked : ", yesRadialTyres.Checked.ToString());
                            }

                            if (!Convert.IsDBNull(dt.Rows[0]["AlloyWheels"]))
                            {
                                yesAlloyWheels.Checked = Convert.ToBoolean(dt.Rows[0]["AlloyWheels"]);
                                noAlloyWheels.Checked = !(yesAlloyWheels.Checked);
                                alloyWheelsNotSure.Checked = false;
                                Trace.Warn("yesAlloyWheels checked : ", yesAlloyWheels.Checked.ToString());
                            }

                            txtElectricSystem.Text = dt.Rows[0]["ElectricSystem"].ToString();
                            txtBattery.Text = dt.Rows[0]["Battery"].ToString();
                            txtHeadlightType.Text = dt.Rows[0]["HeadlightType"].ToString();
                            txtHeadlightBulbType.Text = dt.Rows[0]["HeadlightBulbType"].ToString();
                            txtBrake_Tail_Light.Text = dt.Rows[0]["Brake_Tail_Light"].ToString();
                            txtTurnsignal.Text = dt.Rows[0]["Turnsignal"].ToString();

                            if (!Convert.IsDBNull(dt.Rows[0]["PassLight"]))
                            {
                                yesPassLight.Checked = Convert.ToBoolean(dt.Rows[0]["PassLight"]);
                                noPassLight.Checked = !(yesPassLight.Checked);
                                passLightNotSure.Checked = false;
                                Trace.Warn("passLight checked : ", yesPassLight.Checked.ToString());
                            }

                            txtSpeedometer.Text = dt.Rows[0]["Speedometer"].ToString();
                            if (!Convert.IsDBNull(dt.Rows[0]["Tachometer"]))
                            {
                                yesTachometer.Checked = Convert.ToBoolean(dt.Rows[0]["Tachometer"]);
                                noTachometer.Checked = !(yesTachometer.Checked);
                                tachometerNotSure.Checked = false;
                            }

                            txtTachometerType.Text = dt.Rows[0]["TachometerType"].ToString();

                            if (!Convert.IsDBNull(dt.Rows[0]["ShiftLight"]))
                            {
                                yesShiftLight.Checked = Convert.ToBoolean(dt.Rows[0]["ShiftLight"]);
                                noShiftLight.Checked = !(yesShiftLight.Checked);
                                shiftLightNotSure.Checked = false;
                            }

                            if (!Convert.IsDBNull(dt.Rows[0]["ElectricStart"]))
                            {
                                yesElectricStart.Checked = Convert.ToBoolean(dt.Rows[0]["ElectricStart"]);
                                noElectricStart.Checked = !(yesElectricStart.Checked);
                                electricStartNotSure.Checked = false;
                            }

                            if (!Convert.IsDBNull(dt.Rows[0]["Tripmeter"]))
                            {
                                yesTripmeter.Checked = Convert.ToBoolean(dt.Rows[0]["Tripmeter"]);
                                noTripmeter.Checked = !(yesTripmeter.Checked);
                                tripmeterNotSure.Checked = false;
                            }

                            txtNoOfTripmeters.Text = dt.Rows[0]["NoOfTripmeters"].ToString();
                            txtTripmeterType.Text = dt.Rows[0]["TripmeterType"].ToString();

                            if (!Convert.IsDBNull(dt.Rows[0]["LowFuelIndicator"]))
                            {
                                yesLowFuelIndicator.Checked = Convert.ToBoolean(dt.Rows[0]["LowFuelIndicator"]);
                                noLowFuelIndicator.Checked = !(yesLowFuelIndicator.Checked);
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

                            if (!Convert.IsDBNull(dt.Rows[0]["PillionFootrest"]))
                            {
                                yesPillionFootrest.Checked = Convert.ToBoolean(dt.Rows[0]["PillionFootrest"]);
                                noPillionFootrest.Checked = !(yesPillionFootrest.Checked);
                                pillionFootrestNotSure.Checked = false;
                            }

                            if (!Convert.IsDBNull(dt.Rows[0]["PillionBackrest"]))
                            {
                                yesPillionBackrest.Checked = Convert.ToBoolean(dt.Rows[0]["PillionBackrest"]);
                                noPillionBackrest.Checked = !(yesPillionBackrest.Checked);
                                pillionBackrestNotSure.Checked = false;
                            }

                            if (!Convert.IsDBNull(dt.Rows[0]["PillionGrabrail"]))
                            {
                                yesPillionGrabrail.Checked = Convert.ToBoolean(dt.Rows[0]["PillionGrabrail"]);
                                noPillionGrabrail.Checked = !(yesPillionGrabrail.Checked);
                                pillionGrabrailNotSure.Checked = false;
                            }

                            if (!Convert.IsDBNull(dt.Rows[0]["StandAlarm"]))
                            {
                                yesStandAlarm.Checked = Convert.ToBoolean(dt.Rows[0]["StandAlarm"]);
                                noStandAlarm.Checked = !(yesStandAlarm.Checked);
                                standAlarmNotSure.Checked = false;
                            }

                            if (!Convert.IsDBNull(dt.Rows[0]["SteppedSeat"]))
                            {
                                yesSteppedSeat.Checked = Convert.ToBoolean(dt.Rows[0]["SteppedSeat"]);
                                noSteppedSeat.Checked = !(yesSteppedSeat.Checked);
                                steppedSeatNotSure.Checked = false;
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
                            txtColors.Text = dt.Rows[0]["Colors"].ToString();
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }

        }   // End of FillExistingData function

	/// <summary>
	///      Function will insert the specifications data into newbikespecifications table 
	/// Modified by : Ashutosh Sharma on 29 Sep 2017 
	/// Description : Changed cache key from 'BW_VersionMinSpecs_' to 'BW_VersionMinSpecs_V1_'.
	/// Modified by:Snehal Dange on 6th Nov 2017
	/// Description: added logic to remove mileage cache "BW_BikesByMileage" key
	/// Modified by : Ashutosh Sharma on 27 Dec 2017
	/// Description : Added call to clear cache for 'BW_SpecsFeatures_version_{versionId}'.
    /// Modified By : Deepak Israni on 8 March 2018
    /// Description : Added method call to push to BWEsDocumentBuilder consumer.
	/// </summary>
	/// <param name="Sender"></param>
	/// <param name="e"></param>
	void btnSubmit_Click(object Sender, EventArgs e)
        {

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "insertnewbikespecifications";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int16, _versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_displacement", DbType.Double, txtDisplacement.Text.Trim() == "" ? Convert.DBNull : txtDisplacement.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cylinders", DbType.Int16, txtCylinders.Text.Trim() == "" ? Convert.DBNull : txtCylinders.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxpower", DbType.Double, txtMaxPower.Text.Trim() == "" ? Convert.DBNull : txtMaxPower.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxpowerrpm", DbType.Int32, txtMaxPowerRpm.Text.Trim() == "" ? Convert.DBNull : txtMaxPowerRpm.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maximumtorque", DbType.Double, txtMaximumTorque.Text.Trim() == "" ? Convert.DBNull : txtMaximumTorque.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maximumtorquerpm", DbType.Int32, txtMaximumTorqueRpm.Text.Trim() == "" ? Convert.DBNull : txtMaximumTorqueRpm.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bore", DbType.Double, txtBore.Text.Trim() == "" ? Convert.DBNull : txtBore.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stroke", DbType.Double, txtStroke.Text.Trim() == "" ? Convert.DBNull : txtStroke.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_valvespercylinder", DbType.Int16, txtValvesPerCylinder.Text.Trim() == "" ? Convert.DBNull : txtValvesPerCylinder.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fueldeliverysystem", DbType.String, txtFuelDeliverySystem.Text.Trim() == "" ? Convert.DBNull : txtFuelDeliverySystem.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fueltype", DbType.String, txtFuelType.Text.Trim() == "" ? Convert.DBNull : txtFuelType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ignition", DbType.String, txtIgnition.Text.Trim() == "" ? Convert.DBNull : txtIgnition.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sparkplugspercylinder", DbType.String, txtSparkPlugsPerCylinder.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_coolingsystem", DbType.String, txtCoolingSystem.Text.Trim() == "" ? Convert.DBNull : txtCoolingSystem.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_gearboxtype", DbType.String, txtGearboxType.Text.Trim() == "" ? Convert.DBNull : txtGearboxType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_noofgears", DbType.Int16, txtNoOfGears.Text.Trim() == "" ? Convert.DBNull : txtNoOfGears.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_transmissiontype", DbType.String, txtTransmissionType.Text.Trim() == "" ? Convert.DBNull : txtTransmissionType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clutch", DbType.String, txtClutch.Text.Trim() == "" ? Convert.DBNull : txtClutch.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance_0_60_kmph", DbType.Double, txtPerformance_0_60_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_0_60_kmph.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance_0_80_kmph", DbType.Double, txtPerformance_0_80_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_0_80_kmph.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance_0_40_m", DbType.Double, txtPerformance_0_40_m.Text.Trim() == "" ? Convert.DBNull : txtPerformance_0_40_m.Text.Trim()));
                    //changed topspeed data type from small int to Float
                    //Modified By : Sushil Kumar on 15-07-2015
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topspeed", DbType.Double, txtTopSpeed.Text.Trim() == "" ? Convert.DBNull : txtTopSpeed.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance_60_0_kmph", DbType.String, txtPerformance_60_0_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_60_0_kmph.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance_80_0_kmph", DbType.String, txtPerformance_80_0_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_80_0_kmph.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_kerbweight", DbType.Int16, txtKerbWeight.Text.Trim() == "" ? Convert.DBNull : txtKerbWeight.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overalllength", DbType.Int16, txtOverallLength.Text.Trim() == "" ? Convert.DBNull : txtOverallLength.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overallwidth", DbType.Int16, txtOverallWidth.Text.Trim() == "" ? Convert.DBNull : txtOverallWidth.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overallheight", DbType.Int16, txtOverallHeight.Text.Trim() == "" ? Convert.DBNull : txtOverallHeight.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_wheelbase", DbType.Int16, txtWheelbase.Text.Trim() == "" ? Convert.DBNull : txtWheelbase.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_groundclearance", DbType.Int16, txtGroundClearance.Text.Trim() == "" ? Convert.DBNull : txtGroundClearance.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seatheight", DbType.Int16, txtSeatHeight.Text.Trim() == "" ? Convert.DBNull : txtSeatHeight.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fueltankcapacity", DbType.Double, txtFuelTankCapacity.Text.Trim() == "" ? Convert.DBNull : txtFuelTankCapacity.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reservefuelcapacity", DbType.Double, txtReserveFuelCapacity.Text.Trim() == "" ? Convert.DBNull : txtReserveFuelCapacity.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fuelefficiencyoverall", DbType.Int16, txtFuelEfficiencyOverall.Text.Trim() == "" ? Convert.DBNull : txtFuelEfficiencyOverall.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fuelefficiencyrange", DbType.Int16, txtFuelEfficiencyRange.Text.Trim() == "" ? Convert.DBNull : txtFuelEfficiencyRange.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_chassistype", DbType.String, txtChassisType.Text.Trim() == "" ? Convert.DBNull : txtChassisType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_frontsuspension", DbType.String, txtFrontSuspension.Text.Trim() == "" ? Convert.DBNull : txtFrontSuspension.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_rearsuspension", DbType.String, txtRearSuspension.Text.Trim() == "" ? Convert.DBNull : txtRearSuspension.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_braketype", DbType.String, txtBrakeType.Text.Trim() == "" ? Convert.DBNull : txtBrakeType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_frontdisc", DbType.Boolean, frontDiscNotSure.Checked == true ? Convert.DBNull : yesFrontDisc.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_frontdisc_drumsize", DbType.Int16, txtFrontDisc_DrumSize.Text.Trim() == "" ? Convert.DBNull : txtFrontDisc_DrumSize.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reardisc", DbType.Boolean, rearDiscNotSure.Checked == true ? Convert.DBNull : yesRearDisc.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reardisc_drumsize", DbType.Int16, txtRearDisc_DrumSize.Text.Trim() == "" ? Convert.DBNull : txtRearDisc_DrumSize.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_callipertype", DbType.String, txtCalliperType.Text.Trim() == "" ? Convert.DBNull : txtCalliperType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_wheelsize", DbType.Double, txtWheelSize.Text.Trim() == "" ? Convert.DBNull : txtWheelSize.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fronttyre", DbType.String, txtFrontTyre.Text.Trim() == "" ? Convert.DBNull : txtFrontTyre.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reartyre", DbType.String, txtRearTyre.Text.Trim() == "" ? Convert.DBNull : txtRearTyre.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tubelesstyres", DbType.Boolean, tubelessTyresNotSure.Checked == true ? Convert.DBNull : yesTubelessTyres.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_radialtyres", DbType.Boolean, radialTyresNotSure.Checked == true ? Convert.DBNull : yesRadialTyres.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_alloywheels", DbType.Boolean, alloyWheelsNotSure.Checked == true ? Convert.DBNull : yesAlloyWheels.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_electricsystem", DbType.String, txtElectricSystem.Text.Trim() == "" ? Convert.DBNull : txtElectricSystem.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_battery", DbType.String, txtBattery.Text.Trim() == "" ? Convert.DBNull : txtBattery.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_headlighttype", DbType.String, txtHeadlightType.Text.Trim() == "" ? Convert.DBNull : txtHeadlightType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_headlightbulbtype", DbType.String, txtHeadlightBulbType.Text.Trim() == "" ? Convert.DBNull : txtHeadlightBulbType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_brake_tail_light", DbType.String, txtBrake_Tail_Light.Text.Trim() == "" ? Convert.DBNull : txtBrake_Tail_Light.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_turnsignal", DbType.String, txtTurnsignal.Text.Trim() == "" ? Convert.DBNull : txtTurnsignal.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_passlight", DbType.Boolean, passLightNotSure.Checked == true ? Convert.DBNull : yesPassLight.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_speedometer", DbType.String, txtSpeedometer.Text.Trim() == "" ? Convert.DBNull : txtSpeedometer.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tachometer", DbType.Boolean, tachometerNotSure.Checked == true ? Convert.DBNull : yesTachometer.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tachometertype", DbType.String, txtTachometerType.Text.Trim() == "" ? Convert.DBNull : txtTachometerType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_shiftlight", DbType.Boolean, shiftLightNotSure.Checked == true ? Convert.DBNull : yesShiftLight.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_electricstart", DbType.Boolean, electricStartNotSure.Checked == true ? Convert.DBNull : yesElectricStart.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tripmeter", DbType.Boolean, tripmeterNotSure.Checked == true ? Convert.DBNull : yesTripmeter.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_nooftripmeters", DbType.String, txtNoOfTripmeters.Text.Trim() == "" ? Convert.DBNull : txtNoOfTripmeters.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tripmetertype", DbType.String, txtTripmeterType.Text.Trim() == "" ? Convert.DBNull : txtTripmeterType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lowfuelindicator", DbType.Boolean, lowFuelIndicatorNotSure.Checked == true ? Convert.DBNull : yesLowFuelIndicator.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lowoilindicator", DbType.Boolean, lowOilIndicatorNotSure.Checked == true ? Convert.DBNull : yesLowOilIndicator.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lowbatteryindicator", DbType.Boolean, lowBatteryIndicatorNotSure.Checked ? Convert.DBNull : yesLowBatteryIndicator.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fuelgauge", DbType.Boolean, fuelGaugeNotSure.Checked == true ? Convert.DBNull : yesFuelGauge.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_digitalfuelgauge", DbType.Boolean, digitalFuelGaugeNotSure.Checked == true ? Convert.DBNull : yesDigitalFuelGauge.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pillionseat", DbType.Boolean, pillionSeatNotSure.Checked == true ? Convert.DBNull : yesPillionSeat.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pillionfootrest", DbType.Boolean, pillionFootrestNotSure.Checked == true ? Convert.DBNull : yesPillionFootrest.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pillionbackrest", DbType.Boolean, pillionBackrestNotSure.Checked == true ? Convert.DBNull : yesPillionBackrest.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pilliongrabrail", DbType.Boolean, pillionGrabrailNotSure.Checked == true ? Convert.DBNull : yesPillionGrabrail.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_standalarm", DbType.Boolean, standAlarmNotSure.Checked == true ? Convert.DBNull : yesStandAlarm.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_steppedseat", DbType.Boolean, steppedSeatNotSure.Checked == true ? Convert.DBNull : yesSteppedSeat.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_antilockbrakingsystem", DbType.Boolean, absNotSure.Checked == true ? Convert.DBNull : yesAntilockBrakingSystem.Checked ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_killswitch", DbType.Boolean, killswitchNotSure.Checked == true ? Convert.DBNull : yesKillswitch.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clock", DbType.Boolean, clockNotSure.Checked ? Convert.DBNull : yesClock.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_colors", DbType.String, txtColors.Text.Trim() == "" ? Convert.DBNull : txtColors.Text.Trim()));

                    bool status = MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);

                    if (status)
                    {
                        spnError.InnerText = "Data Saved Successfully.";
                        
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IBikeModelsRepository, BikeModelsRepository>();
                            container.RegisterType<IBikeModels, BikewaleOpr.BAL.BikeModels>();
                            IBikeModels bikeModels = container.Resolve<IBikeModels>();

                            bikeModels.UpdateModelESIndex(Request.QueryString["modelid"], "update");
                        }
                    }
                    else
                    {
                        spnError.InnerText = "Problem occered while saving data";
                    }

                    bool isNew1;
                    if (Request.QueryString["isNew"].Equals("1"))
                    {
                        isNew1 = true;
                    }
                    else
                    {
                        isNew1 = false;
                    }

                    //Refresh memcache object for bikeVersionSpecs change
                    MemCachedUtility.Remove(string.Format("BW_VersionMinSpecs_V1_{0}_New_{1}", Request.QueryString["modelid"], isNew1));
                    MemCachedUtility.Remove("BW_BikesByMileage");
                    MemCachedUtility.Remove(string.Format("BW_SpecsFeatures_version_{0}", _versionId.Trim()));
                }
            }
            catch (SqlException err)
            {
                Trace.Warn("btnSubmit_Click sql exception : ", err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn("btnSubmit_Click exception : ", err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
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
            bool versionExists = false;
            string sql = string.Empty;
            int _vid = default(int);

            if (!string.IsNullOrEmpty(_versionId) && int.TryParse(_versionId, out _vid))
                sql = "select bikeversionid from newbikespecifications where bikeversionid = " + _vid;

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                                versionExists = true;
                        }

                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn("btnSubmit_Click sql exception : ", err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn("btnSubmit_Click exception : ", err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }

            return versionExists;
        } // End of IsVersionExists function

        private string GetBikeName(string VersionId)
        {
            string bikeName = "";
            string logoPath = CommonOpn.AppPath + "images/bikeMakes/";
            string sql = string.Empty;
            int _vid = default(int);

            if (!string.IsNullOrEmpty(_versionId) && int.TryParse(_versionId, out _vid))
                sql = @"select concat( ve.makename , ' ' , ve.modelname , ' ' , ve.name ) bikemake
                    from bikeversions ve 
                    where ve.id = " + _vid;

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    using (IDataReader dr = MySqlDatabase.SelectQuery(sql, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                bikeName = dr[0].ToString();
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message);
            }
            return bikeName;
        } // GetBikeName
    } // class
} // namespace
