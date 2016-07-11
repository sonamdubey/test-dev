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
using BikeWaleOPR.Utilities;
using System.Data.Common;
using MySql.CoreDAL;

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
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }   // End of FillExistingData function

        /// <summary>
        ///      Function will insert the specifications data into newbikespecifications table 
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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], _versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_displacement", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtDisplacement.Text.Trim() == "" ? Convert.DBNull : txtDisplacement.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cylinders", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtCylinders.Text.Trim() == "" ? Convert.DBNull : txtCylinders.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxpower", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtMaxPower.Text.Trim() == "" ? Convert.DBNull : txtMaxPower.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxpowerrpm", DbParamTypeMapper.GetInstance[SqlDbType.Int], txtMaxPowerRpm.Text.Trim() == "" ? Convert.DBNull : txtMaxPowerRpm.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maximumtorque", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtMaximumTorque.Text.Trim() == "" ? Convert.DBNull : txtMaximumTorque.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maximumtorquerpm", DbParamTypeMapper.GetInstance[SqlDbType.Int], txtMaximumTorqueRpm.Text.Trim() == "" ? Convert.DBNull : txtMaximumTorqueRpm.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bore", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtBore.Text.Trim() == "" ? Convert.DBNull : txtBore.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stroke", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtStroke.Text.Trim() == "" ? Convert.DBNull : txtStroke.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_valvespercylinder", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtValvesPerCylinder.Text.Trim() == "" ? Convert.DBNull : txtValvesPerCylinder.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fueldeliverysystem", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtFuelDeliverySystem.Text.Trim() == "" ? Convert.DBNull : txtFuelDeliverySystem.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fueltype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtFuelType.Text.Trim() == "" ? Convert.DBNull : txtFuelType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ignition", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtIgnition.Text.Trim() == "" ? Convert.DBNull : txtIgnition.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sparkplugspercylinder", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtSparkPlugsPerCylinder.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_coolingsystem", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtCoolingSystem.Text.Trim() == "" ? Convert.DBNull : txtCoolingSystem.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_gearboxtype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtGearboxType.Text.Trim() == "" ? Convert.DBNull : txtGearboxType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_noofgears", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtNoOfGears.Text.Trim() == "" ? Convert.DBNull : txtNoOfGears.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_transmissiontype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtTransmissionType.Text.Trim() == "" ? Convert.DBNull : txtTransmissionType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clutch", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtClutch.Text.Trim() == "" ? Convert.DBNull : txtClutch.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance_0_60_kmph", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtPerformance_0_60_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_0_60_kmph.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance_0_80_kmph", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtPerformance_0_80_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_0_80_kmph.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance_0_40_m", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtPerformance_0_40_m.Text.Trim() == "" ? Convert.DBNull : txtPerformance_0_40_m.Text.Trim()));
                    //changed topspeed data type from small int to Float
                    //Modified By : Sushil Kumar on 15-07-2015
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topspeed", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtTopSpeed.Text.Trim() == "" ? Convert.DBNull : txtTopSpeed.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance_60_0_kmph", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtPerformance_60_0_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_60_0_kmph.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performance_80_0_kmph", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtPerformance_80_0_kmph.Text.Trim() == "" ? Convert.DBNull : txtPerformance_80_0_kmph.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_kerbweight", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtKerbWeight.Text.Trim() == "" ? Convert.DBNull : txtKerbWeight.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overalllength", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtOverallLength.Text.Trim() == "" ? Convert.DBNull : txtOverallLength.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overallwidth", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtOverallWidth.Text.Trim() == "" ? Convert.DBNull : txtOverallWidth.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overallheight", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtOverallHeight.Text.Trim() == "" ? Convert.DBNull : txtOverallHeight.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_wheelbase", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtWheelbase.Text.Trim() == "" ? Convert.DBNull : txtWheelbase.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_groundclearance", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtGroundClearance.Text.Trim() == "" ? Convert.DBNull : txtGroundClearance.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seatheight", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtSeatHeight.Text.Trim() == "" ? Convert.DBNull : txtSeatHeight.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fueltankcapacity", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtFuelTankCapacity.Text.Trim() == "" ? Convert.DBNull : txtFuelTankCapacity.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reservefuelcapacity", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtReserveFuelCapacity.Text.Trim() == "" ? Convert.DBNull : txtReserveFuelCapacity.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fuelefficiencyoverall", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtFuelEfficiencyOverall.Text.Trim() == "" ? Convert.DBNull : txtFuelEfficiencyOverall.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fuelefficiencyrange", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtFuelEfficiencyRange.Text.Trim() == "" ? Convert.DBNull : txtFuelEfficiencyRange.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_chassistype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtChassisType.Text.Trim() == "" ? Convert.DBNull : txtChassisType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_frontsuspension", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtFrontSuspension.Text.Trim() == "" ? Convert.DBNull : txtFrontSuspension.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_rearsuspension", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtRearSuspension.Text.Trim() == "" ? Convert.DBNull : txtRearSuspension.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_braketype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtBrakeType.Text.Trim() == "" ? Convert.DBNull : txtBrakeType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_frontdisc", DbParamTypeMapper.GetInstance[SqlDbType.Bit], frontDiscNotSure.Checked == true ? Convert.DBNull : yesFrontDisc.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_frontdisc_drumsize", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtFrontDisc_DrumSize.Text.Trim() == "" ? Convert.DBNull : txtFrontDisc_DrumSize.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reardisc", DbParamTypeMapper.GetInstance[SqlDbType.Bit], rearDiscNotSure.Checked == true ? Convert.DBNull : yesRearDisc.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reardisc_drumsize", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], txtRearDisc_DrumSize.Text.Trim() == "" ? Convert.DBNull : txtRearDisc_DrumSize.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_callipertype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtCalliperType.Text.Trim() == "" ? Convert.DBNull : txtCalliperType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_wheelsize", DbParamTypeMapper.GetInstance[SqlDbType.Float], txtWheelSize.Text.Trim() == "" ? Convert.DBNull : txtWheelSize.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fronttyre", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtFrontTyre.Text.Trim() == "" ? Convert.DBNull : txtFrontTyre.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reartyre", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtRearTyre.Text.Trim() == "" ? Convert.DBNull : txtRearTyre.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tubelesstyres", DbParamTypeMapper.GetInstance[SqlDbType.Bit], tubelessTyresNotSure.Checked == true ? Convert.DBNull : yesTubelessTyres.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_radialtyres", DbParamTypeMapper.GetInstance[SqlDbType.Bit], radialTyresNotSure.Checked == true ? Convert.DBNull : yesRadialTyres.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_alloywheels", DbParamTypeMapper.GetInstance[SqlDbType.Bit], alloyWheelsNotSure.Checked == true ? Convert.DBNull : yesAlloyWheels.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_electricsystem", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtElectricSystem.Text.Trim() == "" ? Convert.DBNull : txtElectricSystem.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_battery", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtBattery.Text.Trim() == "" ? Convert.DBNull : txtBattery.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_headlighttype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtHeadlightType.Text.Trim() == "" ? Convert.DBNull : txtHeadlightType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_headlightbulbtype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtHeadlightBulbType.Text.Trim() == "" ? Convert.DBNull : txtHeadlightBulbType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_brake_tail_light", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtBrake_Tail_Light.Text.Trim() == "" ? Convert.DBNull : txtBrake_Tail_Light.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_turnsignal", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtTurnsignal.Text.Trim() == "" ? Convert.DBNull : txtTurnsignal.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_passlight", DbParamTypeMapper.GetInstance[SqlDbType.Bit], passLightNotSure.Checked == true ? Convert.DBNull : yesPassLight.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_speedometer", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtSpeedometer.Text.Trim() == "" ? Convert.DBNull : txtSpeedometer.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tachometer", DbParamTypeMapper.GetInstance[SqlDbType.Bit], tachometerNotSure.Checked == true ? Convert.DBNull : yesTachometer.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tachometertype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtTachometerType.Text.Trim() == "" ? Convert.DBNull : txtTachometerType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_shiftlight", DbParamTypeMapper.GetInstance[SqlDbType.Bit], shiftLightNotSure.Checked == true ? Convert.DBNull : yesShiftLight.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_electricstart", DbParamTypeMapper.GetInstance[SqlDbType.Bit], electricStartNotSure.Checked == true ? Convert.DBNull : yesElectricStart.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tripmeter", DbParamTypeMapper.GetInstance[SqlDbType.Bit], tripmeterNotSure.Checked == true ? Convert.DBNull : yesTripmeter.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_nooftripmeters", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtNoOfTripmeters.Text.Trim() == "" ? Convert.DBNull : txtNoOfTripmeters.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tripmetertype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtTripmeterType.Text.Trim() == "" ? Convert.DBNull : txtTripmeterType.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lowfuelindicator", DbParamTypeMapper.GetInstance[SqlDbType.Bit], lowFuelIndicatorNotSure.Checked == true ? Convert.DBNull : yesLowFuelIndicator.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lowoilindicator", DbParamTypeMapper.GetInstance[SqlDbType.Bit], lowOilIndicatorNotSure.Checked == true ? Convert.DBNull : yesLowOilIndicator.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lowbatteryindicator", DbParamTypeMapper.GetInstance[SqlDbType.Bit], lowBatteryIndicatorNotSure.Checked ? Convert.DBNull : yesLowBatteryIndicator.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fuelgauge", DbParamTypeMapper.GetInstance[SqlDbType.Bit], fuelGaugeNotSure.Checked == true ? Convert.DBNull : yesFuelGauge.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_digitalfuelgauge", DbParamTypeMapper.GetInstance[SqlDbType.Bit], digitalFuelGaugeNotSure.Checked == true ? Convert.DBNull : yesDigitalFuelGauge.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pillionseat", DbParamTypeMapper.GetInstance[SqlDbType.Bit], pillionSeatNotSure.Checked == true ? Convert.DBNull : yesPillionSeat.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pillionfootrest", DbParamTypeMapper.GetInstance[SqlDbType.Bit], pillionFootrestNotSure.Checked == true ? Convert.DBNull : yesPillionFootrest.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pillionbackrest", DbParamTypeMapper.GetInstance[SqlDbType.Bit], pillionBackrestNotSure.Checked == true ? Convert.DBNull : yesPillionBackrest.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pilliongrabrail", DbParamTypeMapper.GetInstance[SqlDbType.Bit], pillionGrabrailNotSure.Checked == true ? Convert.DBNull : yesPillionGrabrail.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_standalarm", DbParamTypeMapper.GetInstance[SqlDbType.Bit], standAlarmNotSure.Checked == true ? Convert.DBNull : yesStandAlarm.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_steppedseat", DbParamTypeMapper.GetInstance[SqlDbType.Bit], steppedSeatNotSure.Checked == true ? Convert.DBNull : yesSteppedSeat.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_antilockbrakingsystem", DbParamTypeMapper.GetInstance[SqlDbType.Bit], absNotSure.Checked == true ? Convert.DBNull : yesAntilockBrakingSystem.Checked ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_killswitch", DbParamTypeMapper.GetInstance[SqlDbType.Bit], killswitchNotSure.Checked == true ? Convert.DBNull : yesKillswitch.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clock", DbParamTypeMapper.GetInstance[SqlDbType.Bit], clockNotSure.Checked ? Convert.DBNull : yesClock.Checked == true ? 1 : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_colors", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], txtColors.Text.Trim() == "" ? Convert.DBNull : txtColors.Text.Trim()));

                    bool status = MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);

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
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn("btnSubmit_Click exception : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                sql = @"select concat( ma.name , ' ' , mo.name , ' ' , ve.name ) bikemake
                    from bikemakes ma, bikemodels mo, bikeversions ve 
                    where ma.id=mo.bikemakeid and mo.id=ve.bikemodelid 
                    and ve.id = " + _vid;

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
