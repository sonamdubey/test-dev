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
using System.Collections;
using Bikewale.Common;
//using BikeWale.Controls;

namespace Bikewale.New
{
	public class BikeComparison : Page
	{		
		protected HtmlGenericControl spnError, ad_160x600;
		protected HtmlTable tblCompare;
        protected Literal ltrDefaultCityName, ltrTitle;

		public string compareWidth = "790px";
		public string featuredBikeId = "", spotlightUrl = "";
		public int featuredBikeIndex = 0;
		static int arrSize = 5;
		
		protected ArrayList arObj = null;
		
		public string[]     bike = new string[arrSize], makeName = new string[arrSize], modelName = new string[arrSize], ModelMaskingName = new string[arrSize], MakeMaskingName = new string[arrSize], versionName = new string[arrSize],
                            versionId = new string[arrSize], hostURL = new string[arrSize], loan = new string[arrSize], price = new string[arrSize],
                            leadAggregator = new string[arrSize],
                            
                            // Specs
                            displacement = new string[arrSize], cylinders = new string[arrSize], maxPower = new string[arrSize],
                            maximumTorque = new string[arrSize], bore = new string[arrSize], stroke = new string[arrSize], valvesPerCylinder = new string[arrSize],
                            fuelDeliverySystem = new string[arrSize], fuelType = new string[arrSize],ignition = new string[arrSize], sparkPlugsPerCylinder = new string[arrSize],
                            coolingSystem = new string[arrSize], gearboxType = new string[arrSize],noOfGears = new string[arrSize],
                            transmissionType = new string[arrSize], clutch = new string[arrSize], performance_0_60_kmph = new string[arrSize],
                            performance_0_80_kmph = new string[arrSize], performance_0_40_m = new string[arrSize], topSpeed = new string[arrSize],
                            performance_60_0_kmph = new string[arrSize], performance_80_0_kmph = new string[arrSize],
                            kerbWeight = new string[arrSize], overallLength = new string[arrSize],overallWidth = new string[arrSize], overallHeight = new string[arrSize],
                            wheelbase = new string[arrSize], groundClearance = new string[arrSize], seatHeight = new string[arrSize], fuelTankCapacity = new string[arrSize],
                            reserveFuelCapacity = new string[arrSize], fuelEfficiencyOverall = new string[arrSize], fuelEfficiencyRange = new string[arrSize],
                            chassisType = new string[arrSize], frontSuspension = new string[arrSize], rearSuspension = new string[arrSize], brakeType = new string[arrSize],
                            frontDisc = new string[arrSize], frontDisc_DrumSize = new string[arrSize], rearDisc = new string[arrSize], rearDisc_DrumSize = new string[arrSize],
                            calliperType = new string[arrSize], wheelSize = new string[arrSize], frontTyre = new string[arrSize], rearTyre = new string[arrSize],
                            tubelessTyres = new string[arrSize], radialTyres = new string[arrSize], alloyWheels = new string[arrSize], electricSystem = new string[arrSize],
                            battery = new string[arrSize], headlightType = new string[arrSize], headlightBulbType = new string[arrSize],
                            brake_Tail_Light = new string[arrSize], turnSignal = new string[arrSize], passLight = new string[arrSize], 
                            
                            // Features
                            speedometer = new string[arrSize], tachometer = new string[arrSize],tachometerType = new string[arrSize], shiftLight = new string[arrSize],
                            electricStart = new string[arrSize],tripmeter = new string[arrSize], noOfTripmeters = new string[arrSize],
                            tripmeterType = new string[arrSize], lowFuelIndicator = new string[arrSize],lowOilIndicator = new string[arrSize],
                            lowBatteryIndicator = new string[arrSize],fuelGauge = new string[arrSize], digitalFuelGauge = new string[arrSize],
                            pillionSeat = new string[arrSize], pillionFootrest = new string[arrSize],pillionBackrest = new string[arrSize],
                            pillionGrabrail = new string[arrSize],standAlarm = new string[arrSize], steppedSeat = new string[arrSize],
                            antilockBrakingSystem = new string[arrSize],killswitch = new string[arrSize], clock = new string[arrSize],colors = new string[arrSize]; 
		
		public string title = "", youHere = "";

        protected string pageTitle = string.Empty;

        protected string versions = string.Empty;

        protected DataSet dsColors = null;
		
        //public string oem1
        //{
        //    get
        //    {
        //        if(ViewState["oem1"] != null)
        //            return ViewState["oem1"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["oem1"] = value;}
        //}
		
        //public string bodyType1
        //{
        //    get
        //    {
        //        if(ViewState["bodyType1"] != null)
        //            return ViewState["bodyType1"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["bodyType1"] = value;}
        //}
		
        //public string subSegment1
        //{
        //    get
        //    {
        //        if(ViewState["subSegment1"] != null)
        //            return ViewState["subSegment1"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["subSegment1"] = value;}
        //}
		
        //public string oem2
        //{
        //    get
        //    {
        //        if(ViewState["oem2"] != null)
        //            return ViewState["oem2"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["oem2"] = value;}
        //}
		
        //public string bodyType2
        //{
        //    get
        //    {
        //        if(ViewState["bodyType2"] != null)
        //            return ViewState["bodyType2"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["bodyType2"] = value;}
        //}
		
        //public string subSegment2
        //{
        //    get
        //    {
        //        if(ViewState["subSegment2"] != null)
        //            return ViewState["subSegment2"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["subSegment2"] = value;}
        //}
		
        //public string oem3
        //{
        //    get
        //    {
        //        if(ViewState["oem3"] != null)
        //            return ViewState["oem3"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["oem3"] = value;}
        //}
		
        //public string bodyType3
        //{
        //    get
        //    {
        //        if(ViewState["bodyType3"] != null)
        //            return ViewState["bodyType3"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["bodyType3"] = value;}
        //}
		
        //public string subSegment3
        //{
        //    get
        //    {
        //        if(ViewState["subSegment3"] != null)
        //            return ViewState["subSegment3"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["subSegment3"] = value;}
        //}
		
        //public string oem4
        //{
        //    get
        //    {
        //        if(ViewState["oem4"] != null)
        //            return ViewState["oem4"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["oem4"] = value;}
        //}
		
        //public string bodyType4
        //{
        //    get
        //    {
        //        if(ViewState["bodyType4"] != null)
        //            return ViewState["bodyType4"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["bodyType4"] = value;}
        //}
		
        //public string subSegment4
        //{
        //    get
        //    {
        //        if(ViewState["subSegment4"] != null)
        //            return ViewState["subSegment4"].ToString();
        //        else
        //            return "";
        //    }
        //    set{ViewState["subSegment4"] = value;}
        //}
												 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{	
			string[] arrTemp = new string[arrSize];
            int c = 0;

			for ( int i=1; i <= arrSize - 1; i++ )
			{
				if ( !String.IsNullOrEmpty(Request[ "bike" + i ]) && CommonOpn.CheckId( Request[ "bike" + i ] ) && Request[ "bike" + i ].ToString() != "0" )
				{
					versionId[i-1] = Request[ "bike" + i ];
					
					Trace.Warn("versionId " + i + " "  + versionId[i-1]);
					
					featuredBikeIndex++;
				}
				else 
				{
                    Trace.Warn("QS EMPTY");
					versionId[i-1] = "0";
				}
                Trace.Warn("versionid length: " + versionId.Length.ToString());

                Trace.Warn("versionId " + i + "::: " + versionId[i - 1]);
                                
				if ( versionId[i-1] != "0" )
				{
                    Trace.Warn(versionId[i - 1]);
                    arrTemp[c] = versionId[i - 1];
                    Trace.Warn("arr " + c.ToString() + ": " + arrTemp[c]);
					versions += versionId[i-1] + ",";
                    c += 1;
				}
                Trace.Warn("versions : ", versions);      
			}
            
            Array.Clear(versionId, 0, versionId.Length);
            Array.Copy(arrTemp, versionId, arrTemp.Length);
            
            Trace.Warn("versions :: " + versions);
		
			if ( versions.Length > 0 )
			{
				versions = versions.Substring( 0, versions.Length - 1 );
				
				// Get version id of the featured bike on the basis of versions selected for comparison
				// There might be multiple featured Bikes available. But only show top 1
				string featuredBike = CompareBikes.GetFeaturedBike(versions);
				
				if(featuredBike != "")
				{
					featuredBikeId = featuredBike.Split('#')[0];
					spotlightUrl = featuredBike.Split('#')[1];
				}
				
				
				// If featured bike available to show.
				// Check if featured bike is already selected by the user.
				if( featuredBikeId != "" && versions.IndexOf(featuredBikeId) < 0 )
				{
					versionId[ featuredBikeIndex ] = featuredBikeId;
					versions += "," + featuredBikeId;
					
                    //if( featuredBikeIndex >= 3 )
                    //{
                    //    ad_160x600.Visible = false;
                    //    compareWidth = "100%";
                    //}
				}
                //else			
                //{
                //    if( featuredBikeIndex > 3 )
                //    {
                //        ad_160x600.Visible = false;
                //        compareWidth = "100%";
                //    }
                //}
			}
			
			Trace.Warn("featuredBikeIndex : " + featuredBikeIndex);
			
			Trace.Warn( "Versions : " + versions );

            if (versions.Length == 0)
            {
                Response.Redirect("/new/compare/",false);//return;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

            ltrDefaultCityName.Text = Configuration.GetDefaultCityName;

			if ( !IsPostBack )
			{			
				FillComparisonData( versions );
                ltrTitle.Text = title;
                pageTitle = title;
                Trace.Warn("title final", title);

                if (HttpContext.Current.Request.ServerVariables["URL"].ToLower().IndexOf("comparecolors.aspx") >= 0)
                {
                    Trace.Warn("compare colors page");
                    GetColors();
                }

                if (HttpContext.Current.Request.ServerVariables["URL"].ToLower().IndexOf("comparefeatures.aspx") >= 0)
                {
                    Trace.Warn("compare features page");
                    GetFeatures();
                }
			}
			
			//GoogleKeywords( versions );
		} // Page_Load

        void FillComparisonData(string versions)
        {
            Trace.Warn("Fetching Comparison Data...");
            //string sql = string.Empty;
            Database db = null;
            SqlCommand cmd = null;
            DataSet ds = null;

            //sql = " SELECT NS.*, SF.*, Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name BikeName, Ma.Name AS MakeName, Mo.Name AS ModelName, Ve.HostURL, Ve.Name AS VersionName, "
            //    + " (SELECT AvgPrice FROM Con_NewBikeNationalPrices WHERE VersionId = Ve.id ) Price, Ve.New "
            //    + " FROM (((( NewBikeSpecifications NS LEFT JOIN NewBikeStandardFeatures SF ON NS.BikeVersionId=SF.BikeVersionId ) "
            //    + " LEFT JOIN BikeVersions Ve ON Ve.Id=NS.BikeVersionId ) "
            //    + " LEFT JOIN BikeModels Mo ON Mo.Id=Ve.BikeModelId ) "
            //    + " LEFT JOIN BikeMakes Ma ON Ma.Id=Mo.BikeMakeId ) "
            //    + " WHERE NS.BikeVersionId IN (" + db.GetInClauseValue(versions, "BikeVersionId", cmd) + ")";

            //Trace.Warn(sql);

            try
            {
                db = new Database();
                cmd = new SqlCommand();

                cmd.CommandText = "GetBikeComparisonDetails";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@condition", SqlDbType.VarChar, 25).Value = "specs";
                cmd.Parameters.Add("@BikeVersions", SqlDbType.VarChar, 20).Value = versions;
                cmd.Parameters.Add("@CityId", SqlDbType.VarChar, 20).Value = Configuration.GetDefaultCityId;

                ds = db.SelectAdaptQry(cmd);

                Trace.Warn("Bike 1 : " + versions);

                int i = 0;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    Trace.Warn(" Data Found");
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        // get index of versionId to be displayed!
                        i = GetVersionIndex(ds.Tables[0].Rows[j]["BikeVersionId"].ToString());

                        Trace.Warn("Index : " + i + "; Version : " + ds.Tables[0].Rows[j]["BikeVersionId"].ToString());

                        bike[i] = ds.Tables[0].Rows[j]["Bike"].ToString();
                        makeName[i] = ds.Tables[0].Rows[j]["Make"].ToString();
                        modelName[i] = ds.Tables[0].Rows[j]["Model"].ToString();
                        ModelMaskingName[i] = ds.Tables[0].Rows[j]["ModelMaskingName"].ToString();
                        MakeMaskingName[i] = ds.Tables[0].Rows[j]["MakeMaskingName"].ToString();
                        versionName[i] = ds.Tables[0].Rows[j]["Version"].ToString();
                        hostURL[i] = ds.Tables[0].Rows[j]["HostURL"].ToString();
                        leadAggregator[i] = Convert.ToBoolean(ds.Tables[0].Rows[j]["IsNew"]) == true ? ds.Tables[0].Rows[j]["BikeVersionId"].ToString() : "";

                        if (ds.Tables[0].Rows[j]["Price"].ToString() != "")
                            price[i] = CommonOpn.FormatNumeric(ds.Tables[0].Rows[j]["Price"].ToString());

                        //if (ds.Tables[0].Rows[j]["Price"].ToString() != "")
                        //{
                        //loan[i] = (int.Parse(ds.Tables[0].Rows[j]["Price"].ToString()) * 0.8).ToString();
                        //emi[i] = CommonOpn.FormatNumeric(CommonOpn.GetEMI(int.Parse(ds.Tables[0].Rows[j]["Price"].ToString())));
                        //}

                        displacement[i] = ds.Tables[0].Rows[j]["Displacement"].ToString();
                        cylinders[i] = ds.Tables[0].Rows[j]["Cylinders"].ToString();
                        maxPower[i] = ds.Tables[0].Rows[j]["MaxPower"].ToString();
                        maximumTorque[i] = ds.Tables[0].Rows[j]["MaximumTorque"].ToString();
                        bore[i] = ds.Tables[0].Rows[j]["Bore"].ToString();
                        stroke[i] = ds.Tables[0].Rows[j]["Stroke"].ToString();
                        valvesPerCylinder[i] = ds.Tables[0].Rows[j]["ValvesPerCylinder"].ToString();
                        fuelDeliverySystem[i] = ds.Tables[0].Rows[j]["FuelDeliverySystem"].ToString();
                        fuelType[i] = ds.Tables[0].Rows[j]["FuelType"].ToString();
                        ignition[i] = ds.Tables[0].Rows[j]["Ignition"].ToString();
                        sparkPlugsPerCylinder[i] = ds.Tables[0].Rows[j]["SparkPlugsPerCylinder"].ToString();
                        coolingSystem[i] = ds.Tables[0].Rows[j]["CoolingSystem"].ToString();
                        gearboxType[i] = ds.Tables[0].Rows[j]["GearboxType"].ToString();
                        noOfGears[i] = ds.Tables[0].Rows[j]["NoOfGears"].ToString();
                        transmissionType[i] = ds.Tables[0].Rows[j]["TransmissionType"].ToString();
                        clutch[i] = ds.Tables[0].Rows[j]["Clutch"].ToString();
                        performance_0_60_kmph[i] = ds.Tables[0].Rows[j]["Performance_0_60_kmph"].ToString();
                        performance_0_80_kmph[i] = ds.Tables[0].Rows[j]["Performance_0_80_kmph"].ToString();
                        performance_0_40_m[i] = ds.Tables[0].Rows[j]["Performance_0_40_m"].ToString();
                        topSpeed[i] = ds.Tables[0].Rows[j]["TopSpeed"].ToString();
                        performance_60_0_kmph[i] = ds.Tables[0].Rows[j]["Performance_60_0_kmph"].ToString();
                        performance_80_0_kmph[i] = ds.Tables[0].Rows[j]["Performance_80_0_kmph"].ToString();
                        kerbWeight[i] = ds.Tables[0].Rows[j]["KerbWeight"].ToString();
                        overallLength[i] = ds.Tables[0].Rows[j]["OverallLength"].ToString();
                        overallWidth[i] = ds.Tables[0].Rows[j]["OverallWidth"].ToString();
                        overallHeight[i] = ds.Tables[0].Rows[j]["OverallHeight"].ToString();
                        wheelbase[i] = ds.Tables[0].Rows[j]["Wheelbase"].ToString();
                        groundClearance[i] = ds.Tables[0].Rows[j]["GroundClearance"].ToString();
                        seatHeight[i] = ds.Tables[0].Rows[j]["SeatHeight"].ToString();
                        fuelTankCapacity[i] = ds.Tables[0].Rows[j]["FuelTankCapacity"].ToString();
                        reserveFuelCapacity[i] = ds.Tables[0].Rows[j]["ReserveFuelCapacity"].ToString();
                        fuelEfficiencyOverall[i] = ds.Tables[0].Rows[j]["FuelEfficiencyOverall"].ToString();
                        fuelEfficiencyRange[i] = ds.Tables[0].Rows[j]["FuelEfficiencyRange"].ToString();
                        chassisType[i] = ds.Tables[0].Rows[j]["ChassisType"].ToString();
                        frontSuspension[i] = ds.Tables[0].Rows[j]["FrontSuspension"].ToString();
                        rearSuspension[i] = ds.Tables[0].Rows[j]["RearSuspension"].ToString();
                        brakeType[i] = ds.Tables[0].Rows[j]["BrakeType"].ToString();
                        frontDisc[i] = ds.Tables[0].Rows[j]["FrontDisc"].ToString();
                        frontDisc_DrumSize[i] = ds.Tables[0].Rows[j]["FrontDisc_DrumSize"].ToString();
                        rearDisc[i] = ds.Tables[0].Rows[j]["RearDisc"].ToString();
                        rearDisc_DrumSize[i] = ds.Tables[0].Rows[j]["RearDisc_DrumSize"].ToString();
                        calliperType[i] = ds.Tables[0].Rows[j]["CalliperType"].ToString();
                        wheelSize[i] = ds.Tables[0].Rows[j]["WheelSize"].ToString();
                        frontTyre[i] = ds.Tables[0].Rows[j]["FrontTyre"].ToString();
                        rearTyre[i] = ds.Tables[0].Rows[j]["RearTyre"].ToString();
                        tubelessTyres[i] = ds.Tables[0].Rows[j]["TubelessTyres"].ToString();
                        radialTyres[i] = ds.Tables[0].Rows[j]["RadialTyres"].ToString();
                        alloyWheels[i] = ds.Tables[0].Rows[j]["AlloyWheels"].ToString();
                        electricSystem[i] = ds.Tables[0].Rows[j]["ElectricSystem"].ToString();
                        battery[i] = ds.Tables[0].Rows[j]["Battery"].ToString();
                        headlightType[i] = ds.Tables[0].Rows[j]["HeadlightType"].ToString();
                        headlightBulbType[i] = ds.Tables[0].Rows[j]["HeadlightBulbType"].ToString();
                        brake_Tail_Light[i] = ds.Tables[0].Rows[j]["Brake_Tail_Light"].ToString();
                        turnSignal[i] = ds.Tables[0].Rows[j]["TurnSignal"].ToString();
                        passLight[i] = ds.Tables[0].Rows[j]["PassLight"].ToString();
                    }

                    Trace.Warn("price : " + price[0] + "," + price[1]);
                }
            }
            catch (SqlException err)
            {
                Trace.Warn("errrrrrr::: " + err.Message);
                Exception ex = new Exception(err.Message + "---- versions ----- " + versions);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn("errrrrrr::: " + err.Message);
                Exception ex = new Exception(err.Message + "---- versions ----- " + versions);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            HttpContext.Current.Trace.Warn("title : ", title);
            try
            {
                int hideStart = arrSize;
                HttpContext.Current.Trace.Warn("title : ", title);
                for (int i = 0; i < arrSize; i++)
                {
                    if (bike[i] != null)
                    {
                        if (featuredBikeIndex != i)
                        {
                            title += bike[i] + " vs ";
                            youHere += "bike" + (i + 1).ToString() + "=" + versionId[i] + "&";
                        }
                    }
                    else
                    {
                        if (hideStart == arrSize) hideStart = i;
                    }
                }

                HideComparison(hideStart); // Hide unwanted colmuns! 

                if (title.Length > 2)
                    title = title.Substring(0, title.Length - 3); // format title. remove extra vs from end.

                HttpContext.Current.Trace.Warn("titlejgjdsf : ", title);

                if (youHere.Length > 0)
                    youHere = youHere.Substring(0, youHere.Length - 1); // format you are here link. remove extra & from end.
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + "---- versions ----- " + versions);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // End of FillComparisonData method
		
        //void FillComparisonData( string versions )
        //{
        //    Trace.Warn( "Fetching Comparison Data..." );
        //    string sql;
        //    Database db = new Database();
        //    SqlCommand cmd = new SqlCommand();

        //    sql = " SELECT NS.*, SF.*, Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name BikeName, Ma.Name AS MakeName, Mo.Name AS ModelName, Ve.HostURL, Ve.Name AS VersionName, "
        //        + " (SELECT AvgPrice FROM Con_NewBikeNationalPrices WHERE VersionId = Ve.id ) Price, Ve.New "
        //        + " FROM (((( NewBikeSpecifications NS LEFT JOIN NewBikeStandardFeatures SF ON NS.BikeVersionId=SF.BikeVersionId ) "
        //        + " LEFT JOIN BikeVersions Ve ON Ve.Id=NS.BikeVersionId ) "
        //        + " LEFT JOIN BikeModels Mo ON Mo.Id=Ve.BikeModelId ) "
        //        + " LEFT JOIN BikeMakes Ma ON Ma.Id=Mo.BikeMakeId ) "
        //        + " WHERE NS.BikeVersionId IN (" + db.GetInClauseValue(versions, "BikeVersionId", cmd) +")";
				
        //    Trace.Warn( sql );
			
        //    cmd.CommandText = sql;
        //    DataSet ds = new DataSet();
			
        //    try
        //    {
        //        ds = db.SelectAdaptQry(cmd);
				
        //        Trace.Warn( "Bike 1 : " + versionId[0] );
				
        //        int i = 0;
				
        //        if(ds.Tables[0].Rows.Count > 0)
        //        {
					
        //            Trace.Warn("Data Found");
        //            for ( int j = 0; j < ds.Tables[0].Rows.Count; j++ )
        //            {
        //                // get index of versionId to be displayed!
        //                i = GetVersionIndex( ds.Tables[0].Rows[j]["BikeVersionId"].ToString() );
	
        //                Trace.Warn( "Index : " + i + "; Version : " + ds.Tables[0].Rows[j]["BikeVersionId"].ToString() );

        //                bike[i] = ds.Tables[0].Rows[j]["BikeName"].ToString();
        //                makeName[i] = ds.Tables[0].Rows[j]["MakeName"].ToString();
        //                modelName[i] = ds.Tables[0].Rows[j]["ModelName"].ToString();
        //                versionName[i] = ds.Tables[0].Rows[j]["VersionName"].ToString();
        //                hostURL[i] = ds.Tables[0].Rows[j]["HostURL"].ToString();
						
        //                if(ds.Tables[0].Rows[j]["Price"].ToString() != "")
        //                    price[i] 					= CommonOpn.FormatNumeric( ds.Tables[0].Rows[j]["Price"].ToString() ); 
											
        //                if ( ds.Tables[0].Rows[j]["Price"].ToString() != "" )
        //                {
        //                    loan[i] = ( int.Parse( ds.Tables[0].Rows[j]["Price"].ToString() ) * 0.8 ).ToString(); 
        //                    emi[i] = CommonOpn.FormatNumeric( CommonOpn.GetEMI( int.Parse( ds.Tables[0].Rows[j]["Price"].ToString() ) ) );
        //                }
						
        //                length[i] 					= ds.Tables[0].Rows[j]["length"].ToString(); 
        //                width[i] 					= ds.Tables[0].Rows[j]["width"].ToString(); 
        //                height[i] 					= ds.Tables[0].Rows[j]["height"].ToString(); 
        //                wheelBase[i] 				= ds.Tables[0].Rows[j]["WheelBase"].ToString(); 
        //                groundClearance[i] 			= ds.Tables[0].Rows[j]["GroundClearance"].ToString(); 
        //                frontTrack[i] 				= ds.Tables[0].Rows[j]["FrontTrack"].ToString();
        //                rearTrack[i] 				= ds.Tables[0].Rows[j]["RearTrack"].ToString(); 
        //                headRoom[i] 				= ds.Tables[0].Rows[j]["FrontHeadRoom"].ToString(); 
        //                frontLegRoom[i] 			= ds.Tables[0].Rows[j]["FrontLegRoom"].ToString(); 
        //                rearLegRoom[i] 				= ds.Tables[0].Rows[j]["RearLegRoom"].ToString(); 
        //                bootSpace[i] 				= ds.Tables[0].Rows[j]["BootSpace"].ToString(); 
        //                grossWeight[i] 				= ds.Tables[0].Rows[j]["GrossWeight"].ToString();
        //                kerbWeight[i] 				= ds.Tables[0].Rows[j]["KerbWeight"].ToString(); 
        //                seatingCapacity[i] 			= ds.Tables[0].Rows[j]["SeatingCapacity"].ToString(); 
        //                fuelTankCapacity[i]			= ds.Tables[0].Rows[j]["FuelTankCapacity"].ToString(); 
        //                doors[i]					= ds.Tables[0].Rows[j]["Doors"].ToString(); 
        //                engineType[i] 				= ds.Tables[0].Rows[j]["EngineType"].ToString();
        //                displacement[i] 			= ds.Tables[0].Rows[j]["Displacement"].ToString(); 					
        //                power[i]					= ds.Tables[0].Rows[j]["Power"].ToString(); 
        //                torque[i]					= ds.Tables[0].Rows[j]["Torque"].ToString(); 
        //                valveMechanism[i] 			= ds.Tables[0].Rows[j]["ValueMechanism"].ToString(); 
        //                bore[i] 					= ds.Tables[0].Rows[j]["bore"].ToString(); 
        //                stroke[i] 					= ds.Tables[0].Rows[j]["Stroke"].ToString(); 
        //                compressionRatio[i]			= ds.Tables[0].Rows[j]["CompressionRatio"].ToString(); 
        //                noOfCylinders[i] 			= ds.Tables[0].Rows[j]["NoOfCylinders"].ToString(); 
        //                cylinderConfiguration[i] 	= ds.Tables[0].Rows[j]["CylinderConfiguration"].ToString(); 
        //                valvesPerCylinder[i] 		= ds.Tables[0].Rows[j]["ValvesPerCylinder"].ToString(); 
        //                ignitionType[i] 			= ds.Tables[0].Rows[j]["IgnitionType"].ToString(); 
        //                blockMaterial[i] 			= ds.Tables[0].Rows[j]["EngineBlockMaterial"].ToString(); 
        //                headMaterial[i] 			= ds.Tables[0].Rows[j]["BlockHeadMaterial"].ToString(); 
        //                fuelSystem[i] 				= ds.Tables[0].Rows[j]["FuelSystem"].ToString();
        //                gears[i] 					= ds.Tables[0].Rows[j]["Speeds"].ToString(); 
        //                maximumSpeed[i] 			= ds.Tables[0].Rows[j]["MaxSpeed"].ToString(); 
        //                clutchType[i] 				= ds.Tables[0].Rows[j]["ClutchType"].ToString(); 
        //                gearRatio[i] 				= ds.Tables[0].Rows[j]["FinalGearReductionRatio"].ToString(); 
        //                frontSuspension[i] 			= ds.Tables[0].Rows[j]["SuspensionFront"].ToString(); 
        //                rearSuspension[i] 			= ds.Tables[0].Rows[j]["SuspensionRear"].ToString(); 
        //                steeringType[i] 			= ds.Tables[0].Rows[j]["SteeringType"].ToString(); 
        //                powerAssisted[i] 			= ds.Tables[0].Rows[j]["PowerAssisted"].ToString(); 
        //                turningRadius[i] 			= ds.Tables[0].Rows[j]["MinimumTurningRadius"].ToString(); 
        //                brakeType[i] 				= ds.Tables[0].Rows[j]["BrakesType"].ToString(); 
        //                frontBrakes[i] 				= ds.Tables[0].Rows[j]["BrakesFront"].ToString(); 
        //                rearBrakes[i] 				= ds.Tables[0].Rows[j]["BrakesRear"].ToString(); 
        //                wheelSize[i] 				= ds.Tables[0].Rows[j]["WheelSize"].ToString(); 
        //                wheelType[i] 				= ds.Tables[0].Rows[j]["WheelType"].ToString(); 
        //                tyres[i] 					= ds.Tables[0].Rows[j]["Tyres"].ToString();
						
        //                transmissionType[i]			= ds.Tables[0].Rows[j]["TransmissionType"].ToString();
        //                fuelType[i]					= ds.Tables[0].Rows[j]["FuelType"].ToString();
						
        //                mileageHighway[i]			= ds.Tables[0].Rows[j]["MileageHighway"].ToString();
        //                mileageCity[i]				= ds.Tables[0].Rows[j]["MileageCity"].ToString();
        //                mileageOverall[i]			= ds.Tables[0].Rows[j]["MileageOverall"].ToString();
						
        //                rearShoulder[i]				= ds.Tables[0].Rows[j]["RearShoulder"].ToString();
        //                zeroToHundred[i]			= ds.Tables[0].Rows[j]["ZeroToHundred"].ToString();
        //                quarterMile[i]				= ds.Tables[0].Rows[j]["QuarterMile"].ToString();
        //                hundredToZero[i]			= ds.Tables[0].Rows[j]["BrakingHundredToZero"].ToString();
        //                eightyToZero[i]				= ds.Tables[0].Rows[j]["BrakingEightyToZero"].ToString();
	
        //                cfAC[i]						= ds.Tables[0].Rows[j]["AirConditioner"].ToString();
        //                cfPW[i]						= ds.Tables[0].Rows[j]["PowerWindows"].ToString();	
        //                cfDoorLocks[i] 				= ds.Tables[0].Rows[j]["PowerDoorLocks"].ToString();
        //                cfPowerSteering[i]			= ds.Tables[0].Rows[j]["PowerSteering"].ToString();
        //                cfABS[i] 					= ds.Tables[0].Rows[j]["ABS"].ToString();
        //                cfSteeringAdjust[i]			= ds.Tables[0].Rows[j]["SteeringAdjustment"].ToString();	
        //                cfTacho[i] 					= ds.Tables[0].Rows[j]["Tachometer"].ToString();
        //                cfLocks[i]					= ds.Tables[0].Rows[j]["ChildSafetyLocks"].ToString(); 
        //                cfFogLights[i]				= ds.Tables[0].Rows[j]["FrontFogLights"].ToString(); 
        //                cfDefogger[i]				= ds.Tables[0].Rows[j]["Defogger"].ToString(); 
        //                cfLeather[i]				= ds.Tables[0].Rows[j]["LeatherSeats"].ToString();
        //                cfPowerSeats[i] 			= ds.Tables[0].Rows[j]["PowerSeats"].ToString();
        //                cfRadio[i] 					= ds.Tables[0].Rows[j]["Radio"].ToString();
        //                cfCassette[i] 				= ds.Tables[0].Rows[j]["CassettePlayer"].ToString();
        //                cfCD[i] 					= ds.Tables[0].Rows[j]["CDPlayer"].ToString();
        //                cfSun[i] 					= ds.Tables[0].Rows[j]["SunRoof"].ToString();
        //                cfMoon[i]					= ds.Tables[0].Rows[j]["MoonRoof"].ToString();
						
        //                cfTraction[i]				= ds.Tables[0].Rows[j]["TractionControl"].ToString();	
        //                cfDriverAirBags[i]			= ds.Tables[0].Rows[j]["DriverAirBags"].ToString();
        //                cfPassengerAirBags[i]		= ds.Tables[0].Rows[j]["PassengerAirBags"].ToString();
        //                cfImmobilizer[i]			= ds.Tables[0].Rows[j]["Immobilizer"].ToString();
        //                cfCupHolders[i]				= ds.Tables[0].Rows[j]["CupHolder"].ToString();
        //                cfFoldingRearSeat[i]		= ds.Tables[0].Rows[j]["SplitFoldingRearSeats"].ToString();
        //                cfRearWiper[i]				= ds.Tables[0].Rows[j]["RearWashWiper"].ToString();
        //                cfAlloy[i]					= ds.Tables[0].Rows[j]["AlloyWheels"].ToString();
        //                cfTubeless[i]				= ds.Tables[0].Rows[j]["TubelessTyres"].ToString();
        //                cfCL[i]						= ds.Tables[0].Rows[j]["CentralLocking"].ToString();
        //                cfRemoteBoot[i]				= ds.Tables[0].Rows[j]["RemoteBootFuelLid"].ToString();
						
        //                leadAggregator[i]			= Convert.ToBoolean(ds.Tables[0].Rows[j]["New"]) == true ? ds.Tables[0].Rows[j]["BikeVersionId"].ToString() : "";
        //            }	
        //        }			
        //    }
        //    catch ( Exception err )
        //    {
        //        Trace.Warn(err.Message);
        //        Exception ex = new Exception( err.Message + "---- versions ----- " + versions );
        //        ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
			
        //    try
        //    {
        //        int hideStart = arrSize;
					
        //        for ( int i = 0; i < arrSize; i++ )
        //        {
        //            if ( bike[i] != null )
        //            {
        //                if( featuredBikeIndex != i )
        //                {						
        //                    title += bike[i] + " vs ";						
        //                    youHere += "bike" + ( i + 1 ).ToString() + "=" + versionId[i] + "&";
        //                }
        //            }
        //            else
        //            {
        //                if ( hideStart == arrSize ) hideStart = i;	
        //            }					
        //        }
				
        //        HideComparison( hideStart ); // Hide unwanted colmuns! 
				
        //        if(title.Length > 2)
        //            title = title.Substring( 0, title.Length - 3 ); // format title. remove extra vs from end.
				
        //        if(youHere.Length > 0)
        //            youHere = youHere.Substring( 0, youHere.Length - 1 ); // format you are here link. remove extra & from end.
        //    }
        //    catch ( Exception err )
        //    {
        //        Trace.Warn(err.Message);
        //        Exception ex = new Exception( err.Message + "---- versions ----- " + versions );
        //        ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //}
		
		// This function will get current versionId from
		// dataset and will match it with the version in
		// the array. if it matches, it will return index.
		// this is just to match the order of Bikes being compared!

		private int GetVersionIndex( string versionNo )
		{
			int index = -1;
			
			for ( int i = 0; i < arrSize; i++ )
			{
				Trace.Warn( "Version Passed - Version Array - Index : " + versionNo + "-" + versionId[i] + "-" + i  );
				
				if ( versionNo == versionId[i] )
				{
					//Trace.Warn( "Version Passed - Version Array - Index : " + versionNo + "-" + versionId[i] + "-" + i  );
					index = i;
					break;
				}
			}
			
			return index;
		}

        protected void GetFeatures()
        {
            Database db = null;
            DataSet ds = null;

            try 
            {
                db = new Database();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetBikeComparisonDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@condition", SqlDbType.VarChar, 25).Value = "Features";
                    cmd.Parameters.Add("@BikeVersions", SqlDbType.VarChar, 20).Value = versions;
                    cmd.Parameters.Add("@CityId", SqlDbType.VarChar, 20).Value = Configuration.GetDefaultCityId;

                    ds = db.SelectAdaptQry(cmd);

                    int i = 0;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Trace.Warn("Features Data Found");
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            // get index of versionId to be displayed!
                            i = GetVersionIndex(ds.Tables[0].Rows[j]["BikeVersionId"].ToString());

                            Trace.Warn("Index : " + i + "; Version : " + ds.Tables[0].Rows[j]["BikeVersionId"].ToString());

                            bike[i] = ds.Tables[0].Rows[j]["Bike"].ToString();
                            makeName[i] = ds.Tables[0].Rows[j]["Make"].ToString();
                            modelName[i] = ds.Tables[0].Rows[j]["Model"].ToString();
                            ModelMaskingName[i] = ds.Tables[0].Rows[j]["ModelMaskingName"].ToString();
                            MakeMaskingName[i] = ds.Tables[0].Rows[j]["MakeMaskingName"].ToString();
                            versionName[i] = ds.Tables[0].Rows[j]["Version"].ToString();
                            hostURL[i] = ds.Tables[0].Rows[j]["HostURL"].ToString();
                            leadAggregator[i] = Convert.ToBoolean(ds.Tables[0].Rows[j]["IsNew"]) == true ? ds.Tables[0].Rows[j]["BikeVersionId"].ToString() : "";

                            if (ds.Tables[0].Rows[j]["Price"].ToString() != "")
                                price[i] = CommonOpn.FormatNumeric(ds.Tables[0].Rows[j]["Price"].ToString());

                            speedometer[i] = ds.Tables[0].Rows[j]["Speedometer"].ToString();
                            tachometer[i] = ds.Tables[0].Rows[j]["Tachometer"].ToString();
                            tachometerType[i] = ds.Tables[0].Rows[j]["TachometerType"].ToString();
                            shiftLight[i] = ds.Tables[0].Rows[j]["ShiftLight"].ToString();
                            electricStart[i] = ds.Tables[0].Rows[j]["ElectricStart"].ToString();
                            tripmeter[i] = ds.Tables[0].Rows[j]["Tripmeter"].ToString();
                            noOfTripmeters[i] = ds.Tables[0].Rows[j]["NoOfTripmeters"].ToString();
                            tripmeterType[i] = ds.Tables[0].Rows[j]["TripmeterType"].ToString();
                            lowFuelIndicator[i] = ds.Tables[0].Rows[j]["LowFuelIndicator"].ToString();
                            lowOilIndicator[i] = ds.Tables[0].Rows[j]["LowOilIndicator"].ToString();
                            lowBatteryIndicator[i] = ds.Tables[0].Rows[j]["LowBatteryIndicator"].ToString();
                            fuelGauge[i] = ds.Tables[0].Rows[j]["FuelGauge"].ToString();
                            digitalFuelGauge[i] = ds.Tables[0].Rows[j]["DigitalFuelGauge"].ToString();
                            pillionSeat[i] = ds.Tables[0].Rows[j]["PillionSeat"].ToString();
                            pillionFootrest[i] = ds.Tables[0].Rows[j]["PillionFootrest"].ToString();
                            pillionBackrest[i] = ds.Tables[0].Rows[j]["PillionBackrest"].ToString();
                            pillionGrabrail[i] = ds.Tables[0].Rows[j]["PillionGrabrail"].ToString();
                            standAlarm[i] = ds.Tables[0].Rows[j]["StandAlarm"].ToString();

                            steppedSeat[i] = ds.Tables[0].Rows[j]["SteppedSeat"].ToString();
                            antilockBrakingSystem[i] = ds.Tables[0].Rows[j]["AntilockBrakingSystem"].ToString();
                            killswitch[i] = ds.Tables[0].Rows[j]["Killswitch"].ToString();
                            clock[i] = ds.Tables[0].Rows[j]["Clock"].ToString();
                            colors[i] = ds.Tables[0].Rows[j]["Colors"].ToString();

                            Trace.Warn("speedometer : ", speedometer[i]);
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + "---- versions ----- " + versions);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + "---- versions ----- " + versions);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
        }   // End of GetFeaures method

        protected string ShowFormatedData(string value)
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

        public string ShowFeature(string featureValue)
        {
            string adString = "";

            if (String.IsNullOrEmpty(featureValue))
                return "--";

            switch (featureValue)
            {
                case "True":
                    adString = "<img align=\"absmiddle\" src=\"http://img.carwale.com/images/icons/tick.gif\" />";
                    break;
                case "False":
                    adString = "<img align=\"absmiddle\" src=\"http://img.carwale.com/images/icons/delete.ico\" />";
                    break;
                default:
                    adString = "-";
                    break;
            }
            Trace.Warn("adstring : ", adString);
            Trace.Warn("adstring 1: ", "http://img.carwale.com/images/icons/tick.gif");
            Trace.Warn("adstring 2: ", "http://img.carwale.com/images/icons/delete.ico");
            return adString;
        }   // End of ShowFeature method

        //public string GetFeature( string featureValue )
        //{
        //    string adString = "";
			
        //    if ( featureValue == null )
        //        return "";
			
        //    switch( featureValue.ToUpper() )
        //    {
        //        case "A":
        //            adString = "<img align=\"absmiddle\" src=\"" + Bikewale.Common.ImagingFunctions.GetRootImagePath() + "/images/icons/tick.gif\" />";
        //            break;
        //        case "O":
        //            adString = "Optional";
        //            break;
        //        case "N":
        //            adString = "<img align=\"absmiddle\" src=\"" + Bikewale.Common.ImagingFunctions.GetRootImagePath() + "/images/icons/delete.ico\" />";
        //            break;
        //        case "-":
        //            adString = "-";
        //            break;
        //        default:
        //            adString = "-";
        //            break;
        //    }
			
        //    return adString;
        //}
		
		private void HideComparison( int column )
		{
			
			while ( arrSize - column > 0 )
			{	
				for ( int i=0; i < tblCompare.Rows.Count; i++ )
				{
					tblCompare.Rows[i].Cells.RemoveAt( tblCompare.Rows[i].Cells.Count - 1 );
				}
				
				column++; // One column has been deleted, increase the index by 1.
			}
		}
		
		public void GetColors()
		{
            Database db = null;
            SqlCommand cmd = null;
            //DataSet ds = null;
            
            try
            {
                db = new Database();
                cmd = new SqlCommand();

                cmd.CommandText = "GetBikeComparisonDetails";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@condition", SqlDbType.VarChar, 25).Value = "Colors";
                cmd.Parameters.Add("@BikeVersions", SqlDbType.VarChar, 20).Value = versions;
                cmd.Parameters.Add("@CityId", SqlDbType.VarChar, 20).Value = Configuration.GetDefaultCityId;

                dsColors = db.SelectAdaptQry(cmd);
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
		}

        /// <summary>
        ///     PopulateWhere will return formatted color html for the particular version
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        protected string GetModelColors(string versionId, int index)
        {
            string colorString = String.Empty;

            DataView dv = dsColors.DefaultViewManager.CreateDataView(dsColors.Tables[0]);
            dv.Sort = "BikeVersionId";
            //Trace.Warn("drv count", dv.Count.ToString());
            DataRowView []drv = dv.FindRows(versionId);
            
            if(drv.Length > 0)
            {
                Trace.Warn("drv data .............", drv.Length.ToString());

                for (int jTmp = 0; jTmp < drv.Length; jTmp++)
                {
                    colorString += "<div style='width:100; text-align:center;padding:5px;'> "
                                + " <div style='border:1px solid #dddddd;width:50px;margin:auto;background-color:#" + drv[jTmp].Row["HexCode"].ToString() + "'>"
                                + " <img src='http://img.carwale.com/images/spacer.gif' width='50' height='45' /></div> "
                                + " <div style='padding-top:3px;'>" + drv[jTmp].Row["Color"].ToString() + "</div></div> ";
                }

                leadAggregator[index] = Convert.ToBoolean(drv[0].Row["IsNew"]) == true ? drv[0].Row["BikeVersionId"].ToString() : "";
            }
            return colorString;

        }
		
		//public string GetVersionRatings(string versionId)
//		{
//			string sql = "";
//			
//			sql = " SELECT MO.ID as ModelId, IsNull(MO.ReviewRate, 0) AS ModelRate, IsNull(MO.ReviewCount, 0) AS ModelTotal, "
//				+ " IsNull(CV.ReviewRate, 0) AS VersionRate, IsNull(CV.ReviewCount, 0) AS VersionTotal "
//				+ " FROM BikeModels AS MO, BikeVersions AS CV WHERE CV.ID = @ID AND MO.ID = CV.BikeModelId ";
//			
//			SqlCommand cmd =  new SqlCommand(sql);
//			cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = versionId;
//	
//			SqlDataReader dr;
//			Database db = new Database();
//			
//			string reviewString = "";
//			
//			try
//			{
//				dr = db.SelectQry(cmd);
//				
//				while( dr.Read() )
//				{
//					if( Convert.ToDouble(dr["VersionRate"]) > 0 )
//					{
//						string reviews = Convert.ToDouble(dr["VersionTotal"]) > 1 ? " reviews" : " review";
//						reviewString += "<div align='center'>" + CommonOpn.GetRateImage( Convert.ToDouble(dr["VersionRate"]) ) + "</div>"
//									 + " <div style='margin-top:10px;' align='center'><a href='/Research/ReadUserReviews-Bikev-"+ versionId +".html'>"+ dr["VersionTotal"].ToString() + reviews +"</a></div>";
//					}
//					else
//						reviewString = "<div style='margin-top:10px;' align='center'><a href='/Research/UserReviews-Bikev-"+ versionId +".html'>Write a review</a></div>";
//				}			
//				dr.Close();
//				db.CloseConnection();
//			}
//			catch ( SqlException err )
//			{
//				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
//				objErr.SendMail();
//			}
//			return reviewString;
//		}
		
		public string GetModelRatings(string versionId)
		{
			string sql = "";

            sql = " SELECT (SELECT MaskingName FROM BikeMakes With(NoLock) WHERE ID = MO.BikeMakeId) AS MakeMaskingName, MO.ID as ModelId, MO.Name AS ModelName,MO.MaskingName AS ModelMaskingName, IsNull(MO.ReviewRate, 0) AS ModelRate, IsNull(MO.ReviewCount, 0) AS ModelTotal, "
				+ " IsNull(CV.ReviewRate, 0) AS VersionRate, IsNull(CV.ReviewCount, 0) AS VersionTotal "
                + " FROM BikeModels AS MO, BikeVersions AS CV With(NoLock) WHERE CV.ID = @ID AND MO.ID = CV.BikeModelId ";
			
			SqlCommand cmd =  new SqlCommand(sql);
			cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = versionId;
			
			SqlDataReader dr = null;
			Database db = new Database();
			
			string reviewString = "";

            try
            {
                dr = db.SelectQry(cmd);

                while (dr.Read())
                {
                    if (Convert.ToDouble(dr["ModelRate"]) > 0)
                    {
                        string reviews = Convert.ToDouble(dr["ModelTotal"]) > 1 ? " reviews" : " review";
                        //reviewString += "<div align='center'>" + CommonOpn.GetRateImage(Convert.ToDouble(dr["ModelRate"].ToString())) + "</div>"
                        //									 + " <div style='margin-top:10px;' align='center'><a href='/Research/ReadUserReviews-Bikem-"+ dr["ModelId"].ToString() +".html'>"+ dr["ModelTotal"].ToString() + reviews +" </a></div>";
                        reviewString += "<div>" + CommonOpn.GetRateImage(Convert.ToDouble(dr["ModelRate"].ToString())) + "</div>"
                                     + " <div style='margin-top:5px;'><a href='/" + dr["MakeMaskingName"].ToString() + "-bikes/" +dr["ModelMaskingName"].ToString() + "/user-reviews/'>" + dr["ModelTotal"].ToString() + reviews + " </a></div>";

                    }
                    else
                        reviewString = "<div style='margin-top:10px;'><a href='/content/userreviews/writereviews.aspx?bikem=" + dr["ModelId"].ToString() + "'>Write a review</a></div>";
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally 
            {
                if (dr != null)
                {
                    dr.Close();
                }
                db.CloseConnection();
            }
			return reviewString;
		}
		
		public string GetAllFeatures(string versionId)
		{
			string sql = "";
						
			sql = " SELECT NC.Id AS CategoryId, NC.Name As Category "
                + " FROM NewBikeFeatureCategories NC With(NoLock) WHERE NC.Id IN "
                + " (SELECT DISTINCT CategoryId FROM NewBikeFeatures NF, NewBikeFeatureItems NI With(NoLock) "
				+ " WHERE NF.FeatureItemId=NI.Id AND BikeVersionId=@BikeVersionId )";
			
			SqlCommand cmd =  new SqlCommand(sql);
			cmd.Parameters.Add("@BikeVersionId", SqlDbType.BigInt).Value = versionId;
			
		
			DataSet dsCt = new DataSet();
			DataSet dsF = new DataSet();
			Database db = new Database();
			
			string prepareStr = "";
			
			try
			{
				dsCt = db.SelectAdaptQry(cmd);

                sql = " SELECT NI.ID, NI.Name Feature, CategoryId FROM NewBikeFeatures NF, NewBikeFeatureItems NI With(NoLock) "
					+ " WHERE NF.FeatureItemId=NI.Id AND BikeVersionId=@BikeVersionId";
				
				SqlCommand cmd1 =  new SqlCommand(sql);
				cmd1.Parameters.Add("@BikeVersionId", SqlDbType.BigInt).Value = versionId;
			
				dsF = db.SelectAdaptQry(cmd1);
				
				foreach( DataRow row in dsCt.Tables[0].Rows )
				{
					prepareStr += "<br><h2>" + row["Category"].ToString() + "</h2>";
					
					if( dsF.Tables[0].Rows.Count > 0 )
					{
						DataTable dtFeatures = dsF.Tables[0];
						DataRow[] rowFeatures = dtFeatures.Select( "CategoryId = "+  row["CategoryId"].ToString() );
						
						if( rowFeatures.Length > 0 )
						{
							prepareStr += "<ul class=\"normal\">";
							
							for( int i = 0; i < rowFeatures.Length; i++ )
							{
								prepareStr += "<li>" + rowFeatures[i]["Feature"].ToString() + "</li>";
							}
							
							prepareStr += "</ul>";
						}
					}//if
				}//foeeach
			}
			catch ( SqlException err )
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			
			return prepareStr;
		}
		
        //protected void GoogleKeywords(string versions)
        //{
        //    string sql = "";
        //    SqlCommand cmd = new SqlCommand();
        //    Database db = new Database();
        //    sql = " SELECT CM.Name AS Make, Se.Name AS SubSegment, Bo.Name BikeBodyStyle "
        //        + " FROM BikeModels AS CMO, BikeMakes AS CM, BikeBodyStyles Bo, "
        //        + " (BikeVersions Ve LEFT JOIN BikeSubSegments Se ON Se.Id = Ve.SubSegmentId ) "
        //        + " WHERE CM.ID=CMO.BikeMakeId AND CMO.ID=Ve.BikeModelId AND Bo.Id=Ve.BodyStyleId "
        //        + " AND Ve.Id in (" + db.GetInClauseValue(versions, "Id", cmd) + ")";
			
			
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        cmd.CommandText = sql;
        //        ds = db.SelectAdaptQry(cmd );
				
        //        if(ds.Tables[0].Rows.Count > 0)
        //        {
        //            for ( int j = 0; j < ds.Tables[0].Rows.Count; j++ )
        //            {
        //                if (j==0)
        //                {
        //                    oem1 = ds.Tables[0].Rows[j]["Make"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                    bodyType1 = ds.Tables[0].Rows[j]["BikeBodyStyle"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                    subSegment1 = ds.Tables[0].Rows[j]["SubSegment"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                }
        //                if (j==1)
        //                {
        //                    oem2 = ds.Tables[0].Rows[j]["Make"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                    bodyType2 = ds.Tables[0].Rows[j]["BikeBodyStyle"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                    subSegment2 = ds.Tables[0].Rows[j]["SubSegment"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                }
        //                if (j==2)
        //                {
        //                    oem3 = ds.Tables[0].Rows[j]["Make"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                    bodyType3 = ds.Tables[0].Rows[j]["BikeBodyStyle"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                    subSegment3 = ds.Tables[0].Rows[j]["SubSegment"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                }
        //                if (j==3)
        //                {
        //                    oem4 = ds.Tables[0].Rows[j]["Make"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                    bodyType4 = ds.Tables[0].Rows[j]["BikeBodyStyle"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                    subSegment4 = ds.Tables[0].Rows[j]["SubSegment"].ToString().Replace(" ", "").Replace("/","").Replace("-",""); 
        //                }
						
        //            }	
        //        }			
				
        //        Trace.Warn(sql);
        //    }
        //    catch(Exception err)
        //    {
        //        Trace.Warn(err.Message);
        //        ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    finally
        //    {
        //        db.CloseConnection();
        //    }
        //}
		
		public string IsFeaturedBike(string index)	
		{
			if( featuredBikeIndex.ToString() == index)
			{
				Trace.Warn("Appluting calls");
				return "fearured-bike";
			}
			else 
				return "";
		}
		
		
		public string GetLandingURL(string makeName, string modelName, string versionName, string versionId)
		{
			string landingUrl = "";
			
			if(!(versionId == featuredBikeId && spotlightUrl != ""))
			{
                landingUrl = "/" + UrlRewrite.FormatSpecial(makeName) + "-bikes/" + modelName + "/";
						   //+ (versionName == "" ? "" : (UrlRewrite.FormatSpecial(versionName) + "-specs-" + versionId + ".html"));
			}
			else
			{
				landingUrl = spotlightUrl;
			}
			return landingUrl;
		}
	} // class
} // namespace