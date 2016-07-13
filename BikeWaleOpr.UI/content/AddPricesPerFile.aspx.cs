using BikeWaleOpr.Common;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Xml;

namespace BikeWaleOpr.Content
{
    public class AddPricesPerFile : Page
    {
        public bool finished = false;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            finished = ProcessFile();

        } // Page_Load

        bool ProcessFile()
        {
            bool exist = false;

            string dirPath = Server.MapPath("/content/mappingfiles/parsedpricefiles/");

            if (Directory.Exists(dirPath))
            {
                string[] files = Directory.GetFiles(dirPath);

                if (files.Length > 0)
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

            while (xr.Read())
            {
                switch (xr.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xr.Name)
                        {
                            case "bike":
                                string cityId = xr.GetAttribute("cityId");
                                string bikeId = xr.GetAttribute("bikeId");
                                string price = xr.GetAttribute("price");

                                if (price != "")
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

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("insertshowroomprices"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int64, bikeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaiprice", DbType.Int64, price));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaiinsurance", DbType.Int64, insurance));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbairto", DbType.Int64, rto));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaicorporaterto", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetprice", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetinsurance", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetrto", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetcorporaterto", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int64, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastupdated", DbType.DateTime, DateTime.Now));

                    //run the command
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }

            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + " : " + cityId + " : " + bikeId + " : " + price + " : " + insurance.ToString() + " : " + rto.ToString());

                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + " : " + cityId + " : " + bikeId + " : " + price + " : " + insurance.ToString() + " : " + rto.ToString());

                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }
    }//Class
}// Namespace