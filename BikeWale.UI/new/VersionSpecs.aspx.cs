﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;

namespace Bikewale.New
{
    public class VersionSpecs : System.Web.UI.Page
    {
        protected Literal ltr_Displacement, ltr_Cylinders, ltr_MaxPower, ltr_MaximumTorque, ltr_Bore, ltr_Stroke, ltr_ValvesPerCylinder, ltr_FuelDeliverySystem,
                 ltr_FuelType, ltr_Ignition, ltr_SparkPlugsPerCylinder, ltr_CoolingSystem, ltr_GearboxType, ltr_NoOfGears, ltr_TransmissionType, ltr_Clutch,
                 ltr_Performance_0_60_kmph, ltr_Performance_0_80_kmph, ltr_Performance_0_40_m, ltr_TopSpeed, ltr_Performance_60_0_kmph, ltr_Performance_80_0_kmph,
                 ltr_KerbWeight, ltr_OverallLength, ltr_OverallWidth, ltr_OverallHeight, ltr_Wheelbase, ltr_GroundClearance, ltr_SeatHeight, ltr_FuelTankCapacity,
                 ltr_ReserveFuelCapacity, ltr_FuelEfficiencyOverall, ltr_FuelEfficiencyRange, ltr_ChassisType, ltr_FrontSuspension, ltr_RearSuspension,
                 ltr_BrakeType, ltr_FrontDisc, ltr_FrontDisc_DrumSize, ltr_RearDisc, ltr_RearDisc_DrumSize, ltr_CalliperType, ltr_WheelSize, ltr_FrontTyre,
                 ltr_RearTyre, ltr_TubelessTyres, ltr_RadialTyres, ltr_AlloyWheels, ltr_ElectricSystem, ltr_Battery, ltr_HeadlightType, ltr_HeadlightBulbType,
                 ltr_Brake_Tail_Light, ltr_TurnSignal, ltr_PassLight, ltr_Speedometer, ltr_Tachometer, ltr_TachometerType, ltr_ShiftLight, ltr_ElectricStart,
                 ltr_Tripmeter, ltr_NoOfTripmeters, ltr_TripmeterType, ltr_LowFuelIndicator, ltr_LowOilIndicator, ltr_LowBatteryIndicator, ltr_FuelGauge,
                 ltr_DigitalFuelGauge, ltr_PillionSeat, ltr_PillionFootrest, ltr_PillionBackrest, ltr_PillionGrabrail, ltr_StandAlarm, ltr_SteppedSeat,
                 ltr_AntilockBrakingSystem, ltr_Killswitch, ltr_Clock, ltr_Colors, ltr_MaxTorqueRpm, ltr_MaxPowerRpm;
        string versionId = string.Empty;
        
        protected int count = 0;

        protected short isSpecsAvailable;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            versionId = Request.QueryString["version"];
            count = Convert.ToInt16(Request.QueryString["count"]);
            Trace.Warn("version id on ", versionId);
            Trace.Warn("Count" , count.ToString());
            if(!String.IsNullOrEmpty(versionId))
                GetBikeSpecs();
        }

        protected void GetBikeSpecs()
        {
            throw new Exception("Method not used/commented");

            //try
            //{
            //    Database db = new Database();

            //    using (SqlConnection conn = new SqlConnection(db.GetConString()))
            //    {
            //        using (SqlCommand cmd = new SqlCommand())
            //        {
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.CommandText = "GetNewBikesSpecification_SP";
            //            cmd.Connection = conn;
                        Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);
            //            cmd.Parameters.Add("@BikeVersionId", SqlDbType.SmallInt).Value = versionId;
            //            cmd.Parameters.Add("@Displacement", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Cylinders", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@MaxPower", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@MaxPowerRpm", SqlDbType.Int).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@MaximumTorque", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@MaximumTorqueRpm", SqlDbType.Int).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Bore", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Stroke", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@ValvesPerCylinder", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@FuelDeliverySystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@FuelType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Ignition", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@SparkPlugsPerCylinder", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@CoolingSystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@GearboxType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@NoOfGears", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@TransmissionType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Clutch", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Performance_0_60_kmph", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Performance_0_80_kmph", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Performance_0_40_m", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            //changed topspeed data type from small int to Float
            //            //Modified By : Sushil Kumar on 15-07-2015
            //            cmd.Parameters.Add("@TopSpeed", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Performance_60_0_kmph", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Performance_80_0_kmph", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@KerbWeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@OverallLength", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@OverallWidth", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@OverallHeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Wheelbase", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@GroundClearance", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@SeatHeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@FuelTankCapacity", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@ReserveFuelCapacity", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@FuelEfficiencyOverall", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@FuelEfficiencyRange", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@ChassisType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@FrontSuspension", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@RearSuspension", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@BrakeType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@FrontDisc", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@FrontDisc_DrumSize", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@RearDisc", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@RearDisc_DrumSize", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@CalliperType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@WheelSize", SqlDbType.Float).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@FrontTyre", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@RearTyre", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@TubelessTyres", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@RadialTyres", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@AlloyWheels", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@ElectricSystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Battery", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@HeadlightType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@HeadlightBulbType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Brake_Tail_Light", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@TurnSignal", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@PassLight", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Speedometer", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Tachometer", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@TachometerType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@ShiftLight", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@ElectricStart", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Tripmeter", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@NoOfTripmeters", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@TripmeterType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@LowFuelIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@LowOilIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@LowBatteryIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@FuelGauge", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@DigitalFuelGauge", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@PillionSeat", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@PillionFootrest", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@PillionBackrest", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@PillionGrabrail", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@StandAlarm", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@SteppedSeat", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@AntilockBrakingSystem", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Killswitch", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Clock", SqlDbType.Bit).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Colors", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@RowCount", SqlDbType.TinyInt).Direction = ParameterDirection.Output;

            //            conn.Open();
            //            cmd.ExecuteNonQuery();

            //            isSpecsAvailable = Convert.ToInt16(cmd.Parameters["@RowCount"].Value);

            //            if (isSpecsAvailable > 0)
            //                ShowSpecs(cmd);
            //        }
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    Trace.Warn("GetBikeSpecs SqlEX: " + ex.Message);
            //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    Trace.Warn("GetBikeSpecs EX: " + ex.Message);
            //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
        }

        protected void ShowSpecs(SqlCommand sqlCmd)
        {
            throw new Exception("Method not used/commented");

            //ltr_Displacement.Text = ShowNotAvailable(sqlCmd.Parameters["@Displacement"].Value.ToString());
            //ltr_Cylinders.Text = ShowNotAvailable(sqlCmd.Parameters["@Cylinders"].Value.ToString());
            //ltr_MaxPower.Text = ShowNotAvailable(sqlCmd.Parameters["@MaxPower"].Value.ToString()) != "--" ? sqlCmd.Parameters["@MaxPower"].Value.ToString() + " bhp " : "--";
            //ltr_MaxPowerRpm.Text = ShowNotAvailable(sqlCmd.Parameters["@MaxPowerRpm"].Value.ToString()) != "--" ? " @ " + sqlCmd.Parameters["@MaxPowerRpm"].Value.ToString()  + " rpm " : "";
            //ltr_MaximumTorque.Text = ShowNotAvailable(sqlCmd.Parameters["@MaximumTorque"].Value.ToString()) != "--" ? sqlCmd.Parameters["@MaximumTorque"].Value.ToString() + " Nm " : "--";
            //ltr_MaxTorqueRpm.Text = ShowNotAvailable(sqlCmd.Parameters["@MaximumTorqueRpm"].Value.ToString()) != "--" ? " @ " + sqlCmd.Parameters["@MaximumTorqueRpm"].Value.ToString() + " rpm " : "";
            
            //ltr_Bore.Text = ShowNotAvailable(sqlCmd.Parameters["@Bore"].Value.ToString());
            //ltr_Stroke.Text = ShowNotAvailable(sqlCmd.Parameters["@Stroke"].Value.ToString());
            //ltr_ValvesPerCylinder.Text = ShowNotAvailable(sqlCmd.Parameters["@ValvesPerCylinder"].Value.ToString());
            //ltr_FuelDeliverySystem.Text = ShowNotAvailable(sqlCmd.Parameters["@FuelDeliverySystem"].Value.ToString());
            //ltr_FuelType.Text = ShowNotAvailable(sqlCmd.Parameters["@FuelType"].Value.ToString());
            //ltr_Ignition.Text = ShowNotAvailable(sqlCmd.Parameters["@Ignition"].Value.ToString());
            //ltr_SparkPlugsPerCylinder.Text = ShowNotAvailable(sqlCmd.Parameters["@SparkPlugsPerCylinder"].Value.ToString());
            //ltr_CoolingSystem.Text = ShowNotAvailable(sqlCmd.Parameters["@CoolingSystem"].Value.ToString());
            //ltr_GearboxType.Text = ShowNotAvailable(sqlCmd.Parameters["@GearboxType"].Value.ToString());
            //ltr_NoOfGears.Text = ShowNotAvailable(sqlCmd.Parameters["@NoOfGears"].Value.ToString());
            //ltr_TransmissionType.Text = ShowNotAvailable(sqlCmd.Parameters["@TransmissionType"].Value.ToString());
            //ltr_Clutch.Text = ShowNotAvailable(sqlCmd.Parameters["@Clutch"].Value.ToString());
            //ltr_Performance_0_60_kmph.Text = ShowNotAvailable(sqlCmd.Parameters["@Performance_0_60_kmph"].Value.ToString());
            //ltr_Performance_0_80_kmph.Text = ShowNotAvailable(sqlCmd.Parameters["@Performance_0_80_kmph"].Value.ToString());
            //ltr_Performance_0_40_m.Text = ShowNotAvailable(sqlCmd.Parameters["@Performance_0_40_m"].Value.ToString());
            //ltr_TopSpeed.Text = ShowNotAvailable(sqlCmd.Parameters["@TopSpeed"].Value.ToString());
            //ltr_Performance_60_0_kmph.Text = ShowNotAvailable(sqlCmd.Parameters["@Performance_60_0_kmph"].Value.ToString());
            //ltr_Performance_80_0_kmph.Text = ShowNotAvailable(sqlCmd.Parameters["@Performance_80_0_kmph"].Value.ToString());
            //ltr_KerbWeight.Text = ShowNotAvailable(sqlCmd.Parameters["@KerbWeight"].Value.ToString());
            //ltr_OverallLength.Text = ShowNotAvailable(sqlCmd.Parameters["@OverallLength"].Value.ToString());
            //ltr_OverallWidth.Text = ShowNotAvailable(sqlCmd.Parameters["@OverallWidth"].Value.ToString());
            //ltr_OverallHeight.Text = ShowNotAvailable(sqlCmd.Parameters["@OverallHeight"].Value.ToString());
            //ltr_Wheelbase.Text = ShowNotAvailable(sqlCmd.Parameters["@Wheelbase"].Value.ToString());
            //ltr_GroundClearance.Text = ShowNotAvailable(sqlCmd.Parameters["@GroundClearance"].Value.ToString());
            //ltr_SeatHeight.Text = ShowNotAvailable(sqlCmd.Parameters["@SeatHeight"].Value.ToString());
            //ltr_FuelTankCapacity.Text = ShowNotAvailable(sqlCmd.Parameters["@FuelTankCapacity"].Value.ToString());
            //ltr_ReserveFuelCapacity.Text = ShowNotAvailable(sqlCmd.Parameters["@ReserveFuelCapacity"].Value.ToString());
            //ltr_FuelEfficiencyOverall.Text = ShowNotAvailable(sqlCmd.Parameters["@FuelEfficiencyOverall"].Value.ToString());
            //ltr_FuelEfficiencyRange.Text = ShowNotAvailable(sqlCmd.Parameters["@FuelEfficiencyRange"].Value.ToString());
            //ltr_ChassisType.Text = ShowNotAvailable(sqlCmd.Parameters["@ChassisType"].Value.ToString());
            //ltr_FrontSuspension.Text = ShowNotAvailable(sqlCmd.Parameters["@FrontSuspension"].Value.ToString());
            //ltr_RearSuspension.Text = ShowNotAvailable(sqlCmd.Parameters["@RearSuspension"].Value.ToString());
            //ltr_BrakeType.Text = ShowNotAvailable(sqlCmd.Parameters["@BrakeType"].Value.ToString());
            //ltr_FrontDisc.Text = GetFeatures(sqlCmd.Parameters["@FrontDisc"].Value.ToString());
            //ltr_FrontDisc_DrumSize.Text = ShowNotAvailable(sqlCmd.Parameters["@FrontDisc_DrumSize"].Value.ToString());
            //ltr_RearDisc.Text = GetFeatures(sqlCmd.Parameters["@RearDisc"].Value.ToString());
            //ltr_RearDisc_DrumSize.Text = ShowNotAvailable(sqlCmd.Parameters["@RearDisc_DrumSize"].Value.ToString());
            //ltr_CalliperType.Text = ShowNotAvailable(sqlCmd.Parameters["@CalliperType"].Value.ToString());
            //ltr_WheelSize.Text = ShowNotAvailable(sqlCmd.Parameters["@WheelSize"].Value.ToString());
            //ltr_FrontTyre.Text = ShowNotAvailable(sqlCmd.Parameters["@FrontTyre"].Value.ToString());
            //ltr_RearTyre.Text = ShowNotAvailable(sqlCmd.Parameters["@RearTyre"].Value.ToString());
            //ltr_TubelessTyres.Text = GetFeatures(sqlCmd.Parameters["@TubelessTyres"].Value.ToString());
            //ltr_RadialTyres.Text = GetFeatures(sqlCmd.Parameters["@RadialTyres"].Value.ToString());
            //ltr_AlloyWheels.Text = GetFeatures(sqlCmd.Parameters["@AlloyWheels"].Value.ToString());
            //ltr_ElectricSystem.Text = ShowNotAvailable(sqlCmd.Parameters["@ElectricSystem"].Value.ToString());
            //ltr_Battery.Text = ShowNotAvailable(sqlCmd.Parameters["@Battery"].Value.ToString());
            //ltr_HeadlightType.Text = ShowNotAvailable(sqlCmd.Parameters["@HeadlightType"].Value.ToString());
            //ltr_HeadlightBulbType.Text = ShowNotAvailable(sqlCmd.Parameters["@HeadlightBulbType"].Value.ToString());
            //ltr_Brake_Tail_Light.Text = ShowNotAvailable(sqlCmd.Parameters["@Brake_Tail_Light"].Value.ToString());
            //ltr_TurnSignal.Text = ShowNotAvailable(sqlCmd.Parameters["@TurnSignal"].Value.ToString());
            //ltr_PassLight.Text = GetFeatures(sqlCmd.Parameters["@PassLight"].Value.ToString());
            //ltr_Speedometer.Text = ShowNotAvailable(sqlCmd.Parameters["@Speedometer"].Value.ToString());
            //ltr_Tachometer.Text = GetFeatures(sqlCmd.Parameters["@Tachometer"].Value.ToString());
            //ltr_TachometerType.Text = ShowNotAvailable(sqlCmd.Parameters["@TachometerType"].Value.ToString());
            //ltr_ShiftLight.Text = GetFeatures(sqlCmd.Parameters["@ShiftLight"].Value.ToString());
            //ltr_ElectricStart.Text = GetFeatures(sqlCmd.Parameters["@ElectricStart"].Value.ToString());
            //ltr_Tripmeter.Text = GetFeatures(sqlCmd.Parameters["@Tripmeter"].Value.ToString());
            //ltr_NoOfTripmeters.Text = ShowNotAvailable(sqlCmd.Parameters["@NoOfTripmeters"].Value.ToString());
            //ltr_TripmeterType.Text = ShowNotAvailable(sqlCmd.Parameters["@TripmeterType"].Value.ToString());
            //ltr_LowFuelIndicator.Text = GetFeatures(sqlCmd.Parameters["@LowFuelIndicator"].Value.ToString());
            //ltr_LowOilIndicator.Text = GetFeatures(sqlCmd.Parameters["@LowOilIndicator"].Value.ToString());
            //ltr_LowBatteryIndicator.Text = GetFeatures(sqlCmd.Parameters["@LowBatteryIndicator"].Value.ToString());
            //ltr_FuelGauge.Text = GetFeatures(sqlCmd.Parameters["@FuelGauge"].Value.ToString());
            //ltr_DigitalFuelGauge.Text = GetFeatures(sqlCmd.Parameters["@DigitalFuelGauge"].Value.ToString());
            //ltr_PillionSeat.Text = GetFeatures(sqlCmd.Parameters["@PillionSeat"].Value.ToString());
            //ltr_PillionFootrest.Text = GetFeatures(sqlCmd.Parameters["@PillionFootrest"].Value.ToString());
            //ltr_PillionBackrest.Text = GetFeatures(sqlCmd.Parameters["@PillionBackrest"].Value.ToString());
            //ltr_PillionGrabrail.Text = GetFeatures(sqlCmd.Parameters["@PillionGrabrail"].Value.ToString());
            //ltr_StandAlarm.Text = GetFeatures(sqlCmd.Parameters["@StandAlarm"].Value.ToString());
            //ltr_SteppedSeat.Text = GetFeatures(sqlCmd.Parameters["@SteppedSeat"].Value.ToString());
            //ltr_AntilockBrakingSystem.Text = GetFeatures(sqlCmd.Parameters["@AntilockBrakingSystem"].Value.ToString());
            //ltr_Killswitch.Text = GetFeatures(sqlCmd.Parameters["@Killswitch"].Value.ToString());
            //ltr_Clock.Text = GetFeatures(sqlCmd.Parameters["@Clock"].Value.ToString());
            //ltr_Colors.Text = ShowNotAvailable(sqlCmd.Parameters["@Colors"].Value.ToString());
        }   // End of ShowSpecs function

        protected string ShowNotAvailable(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return "--";
            }
            else
            {
                return value;
            }
        }

        protected string GetFeatures(string featureValue)
        {
            string showValue = String.Empty;

            if (String.IsNullOrEmpty(featureValue))
            {
                showValue = "--";
            }
            else
            {
                showValue = featureValue == "True" ? "Yes" : "No";
            }
            return showValue;
        }   // End of GetFeatures method
    }
}