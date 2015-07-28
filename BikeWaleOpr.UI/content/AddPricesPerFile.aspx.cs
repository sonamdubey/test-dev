using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.IO;
using BikeWaleOpr.Common;
using System.Xml;

namespace BikeWaleOpr.Content
{
	public class AddPricesPerFile : Page
	{
		public bool finished = false;
		
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
			finished = ProcessFile();
					
		} // Page_Load
		
		bool ProcessFile()
		{
			bool exist = false;
			
			string dirPath = Server.MapPath("/content/mappingfiles/parsedpricefiles/");
			
			if( Directory.Exists(dirPath))
			{
				string[] files = Directory.GetFiles(dirPath);
				
				if(files.Length > 0)
				{
					exist = true;
					
					Trace.Warn("Parsing file : " + files[0]);
					ParseFile(files[0]);
					
					Trace.Warn("Deleting file : " + files[0]);
					//delete this file
					File.Delete(files[0]);
				}
			}
			
			return !exist;
		}
		
		void ParseFile(string fileName)
		{
			//parse the xml file and then save it into the database
			//get all the mapped cities and the versions
			XmlTextReader xr = new XmlTextReader(fileName);
			xr.WhitespaceHandling = WhitespaceHandling.None;
			
			while(xr.Read())
			{
				switch(xr.NodeType)
				{
					case XmlNodeType.Element:
					switch(xr.Name)
					{
						case "bike":
							string cityId = xr.GetAttribute("cityId");
							string bikeId = xr.GetAttribute("bikeId");
							string price = xr.GetAttribute("price");
							
							if(price != "")
							{
								SaveData(cityId, bikeId, price);
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
		
		
		void SaveData(string cityId, string bikeId, string price)
		{
			//get the new insurance and the new RTO
			double insurance = CommonOpn.GetInsurancePremium(bikeId, cityId, Convert.ToDouble(price));
			double rto = CommonOpn.GetRegistrationCharges(bikeId, cityId, Convert.ToDouble(price));
			
			Trace.Warn("Saving data for : " + cityId + " : " + bikeId + " : " + price + " : " + insurance.ToString() + " : " + rto.ToString());

			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
			CommonOpn op = new CommonOpn();
				
			string conStr = db.GetConString();

			try
			{
                using (con = new SqlConnection(conStr))
                {
                    Trace.Warn("Submitting Data");
                    cmd = new SqlCommand("InsertShowroomPrices", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    prm = cmd.Parameters.Add("@BikeVersionId", SqlDbType.BigInt);
                    prm.Value = bikeId;

                    prm = cmd.Parameters.Add("@MumbaiPrice", SqlDbType.BigInt);
                    prm.Value = price;

                    prm = cmd.Parameters.Add("@MumbaiInsurance", SqlDbType.BigInt);
                    prm.Value = insurance;

                    prm = cmd.Parameters.Add("@MumbaiRTO", SqlDbType.BigInt);
                    prm.Value = rto;

                    prm = cmd.Parameters.Add("@CityId", SqlDbType.BigInt);
                    prm.Value = cityId;

                    prm = cmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime);
                    prm.Value = DateTime.Now;

                    con.Open();
                    //run the command
                    cmd.ExecuteNonQuery();
                        
                    //close the connection	
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
			}
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + " : " + cityId + " : " + bikeId + " : " + price + " : " + insurance.ToString() + " : " + rto.ToString());

                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
			catch(Exception err)
			{
				Trace.Warn(err.Message);
				Exception ex = new Exception(err.Message + " : " + cityId + " : " + bikeId + " : " + price + " : " + insurance.ToString() + " : " + rto.ToString());
					
				ErrorClass objErr = new ErrorClass(ex,Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
	}//Class
}// Namespace