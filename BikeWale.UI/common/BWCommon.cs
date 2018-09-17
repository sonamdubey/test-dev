/*
    This class will use to bind controls like filling makes, states
    Written by: Satish Sharma On Jan 21, 2008 12:28 PM
*/

using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Bikewale.Common
{
    /*
        This class will contain all the common functions used in Bikewale		
    */
    public class BWCommon
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        //selectName is the value which is to be at the top of the dropdown
        public void MaintainDropDownViewState(DropDownList drp, string content, string selectedValue, string selectName)
        {
            char _delimiter = '|';

            drp.Items.Clear();
            drp.Enabled = true;

            //add Any at the top
            drp.Items.Add(new ListItem(selectName, "0"));

            if (content != "")
            {
                string[] listItems = content.Split(_delimiter);

                for (int i = 0; i < listItems.Length - 1; i++)
                {
                    drp.Items.Add(new ListItem(listItems[i], listItems[i + 1]));
                    i++;
                }

                ListItem selectedListItem = drp.Items.FindByValue(selectedValue);
                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }

        // make type is what kind of makes do you want
        // i.e. New/Used/All
        public DataSet GetBikeMakes(string makeType)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "BWCommon.GetBikeMakes");
            
            return null;

            //DataSet ds = new DataSet();
            //Database db = new Database();

            //string sql = "";

            //sql = " SELECT ID AS Value, Name AS Text FROM BikeMakes With(NoLock) WHERE IsDeleted = 0 ";

            //if( makeType == "New" )	
            //    sql += " AND New = 1 ";
            //else if( makeType == "Used" )
            //    sql += " AND Used = 1 ";

            //sql += "ORDER BY Text ";

            //try
            //{
            //    ds = db.SelectAdaptQry(sql);
            //}
            //catch(Exception err)
            //{
            //    ErrorClass.LogError(err,"AjaxFunctions.GetNewMakes");
            //    
            //}

            //return ds;
        }

        // make type is what kind of makes do you want
        // i.e. New/Used/All
        public DataSet GetBikeModels(string makeId)
        {

            ErrorClass.LogError(new Exception("Method not used/commented"), "BWCommon.GetBikeModels");
            
            return null;
            //if( CommonOpn.CheckId(makeId) == false )
            //{
            //    return null;
            //}

            //DataSet ds = new DataSet();
            //Database db = new Database();

            //string sql = "";

            //sql = " SELECT ID AS Value, Name AS Text FROM BikeModels With(NoLock) WHERE IsDeleted = 0 AND BikeMakeId = @BikeMakeId ORDER BY Text ";

            //SqlParameter [] param ={new SqlParameter("@BikeMakeId", makeId)};
            //try
            //{
            //    ds = db.SelectAdaptQry(sql, param);
            //}
            //catch(Exception err)
            //{
            //    ErrorClass.LogError(err,"AjaxFunctions.GetNewMakes");
            //    
            //}

            //return ds;
        }

        /* This function returns DataSet of new Bike Makes(New = 1) */
        public DataSet GetNewMakes()
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "BWCommon.GetNewMakes");
            
            return null;

            //DataSet ds = new DataSet();
            //Database db = new Database();

            //string sql = "";

            ////sql = " SELECT ID AS Value, Name AS Text FROM BikeMakes WHERE IsDeleted = 0 "
            ////    + " AND New = 1 ORDER BY Text ";

            //// Modified By : Ashish G. Kamble
            //// Added Futuristic = 0 in the query to check whether Bike is launched or not.

            //sql = " SELECT ID AS Value, Name AS Text FROM BikeMakes With(NoLock) WHERE IsDeleted = 0 "
            //    + " AND New = 1 AND Futuristic = 0 ORDER BY Text ";
            //try
            //{
            //    ds = db.SelectAdaptQry(sql);
            //}
            //catch(Exception err)
            //{
            //    ErrorClass.LogError(err,"AjaxFunctions.GetNewMakes");
            //    
            //}

            //return ds;
        }

        /* This function returns DataSet of new Bike models(New = 1) */
        public DataSet GetNewModels(string makeId)
        {
            DataSet ds = new DataSet();

            if (makeId == "" || CommonOpn.CheckId(makeId) == false)
                return ds;

            string sql = "";

            sql = " select ID AS Value, Name AS Text from bikemodels  where isdeleted = 0 and bikemakeid = @bikemakeid and new = 1 order by text ";

            DbParameter[] param = new[]
            {
                DbFactory.GetDbParam("@bikemakeid", DbType.Int32,makeId )
            };

            try
            {
                ds = MySqlDatabase.SelectAdapterQuery(sql, param, ConnectionType.ReadOnly);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "AjaxFunctions.GetNewModels");
                
            }

            return ds;
        }

        // make type is what kind of makes do you want
        // i.e. New/Used/All
        public DataSet GetBikeVersions(string modelId, string makeType)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "BWCommon.GetBikeVersions");
            
            return null;

            //DataSet ds = new DataSet();
            //Database db = new Database();

            //string sql = "";

            //sql = " SELECT ID AS Value, Name AS Text FROM BikeVersions With(NoLock) WHERE IsDeleted = 0 AND BikeModelId = @BikeModelId ";

            //SqlParameter [] param ={new SqlParameter("@BikeModelId", modelId)};

            //if( makeType == "New" )	
            //    sql += " AND New = 1 ";
            //else if( makeType == "Used" )
            //    sql += " AND Used = 1 ";

            //sql += " ORDER BY Text ";

            //try
            //{
            //    ds = db.SelectAdaptQry(sql, param);
            //}
            //catch(Exception err)
            //{
            //    ErrorClass.LogError(err,"AjaxFunctions.GetNewMakes");
            //    
            //}

            //return ds;
        }

        public string GetBikeName(string versionId)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "BWCommon.GetBikeName");
            
            return string.Empty;

            //string sql = "", bikeName = "";

            //sql = " select ( mk.name +' '+ mo.name +' '+ vs.name ) Bike from bikeversions vs, bikemodels mo, bikemakes mk  With(NoLock) "
            //    + " where vs.bikemodelid = mo.id and mo.bikemakeid = mk.id and vs.id = @versionId";

            //SqlParameter [] param ={new SqlParameter("@versionId", versionId)};

            //SqlDataReader dr = null;	
            //Database db = new Database();

            //try
            //{
            //    dr = db.SelectQry(sql, param);

            //    if( dr.Read() )
            //    {
            //        bikeName = dr["Bike"].ToString();
            //    }

            //}
            //catch(Exception err)
            //{
            //    ErrorClass.LogError(err,"AjaxFunctions.GetNewMakes");
            //    
            //}
            //finally
            //{
            //    if(dr != null)
            //        dr.Close();

            //    db.CloseConnection();
            //}

            //return bikeName;
        }

        public string Get_Make_Model_Version(string versionId, out string makeName, out string modelName, out string versionName, out string makeId, out string modelId)
        {


            //string sql = "";

            makeName = "";
            modelName = "";
            versionName = "";
            makeId = "";
            modelId = "";

            ErrorClass.LogError(new Exception("Method not used/commented"), "BWCommon.Get_Make_Model_Version");
            
            return string.Empty;

            //sql = " SELECT Mk.Name MakeName, Mo.Name ModelName, Vs.Name VersionName, Mk.Id MakeId, Mo.Id ModelName  "
            //    + " FROM BikeVersions Vs, BikeModels Mo, BikeMakes Mk  With(NoLock) "
            //    +  "WHERE Vs.BikeModelId = Mo.Id AND Mo.BikeMakeId = Mk.Id AND Vs.Id = @versionId";

            //SqlDataReader dr = null;	
            //Database db = new Database();
            //SqlParameter [] param ={new SqlParameter("@versionId", versionId)};

            //try
            //{
            //    dr = db.SelectQry(sql, param);

            //    if( dr.Read() )
            //    {
            //        makeName = dr["MakeName"].ToString();
            //        modelName = dr["ModelName"].ToString();
            //        versionName = dr["VersionName"].ToString();
            //        makeId = dr["MakeId"].ToString();
            //        modelId = dr["ModelId"].ToString();
            //    }

            //}
            //catch(Exception err)
            //{
            //    ErrorClass.LogError(err,"AjaxFunctions.GetNewMakes");
            //    
            //}
            //finally
            //{
            //    if(dr != null)
            //        dr.Close();

            //    db.CloseConnection();
            //}

            //return makeName +" "+ modelName +" "+ versionName;
        }

        //this function adds the selected ids as comma separated values,
        //and returns it to the calling function 
        //at the last one comma is concatnated
        public static string GetSelectedItemRepeater(Repeater rpt, string chk, string lbl)
        {
            string strRet;		//the concated values to be returned
            strRet = "";		//initializes to blank
            CheckBox objChkControl;
            Label objID;

            foreach (RepeaterItem item in rpt.Items)
            {
                objChkControl = (CheckBox)item.FindControl(chk);
                objID = (Label)item.FindControl(lbl);
                if (objChkControl.Checked == true)
                {
                    //now check the type. if it is 1 then check that it shuld not be in ignored list
                    //and if it is 2 then check tht it should not be in associate list. 
                    //else add the counts in the deniedCount
                    string id = objID.Text;

                    //concat the id
                    if (strRet == "")
                        strRet = id;
                    else
                        strRet += "," + id;

                }
            }

            return strRet;
        }

        //to get static makes from xml file
        //this is created to be used in new layout for research 
        public static DataSet GetStaticMakes()
        {
            DataSet ds = new DataSet();
            try
            {
                ds.ReadXml(HttpContext.Current.Server.MapPath("~/XMLFeed/Makes.xml"));
                return ds;
            }
            catch (Exception err)
            {
                ds = null;
                ErrorClass.LogError(err, "GetStaticMakes");
                
            }
            return ds;
        }

        //to get static Body from xml file
        //this is created to be used in new layout for research 
        public static DataSet GetStaticBodyStyles()
        {
            DataSet ds = new DataSet();
            try
            {
                ds.ReadXml(HttpContext.Current.Server.MapPath("~/XMLFeed/BodyStyles.xml"));
                return ds;
            }
            catch (Exception err)
            {
                ds = null;
                ErrorClass.LogError(err, "GetStaticBodyStyles");
                
            }
            return ds;
        }

        public static DataSet GetStaticUpcomingBikes()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                AddUpcomingBikeColumns(ref dt);
                AddUpcomingBikeRows(ref dt);
                ds.Tables.Add(dt);
            }
            catch (Exception err)
            {
                ds = null;
                ErrorClass.LogError(err, "GetStaticUpcomingBikes");
                
            }
            return ds;
        }

        private static void AddUpcomingBikeColumns(ref DataTable dt)
        {
            DataColumn Id, PhotoName, Bike, ExpectedLaunch, EstimatedPrice;

            Id = new DataColumn();
            Id.ColumnName = "Id";
            dt.Columns.Add(Id);

            PhotoName = new DataColumn();
            PhotoName.ColumnName = "PhotoName";
            dt.Columns.Add(PhotoName);

            Bike = new DataColumn();
            Bike.ColumnName = "Bike";
            dt.Columns.Add(Bike);

            ExpectedLaunch = new DataColumn();
            ExpectedLaunch.ColumnName = "ExpectedLaunch";
            dt.Columns.Add(ExpectedLaunch);

            EstimatedPrice = new DataColumn();
            EstimatedPrice.ColumnName = "EstimatedPrice";
            dt.Columns.Add(EstimatedPrice);
        }

        private static void AddUpcomingBikeRows(ref DataTable dt)
        {
            try
            {
                string id = "", bike = "", photoName = "", expectedLaunch = "", estimatedPrice = "";
                string imgPath;
                if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower().IndexOf("bikewale.com") >= 0)
                {
                    imgPath = "https://img.aeplcdn.com/Contents/uc.xml";
                }
                else
                {
                    imgPath = "https://server/Contents/uc.xml";
                }

                XmlTextReader reader = new XmlTextReader(imgPath);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            switch (reader.Name)
                            {
                                case "Id":
                                    id = reader.ReadString();
                                    break;
                                case "Bike":
                                    bike = reader.ReadString();
                                    break;
                                case "PhotoName":
                                    photoName = reader.ReadString();
                                    break;
                                case "ExpectedLaunch":
                                    expectedLaunch = reader.ReadString();
                                    break;
                                case "EstimatedPrice":
                                    estimatedPrice = reader.ReadString();
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case XmlNodeType.EndElement:

                            switch (reader.Name)
                            {
                                case "UpcomingBike":
                                    DataRow dRow = null;
                                    dRow = dt.NewRow();
                                    dRow["Id"] = id;
                                    dRow["Bike"] = bike;
                                    dRow["PhotoName"] = photoName;
                                    dRow["ExpectedLaunch"] = expectedLaunch;
                                    dRow["EstimatedPrice"] = estimatedPrice;
                                    dt.Rows.Add(dRow);
                                    id = "";
                                    bike = "";
                                    photoName = "";
                                    expectedLaunch = "";
                                    estimatedPrice = "";
                                    break;
                                default:
                                    break;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }

        public static DataSet GetStaticRoadTests()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                AddRoadTestColumns(ref dt);
                AddRoadTestRows(ref dt);
                ds.Tables.Add(dt);
            }
            catch (Exception err)
            {
                ds = null;
                ErrorClass.LogError(err, "GetStaticRoadTests");
                
            }
            return ds;
        }

        private static void AddRoadTestColumns(ref DataTable dt)
        {
            DataColumn DetailUrl, MainImgPath, Bike;

            DetailUrl = new DataColumn();
            DetailUrl.ColumnName = "DetailUrl";
            dt.Columns.Add(DetailUrl);

            MainImgPath = new DataColumn();
            MainImgPath.ColumnName = "MainImgPath";
            dt.Columns.Add(MainImgPath);

            Bike = new DataColumn();
            Bike.ColumnName = "Bike";
            dt.Columns.Add(Bike);
        }

        private static void AddRoadTestRows(ref DataTable dt)
        {
            try
            {
                string detailUrl = "", mainImgPath = "", bike = "";
                string imgPath;
                if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower().IndexOf("bikewale.com") >= 0)
                {
                    imgPath = "https://img.aeplcdn.com/Contents/rt.xml";
                }
                else
                {
                    imgPath = "https://server/Contents/rt.xml";
                }

                XmlTextReader reader = new XmlTextReader(imgPath);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            switch (reader.Name)
                            {
                                case "DetailUrl":
                                    detailUrl = reader.ReadString();
                                    break;
                                case "MainImgPath":
                                    mainImgPath = reader.ReadString();
                                    break;
                                case "Bike":
                                    bike = reader.ReadString();
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case XmlNodeType.EndElement:

                            switch (reader.Name)
                            {
                                case "RoadTest":
                                    DataRow dRow = null;
                                    dRow = dt.NewRow();
                                    dRow["DetailUrl"] = detailUrl;
                                    dRow["MainImgPath"] = mainImgPath;
                                    dRow["Bike"] = bike;
                                    dt.Rows.Add(dRow);
                                    detailUrl = "";
                                    mainImgPath = "";
                                    bike = "";
                                    break;
                                default:
                                    break;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }

        public static DataSet GetStaticNewLaunches()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                AddNewLaunchesColumns(ref dt);
                AddNewLaunchesRows(ref dt);
                ds.Tables.Add(dt);
            }
            catch (Exception err)
            {
                ds = null;
                ErrorClass.LogError(err, "GetStaticNewLaunches");
                
            }
            return ds;
        }

        private static void AddNewLaunchesColumns(ref DataTable dt)
        {
            DataColumn Make, Model, ModelId, SmallPic;

            Make = new DataColumn();
            Make.ColumnName = "Make";
            dt.Columns.Add(Make);

            Model = new DataColumn();
            Model.ColumnName = "Model";
            dt.Columns.Add(Model);

            ModelId = new DataColumn();
            ModelId.ColumnName = "ModelId";
            dt.Columns.Add(ModelId);

            SmallPic = new DataColumn();
            SmallPic.ColumnName = "SmallPic";
            dt.Columns.Add(SmallPic);
        }

        private static void AddNewLaunchesRows(ref DataTable dt)
        {
            try
            {
                string make = "", model = "", modelId = "", smallPic = "";
                string imgPath;
                if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower().IndexOf("bikewale.com") >= 0)
                {
                    imgPath = "https://img.aeplcdn.com/Contents/nl.xml";
                }
                else
                {
                    imgPath = "https://server/Contents/nl.xml";
                }

                XmlTextReader reader = new XmlTextReader(imgPath);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            switch (reader.Name)
                            {
                                case "Make":
                                    make = reader.ReadString();
                                    break;
                                case "Model":
                                    model = reader.ReadString();
                                    break;
                                case "ModelId":
                                    modelId = reader.ReadString();
                                    break;
                                case "SmallPic":
                                    smallPic = reader.ReadString();
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case XmlNodeType.EndElement:

                            switch (reader.Name)
                            {
                                case "NewLaunch":
                                    DataRow dRow = null;
                                    dRow = dt.NewRow();
                                    dRow["Make"] = make;
                                    dRow["Model"] = model;
                                    dRow["ModelId"] = modelId;
                                    dRow["SmallPic"] = smallPic;
                                    dt.Rows.Add(dRow);
                                    make = "";
                                    model = "";
                                    modelId = "";
                                    smallPic = "";
                                    break;
                                default:
                                    break;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }

        public bool IsSearchEngine()
        {
            bool ret = false;
            try
            {
                if (HttpContext.Current.Request.Browser.Crawler)
                    ret = true;
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "IsSearchEngine");
                
            }
            return ret;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 16 Oct 2014
        /// Summary    : PopulateWhere to save EMI Assistance requests to database
        /// MOdified By : Sadhana on 17 Oct 2014
        /// Summary : to trim name , email,phone no
        /// </summary>
        /// <param name="custName">Customer name</param>
        /// <param name="email">Customer email</param>
        /// <param name="mobile">Customer mobile</param>
        /// <param name="modelId">model selected by the customer</param>
        /// <param name="selectedCityId"></param>
        /// <param name="leadtype"></param>
        /// <returns></returns>
        public bool SaveEMIAssistaneRequest(string custName, string email, string mobile, string modelId, string selectedCityId, string leadtype)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "BWCommon.SaveEMIAssistaneRequest");
            
            return false;

            //bool isSaved = false;
            //SqlCommand cmd = null;
            //Database db = null;

            ////Carwale.Service.Indigo.Web.HDFC_Bank_C2TSoapClient objTableSpool = new Carwale.Service.Indigo.Web.HDFC_Bank_C2TSoapClient();

            //int _cityId = selectedCityId == "" ? 0 : Convert.ToInt32(selectedCityId);

            //if (custName.Length > 0 && mobile.Length > 0)
            //{
            //    try
            //    {   
            //        db = new Database();

            //        using(cmd = new SqlCommand())
            //        {
            //            cmd.CommandText = "InsertEmiAssistaceRequets";
            //            cmd.CommandType = CommandType.StoredProcedure;

            //            cmd.Parameters.Add("@modelId", SqlDbType.BigInt).Value = modelId;
            //            cmd.Parameters.Add("@custName", SqlDbType.VarChar, 100).Value = custName.Trim();
            //            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email.Trim();
            //            cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = mobile.Trim();
            //            cmd.Parameters.Add("@selectedCityId", SqlDbType.Int).Value = _cityId;

            //            if(!String.IsNullOrEmpty(leadtype))
            //                cmd.Parameters.Add("@leadType", SqlDbType.Int).Value = Convert.ToInt32(leadtype);

            //            //if(leadtype=="1")
            //            //    objTableSpool.AddDetails(custName,"",email,"","","","","",mobile,GeoCitiesRepository.GetCityNameById(cityid.ToString()),

            //            if(db.InsertQry(cmd))
            //                isSaved = true;
            //        }
            //    }
            //    catch (SqlException err)
            //    {
            //        HttpContext.Current.Trace.Warn(err.Message);
            //        ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //        
            //    } 
            //    catch (Exception err)
            //    {
            //        HttpContext.Current.Trace.Warn(err.Message);
            //        ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //        
            //    }
            //    finally
            //    {
            //        db.CloseConnection();
            //        cmd.Dispose();
            //    }              
            //}

            //return isSaved;
        }//End of SaveEMIAssistaneRequest
        public static void dummyFunction()
        {
            Gelf4Net.Appender.GelfUdpAppender.GenerateMessageId();
        }
    }//class
}//namespace
