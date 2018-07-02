using Bikewale.Cache.Core;
using Bikewale.Interfaces.Cache.Core;
using BikewaleOpr.BAL.BikePricing;
using BikewaleOpr.Cache.BikeData;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.DALs.BikePricing;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Interface.Dealers;
using BikeWaleOpr.Common;
using log4net;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.UI;
using System.Xml;

namespace BikeWaleOpr.Content
{
    public class AddPricesPerFile : Page
    {
        public bool finished = false;
        static readonly ILog _logger = LogManager.GetLogger("BulkPriceUpload");
        static IUnityContainer _container = new UnityContainer();

        static AddPricesPerFile()
        {
            _container.RegisterType<IBwPrice, BwPrice>();
            _container.RegisterType<IShowroomPricesRepository, BikeShowroomPrices>();
            _container.RegisterType<IBikeModelsRepository, BikeModelsRepository>();
            _container.RegisterType<IBikeVersionsCacheRepository, BikeVersionsCacheRepository>();
            _container.RegisterType<ICacheManager, MemcacheManager>();
            _container.RegisterType<IBikeVersions, BikeVersionsRepository>();
        }

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
            DateTime dt1 = DateTime.Now, dt2;
            bool exist = false;

            string dirPath = Server.MapPath("/content/mappingfiles/parsedpricefiles/");

            try
            {
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
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
            }
            finally
            {
                dt2 = DateTime.Now;
                ThreadContext.Properties["Old_BulkPrices_TotalTime"] = (dt2 - dt1).TotalMilliseconds;
                _logger.Error("Old_BulkPrices_SavePrices");
            }

            return !exist;
        }

        void ParseFile(string fileName)
        {
            //parse the xml file and then save it into the database
            //get all the mapped cities and the versions
            XmlTextReader xr = new XmlTextReader(fileName);
            xr.WhitespaceHandling = WhitespaceHandling.None;

            try
            {
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
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
            }
        }

        /// <summary>
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Changed SP from 'insertshowroomprices_28062017' to 'insertshowroomprices_30082017', removed catch(SqlException)
        /// Modified By : Deepak Israni on 22 Feb 2018
        /// Description : Added call to create a new document for the bikewalepricingindex (ES Index)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="bikeId"></param>
        /// <param name="price"></param>
        void SaveData(string cityId, string bikeId, string price)
        {

            //get the new insurance and the new RTO
            double insurance = CommonOpn.GetInsurancePremium(bikeId, cityId, Convert.ToDouble(price));
            double rto = CommonOpn.GetRegistrationCharges(bikeId, cityId, Convert.ToDouble(price));
            
            try
            {
                
                using (DbCommand cmd = DbFactory.GetDBCommand("insertshowroomprices_30082017"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int64, bikeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaiprice", DbType.Int64, price));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaiinsurance", DbType.Int64, Convert.ToUInt64(insurance)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbairto", DbType.Int64, Convert.ToUInt64(rto)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaicorporaterto", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetprice", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetinsurance", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetrto", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mumbaimetcorporaterto", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int64, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastupdated", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int16, -1));
                    //run the command
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }                     
                IBwPrice bwPrice = _container.Resolve<IBwPrice>();
                bwPrice.UpdateModelPriceDocument(bikeId, cityId);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + "city: " + cityId + " bike : " + bikeId + " price: " + price + " insurance: " + insurance.ToString() + " rto: " + rto.ToString());

                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            } // catch Exception
            
        }
    }//Class
}// Namespace