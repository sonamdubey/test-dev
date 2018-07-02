using BikeWaleOpr.Common;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace BikeWaleOpr.Content
{
    public class BulkPriceUpload : Page
	{
		Hashtable htC, htV;
		protected DropDownList drpMakes;
		protected HtmlInputFile flUpload;
		protected Button btnUploadFile, butMapUCBikes, btnProcessFile;
		protected HtmlGenericControl spnErr, divStep1, divStep2, divStep3;
		DataSet dsOemP;
		DataSet dsUC;
		DataSet dsUCV;
		public DataSet dsStates, dsModels;
        public DataTable dtStates, dtModels;
		protected Repeater rptUC, rptUCV; 
		public int addedPrice = 0, updatedPrice = 0, ucSerial = 0, ucvSerial = 0;
		ArrayList arUCNames, arUCVNames;
		
		
		public string UnmappedCityNames 
		{
			get
			{
				if(ViewState["UnmappedCityNames"] != null && ViewState["UnmappedCityNames"].ToString() != "")
					return ViewState["UnmappedCityNames"].ToString();
				else
					return "";
			}
			set
			{
				ViewState["UnmappedCityNames"] = value;
			}
		}
		
		public string UnmappedBikeNames 
		{
			get
			{
				if(ViewState["UnmappedBikeNames"] != null && ViewState["UnmappedBikeNames"].ToString() != "")
					return ViewState["UnmappedBikeNames"].ToString();
				else
					return "";
			}
			set
			{
				ViewState["UnmappedBikeNames"] = value;
			}
		}
		
		public string MakeId
		{
			get
			{
				if(ViewState["MakeId"] != null && ViewState["MakeId"].ToString() != "")
					return ViewState["MakeId"].ToString();
				else
					return "-1";
			}
			set
			{
				ViewState["MakeId"] = value;
			}
		}
		
		public string MakeName
		{
			get
			{
				if(ViewState["MakeName"] != null && ViewState["MakeName"].ToString() != "")
					return ViewState["MakeName"].ToString();
				else
					return "-1";
			}
			set
			{
				ViewState["MakeName"] = value;
			}
		}
				
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnUploadFile.Click += new EventHandler(btnUploadFile_Click);
			butMapUCBikes.Click += new EventHandler(butMapUCBikes_Click);
			btnProcessFile.Click += new EventHandler(btnProcessFile_Click);
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            //if ( Request.Cookies["Customer"] == null )
            //        Response.Redirect("../users/login.aspx?ReturnUrl=../content/bulkpriceupload.aspx");
			Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));	
			
			if ( !IsPostBack )
			{
				divStep1.Visible = true;
				divStep2.Visible = false;
				divStep3.Visible = false;
				
                //AjaxFunctions aj = new AjaxFunctions();
                //drpMakes.DataSource = aj.GetMakes();
                MakeModelVersion mmv = new MakeModelVersion();
                drpMakes.DataSource = mmv.GetMakes("NEW");
				drpMakes.DataTextField = "Text";
				drpMakes.DataValueField = "Value";
				drpMakes.DataBind();
				
				drpMakes.Items.Insert(0, new ListItem("Select Make","0"));
			}
		} // Page_Load
		
		
		void btnUploadFile_Click(object sender, EventArgs e)
		{
			if(UploadFile() == true)
			{
				divStep1.Visible = false;
				divStep2.Visible = true;
			}
			else
			{
				divStep1.Visible = true;
				divStep2.Visible = false;
			}
		}
		
		void btnProcessFile_Click(object sender, EventArgs e)
		{
			//load the city xml file into a hashtable
			LoadBikeXmlFile();
			
			//load the version xml file into a hashtable
			LoadCityXmlFile();
			
			//now load the data into a dataset
			LoadOemPriceData(false);
						
			//now process the dealer data file and map the city names, and correspondingly update the status
			ProcessData();
			
			//divStep1.Visible = false;
			divStep2.Visible = false;
			divStep3.Visible = true;
		}

		void butMapUCBikes_Click(object sender, EventArgs e)
		{
			if(UnmappedCityNames != "" || UnmappedBikeNames != "")
			{
				//add the unmapped cities into a arraylist
				arUCNames = new ArrayList();	
				
				string [] cities = UnmappedCityNames.Split(',');
				for(int i = 0; i < cities.Length; i++)
				{
					if(cities[i].Trim() != "")
						arUCNames.Add(cities[i].Trim());
				}
				
				//add the unmapped bikes into a arraylist
				arUCVNames = new ArrayList();	
				
				string [] bikes = UnmappedBikeNames.Split(',');
				for(int i = 0; i < bikes.Length; i++)
				{
					if(bikes[i].Trim() != "")
						arUCVNames.Add(bikes[i].Trim());
				}
				
				//load the city xml file into a hashtable
				LoadBikeXmlFile();
								
				//load the city xml file into a hashtable
				LoadCityXmlFile();
				//now load the price data into a dataset
				LoadOemPriceData(true);
							
				//now process the dealer data file and map the city names, and correspondingly update the status
				ProcessData();
			}
		}
		
		
		void ProcessData()
		{
            //AjaxFunctions aj = new AjaxFunctions();
            //dsStates = aj.GetStates();

            try
            {
                MakeModelVersion mmv = new MakeModelVersion();

                ManageStates objStates = new ManageStates();
                dtStates = objStates.FillStates();
                //DataRow drST = dsStates.Tables[0].NewRow();
                DataRow drST = dtStates.NewRow();
                drST["Text"] = "Select State";
                drST["Value"] = "0";

                //dsStates.Tables[0].Rows.InsertAt(drST, 0);
                dtStates.Rows.InsertAt(drST, 0);

                //dsModels = aj.GetModels(MakeId);

                //DataRow drMO = dsModels.Tables[0].NewRow();
                dtModels = mmv.GetModels(MakeId, "NEW");
                DataRow drMO = dtModels.NewRow();
                drMO["Text"] = "Select Model";
                drMO["Value"] = "0";
                
                //dsModels.Tables[0].Rows.InsertAt(drMO, 0);
                dtModels.Rows.InsertAt(drMO, 0);

                //for each of the row in the OemDealer data, first map the city name, if it is mapped then update it into the database.
                //if the city is not mapped

                dsUC = new DataSet();
                dsUCV = new DataSet();

                //initialize datasets to hold temporary datas
                InitializeDataSets();

                Trace.Warn(dsOemP.Tables[0].Rows.Count.ToString());

                //clear all the files from the ParsedPriceFiles folder
                string pPFPath = Server.MapPath("/content/mappingfiles/parsedpricefiles/");
                //create dir if not exist
                if (!Directory.Exists(pPFPath))
                    Directory.CreateDirectory(pPFPath);
                else//empty directory if already exists
                {
                    string[] files = Directory.GetFiles(pPFPath);

                    foreach (string file in files)
                        File.Delete(file);
                }

                int rowCount = 0, fileCount = 0;

                bool opened = false;
                XmlTextWriter xw = null;

                foreach (DataRow dr in dsOemP.Tables[0].Rows)
                {
                    string bikeName = dr["name"].ToString().Trim();
                    string cityName = dr["city"].ToString().Trim();
                    string price = dr["price"].ToString();

                    string cityId = "-1", bikeId = "-1";

                    //get the id for this city
                    if (htC.ContainsKey(cityName) == true)
                        cityId = htC[cityName].ToString();

                    if (htV.ContainsKey(bikeName) == true)
                    {
                        bikeId = htV[bikeName].ToString();
                    }

                    Trace.Warn("cityId : ", cityId);
                    Trace.Warn("bikeId : ", bikeId);                    

                    if (cityId != "-1" && bikeId != "-1" && bikeId != "")
                    {
                        //Trace.Warn("cityName : " + cityName);
                        //check whether th eprice for this make and this city is to be updated or not
                        string makeId = drpMakes.SelectedItem.Value;

                        if (CanBeAdded(makeId, cityId) == true)
                        {
                            updatedPrice++;

                            rowCount++;

                            if (fileCount == 0 && rowCount == 1)
                            {
                                Trace.Warn("stage 1");

                                fileCount++;
                                //now start a new file
                                string fileName = pPFPath + fileCount.ToString() + ".xml";
                                //creates a new xml file
                                xw = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
                                xw.Formatting = Formatting.Indented;

                                xw.WriteStartDocument();
                                xw.WriteStartElement("bikes");

                                opened = true;
                            }

                            if (rowCount > 100)
                            {
                                Trace.Warn("stage 2");

                                fileCount++;
                                rowCount = 0;

                                //ends the old file and then create a new file
                                xw.WriteEndElement();
                                xw.Flush();
                                xw.Close();
                                opened = false;

                                //now start a new file
                                string fileName = pPFPath + fileCount.ToString() + ".xml";
                                //creates a new xml file
                                xw = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
                                xw.Formatting = Formatting.Indented;

                                xw.WriteStartDocument();
                                xw.WriteStartElement("bikes");

                                opened = true;
                            }

                            price = price.Replace(",", "");

                            if (CommonOpn.IsNumeric(price) == true)
                            {
                                xw.WriteStartElement("bike");
                                xw.WriteAttributeString("cityId", cityId);
                                xw.WriteAttributeString("bikeId", bikeId);
                                xw.WriteAttributeString("price", price);
                                xw.WriteEndElement();
                            }
                        }
                    }

                    if (cityId == "-1")
                    {
                        //add these into the dataset for unmapped cities 
                        UnmappedCities(cityName);
                    }

                    if (bikeId == "-1")
                    {
                        //add these into the dataset for unmapped cities 
                        UnmappedBikes(bikeName);

                        Trace.Warn(bikeName);
                    }
                }

                if (opened == true)
                {
                    //close the file
                    xw.WriteEndElement();
                    xw.Flush();
                    xw.Close();
                }

                rptUCV.DataSource = dsUCV;
                rptUCV.DataBind();

                rptUC.DataSource = dsUC;
                rptUC.DataBind();
            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
		}
		
				
		void UnmappedCities(string cityName)
		{
			DataRow drTC;
			
			DataRow [] rows = dsUC.Tables[0].Select("CityName = '" + cityName.Replace("'", "''") + "'");
			if(rows.Length == 0)
			{
				drTC = dsUC.Tables[0].NewRow();
				drTC["CityName"] = cityName;
				
				dsUC.Tables[0].Rows.Add(drTC);
				
				UnmappedCityNames += cityName + ",";
			}
		}
		
		void UnmappedBikes(string bikeName)
		{
			DataRow drTCV;
			
			DataRow [] rows = dsUCV.Tables[0].Select("BikeName = '" + bikeName.Replace("'", "''") + "'");
			if(rows.Length == 0)
			{
				drTCV = dsUCV.Tables[0].NewRow();
				drTCV["BikeName"] = bikeName;
				
				dsUCV.Tables[0].Rows.Add(drTCV);
				
				UnmappedBikeNames += bikeName + ",";
			}
		}
		
				
		bool UploadFile()
		{
			bool uploaded = false;
			//temp image path
			string filePath = Server.MapPath("/content/mappingfiles");

            if (!Directory.Exists(Request.ServerVariables["APPL_PHYSICAL_PATH"] + @"\content\mappingfiles\"))
                Directory.CreateDirectory( Request.ServerVariables["APPL_PHYSICAL_PATH"] + @"\content\mappingfiles\");
            
			try
			{
				if( flUpload.PostedFile.FileName != "")
				{
					string fileName = "PriceData.xml";
					flUpload.PostedFile.SaveAs(filePath + "/" + fileName);
										
					uploaded = true;
					
					MakeId = drpMakes.SelectedItem.Value;
					
					MakeName = drpMakes.SelectedItem.Text;
				}
				else
					uploaded = false;
			}
			catch(Exception err )
			{
				Trace.Warn(err.Message);
				ErrorClass.LogError(err,Request.ServerVariables["URL"]);
				
				uploaded = false;
			} // catch Exception
			
			return uploaded;
		}
				
		void LoadCityXmlFile()
		{
			string filePath = Server.MapPath("/content/mappingfiles/MappedCityData.xml");
			
			//get all the mapped cities and the versions
            using (XmlTextReader xr = new XmlTextReader(filePath))
            {
                xr.WhitespaceHandling = WhitespaceHandling.None;

                htC = new Hashtable();

                while (xr.Read())
                {
                    switch (xr.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (xr.Name)
                            {
                                case "city":
                                    string oemCityName = xr.GetAttribute("OEMCityName");
                                    string bikewaleCityName = xr.GetAttribute("BikewaleCityname");
                                    string bikewaleCityId = xr.GetAttribute("BikewaleCityid");
                                    htC.Add(oemCityName.Trim(), bikewaleCityId);
                                    break;

                                default:
                                    break;
                            }
                            break;

                        default:
                            break;
                    }
                }

                xr.Close();
            }
		}
		
		void LoadBikeXmlFile()
		{
			string filePath = Server.MapPath("/content/mappingfiles/MappedBikeData.xml");
			
			//get all the mapped cities and the versions
            using (XmlTextReader xr = new XmlTextReader(filePath))
            {
                xr.WhitespaceHandling = WhitespaceHandling.None;

                htV = new Hashtable();

                while (xr.Read())
                {
                    switch (xr.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (xr.Name)
                            {
                                case "bike":
                                    string oemBikeName = xr.GetAttribute("OEMBikeName");
                                    string bikewaleBikeName = xr.GetAttribute("BikewaleBikeName");
                                    string bikewaleBikeId = xr.GetAttribute("BikewaleBikeId");
                                    htV.Add(oemBikeName.Trim(), bikewaleBikeId);
                                    break;

                                default:
                                    break;
                            }
                            break;

                        default:
                            break;
                    }
                }

                xr.Close();
            }
		}
		
		void LoadOemPriceData(bool forSpecificConditions)
		{
			string filePath = Server.MapPath("/content/mappingfiles/PriceData.xml");
			
			dsOemP = new DataSet();
			DataTable dt = dsOemP.Tables.Add();
			
			dt.Columns.Add("name", typeof(string));
			dt.Columns.Add("city", typeof(string));
			dt.Columns.Add("price", typeof(string));
			dt.Columns.Add("bikeId", typeof(string));
			dt.Columns.Add("cityId", typeof(string));
						
			//get all the mapped cities and the versions
            using (XmlTextReader xr = new XmlTextReader(filePath))
            {
                xr.WhitespaceHandling = WhitespaceHandling.None;

                DataRow dr;

                while (xr.Read())
                {
                    switch (xr.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (xr.Name)
                            {
                                case "version":
                                    string name = xr.GetAttribute("name");
                                    string price = xr.GetAttribute("price");
                                    string city = xr.GetAttribute("city");

                                    bool canAddRow = false;

                                    if (forSpecificConditions == true && arUCNames.Contains(city) == true)
                                        canAddRow = true;

                                    if (forSpecificConditions == true && arUCVNames.Contains(name) == true)
                                        canAddRow = true;

                                    if (forSpecificConditions == false)
                                        canAddRow = true;

                                    if (canAddRow == true)
                                    {
                                        dr = dt.NewRow();

                                        dr["name"] = name;
                                        dr["city"] = city;
                                        dr["price"] = price;
                                        dr["bikeId"] = "-1";
                                        dr["cityId"] = "-1";
                                        dt.Rows.Add(dr);
                                    }

                                    break;

                                default:
                                    break;
                            }
                            break;

                        default:
                            break;
                    }
                }

                xr.Close();
            }
		}
		
		
		void InitializeDataSets()
		{
			//create the unmappedcities dataset as a clone of the oemdealerdataset
			dsUC = new DataSet();
			DataTable dtUC = dsUC.Tables.Add();
			dtUC.Columns.Add("CityName", typeof(string));
			
			dsUCV = new DataSet();
			DataTable dtUCV = dsUCV.Tables.Add();
			dtUCV.Columns.Add("BikeName", typeof(string));
		}
		
				
		bool CanBeAdded(string makeId, string cityId)
		{
			ArrayList list = new ArrayList();
			
			list.Add("9:1");  		//Mahindra:Mumbai
			list.Add("9:13");  		//Mahindra:Navi Mumbai
			list.Add("9:40");  		//Mahindra:Thane
			list.Add("9:273");  	//Mahindra:Faridabad
			list.Add("9:225");  	//Mahindra:Ghaziabad
			list.Add("9:246");  	//Mahindra:Gurgaon
			list.Add("9:10");  		//Mahindra:New Delhi
			list.Add("9:224");  	//Mahindra:Noida
			list.Add("9:2");  		//Mahindra:Bangalore
			list.Add("9:176");  	//Mahindra:Chennai
			list.Add("9:12");  		//Mahindra:Pune
			list.Add("9:105");  	//Mahindra:Hyderabad
			list.Add("9:128");  	//Mahindra:Ahmedabad
			
			
			list.Add("29:1");  		//Mahindra-Renault:Mumbai
			list.Add("29:13");  	//Mahindra-Renault:Navi Mumbai
			list.Add("29:40");  	//Mahindra-Renault:Thane
			list.Add("29:273");  	//Mahindra-Renault:Faridabad
			list.Add("29:225");  	//Mahindra-Renault:Ghaziabad
			list.Add("29:246");  	//Mahindra-Renault:Gurgaon
			list.Add("29:10");  	//Mahindra-Renault:New Delhi
			list.Add("29:224");  	//Mahindra-Renault:Noida
			list.Add("29:2");  		//Mahindra-Renault:Bangalore
			list.Add("29:176");  	//Mahindra-Renault:Chennai
			list.Add("29:12");  	//Mahindra-Renault:Pune
			list.Add("29:105");  	//Mahindra-Renault:Hyderabad
			list.Add("29:128");  	//Mahindra-Renault:Ahmedabad
			
			//list.Add("2:1");  		//Chevrolet:Mumbai
			//list.Add("2:13");  		//Chevrolet:Navi Mumbai
			//list.Add("2:40");  		//Chevrolet:Thane
			//list.Add("2:273");  	//Chevrolet:Faridabad
			//list.Add("2:225");  	//Chevrolet:Ghaziabad
			//list.Add("2:246");  	//Chevrolet:Gurgaon
			//list.Add("2:10");  		//Chevrolet:New Delhi
			//list.Add("2:224");  	//Chevrolet:Noida
			//list.Add("2:2");  		//Chevrolet:Bangalore
			//list.Add("2:176");  	//Chevrolet:Chennai
			//list.Add("2:12");  		//Chevrolet:Pune
			//list.Add("2:105");  	//Chevrolet:Hyderabad
			//list.Add("2:128");  	//Chevrolet:Ahmedabad
			
			
			bool canBeAdded = true;
			/*
			if(list.IndexOf(makeId + ":" + cityId) != -1)
				canBeAdded = false;
			*/
			//Trace.Warn("CanBeAdded : " + makeId + " : " + cityId + " : " + canBeAdded.ToString());
				
			return canBeAdded;
		}
	}//Class
}// Namespace