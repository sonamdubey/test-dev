using Bikewale.Common;
using MySql.CoreDAL;
/*
    This class will contain all the common function related to Sell Bike process
*/
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;

namespace Bikewale.Used
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 24/8/2012
    ///     Class contains to process the classified bike inquiry details
    /// </summary>
    public class ClassifiedInquiryDetails
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        // Constructor of the class
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 14 Oct 2014
        /// Summary : Added condition for IsApproved flag
        /// </summary>
        /// <param name="profileId"></param>
        public ClassifiedInquiryDetails(string profileId)
        {
            InquiryId = CommonOpn.GetProfileNo(profileId);

            string sql = "";

            Seller = "Individual";

            sql = @" select cm.id as makeid, cmo.id as modelid, cv.id as versionid,
                 cm.name as make,cm.maskingname as makemaskingname, cm.logourl as logourl, cmo.name as model,cmo.maskingname as modelmaskingname,
                cv.name as version, cv.largepic as bikelargepicurl, cv.bikefueltype, csi.price as price,
                csi.kilometers as kilometers, csi.makeyear as makeyear, csi.color,
                csi.comments as comments, c.name as city,c.maskingname as citymaskingname, st.name state, st.statecode,
                csi.owner as owners, csi.insurancetype as insurance, csi.insuranceexpirydate as insuranceexpiry,
                csi.lifetimetax as tax, csi.registrationplace, csi.kilometers as bikedriven, csi.warranties, csi.modifications,
                spc.cylinders as noofcylinders, spc.transmissiontype, spc.fueltype,
                cu.id as sellerid, csi.cityid as bikecityid,
                cu.isfake, csi.statusid, 0 as isdealer,
                spc.frontdisc, spc.reardisc, spc.tubelesstyres, spc.radialtyres, spc.alloywheels,
                spc.passlight, spc.tachometer, spc.shiftlight, spc.electricstart, spc.tripmeter, spc.lowfuelindicator, spc.lowoilindicator,
                spc.lowbatteryindicator, spc.fuelgauge, spc.digitalfuelgauge, spc.pillionseat, spc.pillionfootrest, spc.pillionbackrest,
                spc.pilliongrabrail, spc.standalarm, spc.steppedseat, spc.antilockbrakingsystem, spc.killswitch, spc.clock
                from classifiedindividualsellinquiries as csi
                left join newbikespecifications spc on csi.bikeversionid = spc.bikeversionid
                left join bikeversions as cv on csi.bikeversionid = cv.id 
                left join bikemodels as cmo on cv.bikemodelid = cmo.id 
                left join bikemakes as cm on cmo.bikemakeid = cm.id 
                left join customers as cu on csi.customerid = cu.id 
                left join bwcities as c on c.id = csi.cityid 
                left join states as st on c.stateid = st.id 
                where csi.id = @v_inquiryid 
                and (csi.customerid = @v_currentuserid or csi.isapproved = 1 ) ";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    //cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt).Value = InquiryId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_currentuserid", DbType.Int64, CurrentUser.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_inquiryid", DbType.Int64, InquiryId));

                    string engine = "";

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            // If Bike removed from the portal for any of the following reason
                            // 1. Bike marked as Fake
                            // 2. Bikes status got changed(not 1)
                            // 3. Bikes page got expired
                            // In any of these cases send used to "BikeSoldOut" page	
                            SoldOutStatus = dr["StatusId"].ToString();// != "1" ? true : false;
                            //IsSoldOut = GetSoldOutStatus(dr["StatusId"].ToString(), dr["ExpiryDate"].ToString(), Convert.ToBoolean(dr["IsBikeFake"]),
                            //                                    dr["PackageExpiryDate"].ToString(), dr["isDealer"].ToString(), Convert.ToBoolean(dr["IsFake"]));						

                            MakeName = dr["Make"].ToString();
                            ModelName = dr["Model"].ToString();
                            VersionName = dr["Version"].ToString();
                            MakeMaskingName = dr["MakeMaskingName"].ToString();
                            ModelMaskingName = dr["ModelMaskingName"].ToString();

                            MakeId = dr["MakeId"].ToString();
                            ModelId = dr["ModelId"].ToString();
                            VersionId = dr["VersionId"].ToString();

                            StateName = dr["State"].ToString();
                            StateCode = dr["StateCode"].ToString();
                            CityName = dr["City"].ToString();
                            CityMaskingName = dr["CityMaskingName"].ToString();

                            CityId = dr["BikeCityId"].ToString();
                            SellerId = dr["SellerId"].ToString();

                            // Bike Basic Information
                            AskingPrice = dr["Price"].ToString() != "" ? CommonOpn.FormatNumeric(dr["Price"].ToString()) : "--";
                            Kms = dr["Kilometers"].ToString() != "" ? CommonOpn.FormatNumeric(dr["Kilometers"].ToString()) : "--";
                            ModelMonthOnly = Convert.ToDateTime(dr["MakeYear"]).ToString("MM");
                            ModelYear = Convert.ToDateTime(dr["MakeYear"]).ToString("MMM-yyyy");
                            ModelYearOnly = Convert.ToDateTime(dr["MakeYear"]).ToString("yyyy");
                            ExtiriorColor = dr["Color"].ToString() != "" ? dr["Color"].ToString() : "--";
                            Registration = dr["RegistrationPlace"].ToString();

                            // format engine string										
                            if (dr["NoOfCylinders"].ToString() != "") engine = dr["NoOfCylinders"].ToString() + "Cyl, ";
                            if (dr["TransmissionType"].ToString() != "") engine += dr["TransmissionType"].ToString() + " Transmission, ";
                            if (dr["FuelType"].ToString() != "") engine += dr["FuelType"].ToString();

                            Engine = engine.Trim() != "" ? engine : "--";

                            Owner = CommonOpn.CheckIsDealerFromProfileNo(profileId) == true ? dr["Owners"].ToString() : GetOwner(dr["Owners"].ToString());
                            Insurance = dr["Insurance"].ToString();

                            if (Insurance != "N/A")
                                InsuranceExpiry = dr["InsuranceExpiry"].ToString() != "" ? "(till " + Convert.ToDateTime(dr["InsuranceExpiry"]).ToString("dd MMM, yyyy") + ")" : "";

                            LifetimeTax = dr["Tax"].ToString();
                            CustomersNote = dr["Comments"].ToString();
                            objTrace.Trace.Warn("cityid0:: " + CityId);

                            Warranties = dr["Warranties"].ToString() != "" ? dr["Warranties"].ToString() : "--";
                            Modifications = dr["Modifications"].ToString() != "" ? dr["Modifications"].ToString() : "--";

                            // Process Features
                            FrontDisc = dr["FrontDisc"].ToString();
                            RearDisc = dr["RearDisc"].ToString();
                            TubelessTyres = dr["TubelessTyres"].ToString();
                            RadialTyres = dr["RadialTyres"].ToString();
                            AlloyWheels = dr["AlloyWheels"].ToString();
                            PassLight = dr["PassLight"].ToString();
                            Tachometer = dr["Tachometer"].ToString();
                            ShiftLight = dr["ShiftLight"].ToString();
                            ElectricStart = dr["ElectricStart"].ToString();
                            Tripmeter = dr["Tripmeter"].ToString();
                            LowFuelIndicator = dr["LowFuelIndicator"].ToString();
                            LowOilIndicator = dr["LowOilIndicator"].ToString();
                            LowBatteryIndicator = dr["LowBatteryIndicator"].ToString();
                            FuelGauge = dr["FuelGauge"].ToString();
                            DigitalFuelGauge = dr["DigitalFuelGauge"].ToString();
                            PillionSeat = dr["PillionSeat"].ToString();
                            PillionFootrest = dr["PillionFootrest"].ToString();
                            PillionBackrest = dr["PillionBackrest"].ToString();
                            PillionGrabrail = dr["PillionGrabrail"].ToString();
                            StandAlarm = dr["StandAlarm"].ToString();
                            SteppedSeat = dr["SteppedSeat"].ToString();
                            AntilockBrakingSystem = dr["AntilockBrakingSystem"].ToString();
                            Killswitch = dr["Killswitch"].ToString();
                            Clock = dr["Clock"].ToString();

                            HttpContext.Current.Trace.Warn("ShiftLight : ", ShiftLight);

                            dr.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objTrace.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, objTrace.Request.ServerVariables["URL"]);
                
            }
        }

        public string Parse_Features()
        {
            StringBuilder sb = null;
            sb = new StringBuilder();

            sb.Append("<ul class='ul-tick-chk'>");

            // Append feature to list only when it is true(available)

            if (CheckFeature(FrontDisc)) { sb.Append("<li>FrontDisc</li>"); }
            if (CheckFeature(RearDisc)) { sb.Append("<li>RearDisc</li>"); }
            if (CheckFeature(TubelessTyres)) { sb.Append("<li>TubelessTyres</li>"); }
            if (CheckFeature(RadialTyres)) { sb.Append("<li>RadialTyres</li>"); }
            if (CheckFeature(AlloyWheels)) { sb.Append("<li>AlloyWheels</li>"); }
            if (CheckFeature(PassLight)) { sb.Append("<li>PassLight</li>"); }
            if (CheckFeature(Tachometer)) { sb.Append("<li>Tachometer</li>"); }
            if (CheckFeature(ShiftLight)) { sb.Append("<li>ShiftLight</li>"); }
            if (CheckFeature(ElectricStart)) { sb.Append("<li>ElectricStart</li>"); }
            if (CheckFeature(Tripmeter)) { sb.Append("<li>Tripmeter</li>"); }
            if (CheckFeature(LowFuelIndicator)) { sb.Append("<li>LowFuelIndicator</li>"); }
            if (CheckFeature(LowOilIndicator)) { sb.Append("<li>LowOilIndicator</li>"); }
            if (CheckFeature(LowBatteryIndicator)) { sb.Append("<li>LowBatteryIndicator</li>"); }
            if (CheckFeature(FuelGauge)) { sb.Append("<li>FuelGauge</li>"); }
            if (CheckFeature(DigitalFuelGauge)) { sb.Append("<li>DigitalFuelGauge</li>"); }
            if (CheckFeature(PillionSeat)) { sb.Append("<li>PillionSeat</li>"); }
            if (CheckFeature(PillionFootrest)) { sb.Append("<li>PillionFootrest</li>"); }
            if (CheckFeature(PillionBackrest)) { sb.Append("<li>PillionBackrest</li>"); }
            if (CheckFeature(PillionGrabrail)) { sb.Append("<li>PillionGrabrail</li>"); }
            if (CheckFeature(StandAlarm)) { sb.Append("<li>StandAlarm</li>"); }
            if (CheckFeature(SteppedSeat)) { sb.Append("<li>SteppedSeat</li>"); }
            if (CheckFeature(AntilockBrakingSystem)) { sb.Append("<li>AntilockBrakingSystem</li>"); }
            if (CheckFeature(Killswitch)) { sb.Append("<li>Killswitch</li>"); }
            if (CheckFeature(Clock)) { sb.Append("<li>Clock</li>"); }

            sb.Append("</ul>");

            return sb != null ? sb.ToString() : "";
        }

        // Check the value of the feature
        protected bool CheckFeature(string feature)
        {
            bool isChecked = false;

            if (!String.IsNullOrEmpty(feature))
            {
                isChecked = (feature == "1") ? true : false;
            }
            //HttpContext.Current.Trace.Warn("isChecked : ", isChecked.ToString() + " feature : " + feature);
            return isChecked;
        }

        string GetOwner(string ownerId)
        {
            string ownerStr = "";

            switch (ownerId)
            {
                case "1":
                    ownerStr = "First";
                    break;
                case "2":
                    ownerStr = "Second";
                    break;
                case "3":
                    ownerStr = "Third";
                    break;
                case "4":
                    ownerStr = "Fourth";
                    break;
                default:
                    ownerStr = "Fifth";
                    break;
            }

            return ownerStr;
        }

        string GetFuelTypeText(string fuelType, string additinalFuel)
        {
            var fuelVal = "-";

            switch (fuelType)
            {
                case "1":
                    fuelVal = "Petrol";
                    break;
                case "2":
                    fuelVal = "Diesel";
                    break;
                case "3":
                    fuelVal = "CNG";
                    break;
                case "4":
                    fuelVal = "LPG";
                    break;
                case "5":
                    fuelVal = "Electric";
                    break;
            }

            if (additinalFuel != "" && fuelVal != additinalFuel)
                fuelVal += " + " + additinalFuel;

            return fuelVal;
        }

        // Lot of redundent conditions just to checkout whether Bike is active on the Portal or Sold Out
        // Need to be optimised
        bool GetSoldOutStatus(string statusId, string expiryDate, bool isBikeFake, string packageExpiryDate, string isDealer, bool isSellerFake)
        {
            bool isSold = true;

            if (statusId == "1")
            {
                if (expiryDate != "")
                {
                    if (Convert.ToDateTime(expiryDate) >= DateTime.Today)
                        isSold = false;
                }
                else
                    isSold = false;
            }

            if (isSold == false && isBikeFake.ToString() != "")
                isSold = isBikeFake;

            if (isDealer == "1") // dealer
            {
                if (isSold == false && isSellerFake.ToString() != "")
                    isSold = isSellerFake;

                if (packageExpiryDate != "")
                {
                    if (Convert.ToDateTime(packageExpiryDate) < DateTime.Today) isSold = true;
                }
                else
                    isSold = true;
            }

            return isSold;
        }
        //
        // Bike Make-Model-Version Information
        //		
        public string BikeName
        {
            get { return MakeName + " " + ModelName + " " + VersionName; }
        }

        string _MakeMaskingName = "";
        public string MakeMaskingName
        {
            get { return _MakeMaskingName; }
            set { _MakeMaskingName = value; }
        }

        string _ModelMaskingName = "";
        public string ModelMaskingName
        {
            get { return _ModelMaskingName; }
            set { _ModelMaskingName = value; }
        }

        string _MakeName = "";
        public string MakeName
        {
            get { return _MakeName; }
            set { _MakeName = value; }
        }

        string _ModelName = "";
        public string ModelName
        {
            get { return _ModelName; }
            set { _ModelName = value; }
        }

        string _VersionName = "";
        public string VersionName
        {
            get { return _VersionName; }
            set { _VersionName = value; }
        }

        string _MakeId = "";
        public string MakeId
        {
            get { return _MakeId; }
            set { _MakeId = value; }
        }

        string _ModelId = "";
        public string ModelId
        {
            get { return _ModelId; }
            set { _ModelId = value; }
        }

        string _VersionId = "";
        public string VersionId
        {
            get { return _VersionId; }
            set { _VersionId = value; }
        }

        string _CityName = "";
        public string CityName
        {
            get { return _CityName; }
            set { _CityName = value; }
        }

        string _CityMaskingName = "";
        public string CityMaskingName
        {
            get { return _CityMaskingName; }
            set { _CityMaskingName = value; }
        }

        string _CityId = "";
        public string CityId
        {
            get { return _CityId; }
            set { _CityId = value; }
        }

        string _AreaName = "";
        public string AreaName
        {
            get { return _AreaName; }
            set { _AreaName = value; }
        }

        string _StateName = "";
        public string StateName
        {
            get { return _StateName; }
            set { _StateName = value; }
        }

        string _StateCode = "";
        public string StateCode
        {
            get { return _StateCode; }
            set { _StateCode = value; }
        }

        //
        // Bike Basic Information
        // 
        string _InquiryId = "";
        public string InquiryId
        {
            get { return _InquiryId; }
            set { _InquiryId = value; }
        }

        string _AskingPrice = "";
        public string AskingPrice
        {
            get { return _AskingPrice; }
            set { _AskingPrice = value; }
        }

        string _Kms = "";
        public string Kms
        {
            get { return _Kms; }
            set { _Kms = value; }
        }

        string _ModelYear = "";
        public string ModelYear
        {
            get { return _ModelYear; }
            set { _ModelYear = value; }
        }

        string _ModelMonthOnly = "";
        public string ModelMonthOnly
        {
            get { return _ModelMonthOnly; }
            set { _ModelMonthOnly = value; }
        }

        string _ModelYearOnly = "";
        public string ModelYearOnly
        {
            get { return _ModelYearOnly; }
            set { _ModelYearOnly = value; }
        }

        string _ExtiriorColor = "";
        public string ExtiriorColor
        {
            get { return _ExtiriorColor; }
            set { _ExtiriorColor = value; }
        }

        string _ExtiriorCode = "";
        public string ExtiriorCode
        {
            get { return _ExtiriorCode; }
            set { _ExtiriorCode = value; }
        }


        string _Registration = "";
        public string Registration
        {
            get { return _Registration; }
            set { _Registration = value; }
        }

        string _FuelEconomy = "";
        public string FuelEconomy
        {
            get { return _FuelEconomy; }
            set { _FuelEconomy = value; }
        }

        string _Engine = "";
        public string Engine
        {
            get { return _Engine; }
            set { _Engine = value; }
        }

        string _Owner = "";
        public string Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }

        string _Seller = "";
        public string Seller
        {
            get { return _Seller; }
            set { _Seller = value; }
        }

        string _SellerId = "";
        public string SellerId
        {
            get { return _SellerId; }
            set { _SellerId = value; }
        }

        string _Insurance = "";
        public string Insurance
        {
            get { return _Insurance; }
            set { _Insurance = value; }
        }

        string _InsuranceExpiry = "";
        public string InsuranceExpiry
        {
            get { return _InsuranceExpiry; }
            set { _InsuranceExpiry = value; }
        }

        string _LifetimeTax = "";
        public string LifetimeTax
        {
            get { return _LifetimeTax; }
            set { _LifetimeTax = value; }
        }

        string _CustomersNote = "";
        public string CustomersNote
        {
            get { return _CustomersNote; }
            set { _CustomersNote = value; }
        }

        string _Warranties = "";
        public string Warranties
        {
            get { return _Warranties; }
            set { _Warranties = value; }
        }

        string _Modifications = "";
        public string Modifications
        {
            get { return _Modifications; }
            set { _Modifications = value; }
        }

        //Modified By : Ashwini Todkar on 4 Sep 2014
        //Changed _soldOutStatus datatype from bool to string to get status of Sell bike inquiry
        string _soldOutStatus = string.Empty;
        public string SoldOutStatus
        {
            get { return _soldOutStatus; }
            set { _soldOutStatus = value; }
        }

        string _LastUpdated = string.Empty;
        public string LastUpdated
        {
            get { return _LastUpdated; }
            set { _LastUpdated = value; }
        }

        string _CertifiedId = string.Empty;
        public string CertifiedId
        {
            get { return _CertifiedId; }
            set { _CertifiedId = value; }
        }

        string _FuelType = string.Empty;
        public string FuelType
        {
            get { return _FuelType; }
            set { _FuelType = value; }
        }

        // Properties to access features list
        public string FrontDisc { get; set; }
        public string RearDisc { get; set; }
        public string TubelessTyres { get; set; }
        public string RadialTyres { get; set; }
        public string AlloyWheels { get; set; }
        public string PassLight { get; set; }
        public string Tachometer { get; set; }
        public string ShiftLight { get; set; }
        public string ElectricStart { get; set; }
        public string Tripmeter { get; set; }
        public string LowFuelIndicator { get; set; }
        public string LowOilIndicator { get; set; }
        public string LowBatteryIndicator { get; set; }
        public string FuelGauge { get; set; }
        public string DigitalFuelGauge { get; set; }
        public string PillionSeat { get; set; }
        public string PillionFootrest { get; set; }
        public string PillionBackrest { get; set; }
        public string PillionGrabrail { get; set; }
        public string StandAlarm { get; set; }
        public string SteppedSeat { get; set; }
        public string AntilockBrakingSystem { get; set; }
        public string Killswitch { get; set; }
        public string Clock { get; set; }
    }
}