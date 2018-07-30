using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Controls;

namespace Bikewale.New
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 25/7/2012
    /// Class to show bike specifications
    /// </summary>
    public class Specs : Page
    {
        protected MakeModelVersion mmv;
        
        // Server vairables
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
                         ltr_AntilockBrakingSystem, ltr_Killswitch, ltr_Clock, ltr_Colors;

        protected BikeRatings ctrl_BikeRatings;

        // Class Level variables
        protected string bike = string.Empty, versionId = string.Empty, imageUrl = string.Empty, estimatedPrice = string.Empty, hostURL = string.Empty,
                         imagePath = string.Empty, make = string.Empty, model = string.Empty, modelId = string.Empty, ModelMaskingName = string.Empty,MakeMaskingName = string.Empty; 
        
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["versionId"]))
                {
                    versionId = Request.QueryString["versionId"];                    
                    //GetBikeDetails();
                    GetBikeSpecs();
                    //GetModelDetails();
                    GetVersionDetails();
                }
            }
        }

        //protected void GetBikeDetails()
        //{
        //    mmv = new MakeModelVersion();
        //    mmv.GetVersionDetails(versionId);
        //    Trace.Warn("versionId : ", versionId);

        //    bike = mmv.BikeName;
        //}

        /// <summary>
        ///     This function will get bike specifications for the verion selected
        /// </summary>
        protected void GetBikeSpecs()
        {
            throw new Exception("Method not used/commented");
           
        }   // end of GetBikeSpecs function

       

        /// <summary>
        ///     Function to show the text "N/A" if data is not available
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     PopulateWhere will check whether value is true or false and return whether value is available or not
        /// </summary>
        /// <param name="featureValue"></param>
        /// <returns></returns>
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

        ///// <summary>
        /////     Function to get all the details of the current model
        ///// </summary>
        ///// <param name="mmv"></param>
        ///// <returns></returns>
        //protected void GetModelDetails()
        //{
        //    MakeModelVersion mmv = new Common.MakeModelVersion();

        //    mmv.GetVersionDetails(versionId.ToString());

        //    mmv.GetModelDetails(mmv.ModelId);
            
        //    imageUrl = "/bikewaleimg/models/" + mmv.LargePic;
        //    hostURL = mmv.HostUrl;

        //    imagePath = MakeModelVersion.GetModelImage(hostURL, imageUrl);
        //    Trace.Warn("image path : ", imagePath);
        //    Trace.Warn("mmv.MaxPrice : ", mmv.MinPrice);
        //    estimatedPrice = CommonOpn.FormatNumeric(mmv.MinPrice) + "-" + CommonOpn.FormatNumeric(mmv.MaxPrice);

        //    make = mmv.Make;
        //    model = mmv.Model;
        //    modelId = mmv.ModelId;

        //    ctrl_BikeRatings.ModelId = modelId;
        //}

        protected void GetVersionDetails()
        {
            mmv = new MakeModelVersion();

            mmv.GetVersionDetails(versionId);

            //imageUrl = "/bikewaleimg/models/" + mmv.LargePic;
            imageUrl = mmv.OriginalImagePath;
            hostURL = mmv.HostUrl;
            imagePath = MakeModelVersion.GetModelImage(hostURL, imageUrl,Bikewale.Utility.ImageSize._227x128);
            Trace.Warn("image path : ", imagePath);
            Trace.Warn("mmv.MaxPrice : ", mmv.MinPrice);
            Trace.Warn("versionId : ", versionId);
            Trace.Warn("mmv.ModelId : ", mmv.ModelId);
            //estimatedPrice = String.IsNullOrEmpty(mmv.MinPrice) ? " - N/A" : CommonOpn.FormatNumeric(mmv.MinPrice) + "-" + CommonOpn.FormatNumeric(mmv.MaxPrice);
            estimatedPrice = CommonOpn.FormatPrice(mmv.MinPrice,mmv.MaxPrice);

            make = mmv.Make;
            model = mmv.Model;
            ModelMaskingName = mmv.ModelMappingName;
            MakeMaskingName = mmv.MakeMappingName;
            modelId = mmv.ModelId;
            bike = mmv.BikeName;
            
            if (!String.IsNullOrEmpty(modelId) && modelId != "-1")
            {
                ctrl_BikeRatings.ModelId = modelId;
            }
            else
            {
                Response.Redirect( "/" + Request.QueryString["make"] + "-bikes/" );
            }
        }

    }   // End of class
}   // End of namespace


