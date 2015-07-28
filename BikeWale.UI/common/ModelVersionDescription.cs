using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ModelDescription
/// </summary>

namespace Bikewale.Common
{
    public class ModelVersionDescription
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }
        public string MakeMaskingName { get; set; }
        public string VersionName { get; set; }
        public string HostUrl { get; set; }
        public string LargePic { get; set; }
        public string SmallPic { get; set; }
        public double ModelRatingLooks { get; set; }
        public double ModelRatingPerformance { get; set; }
        public double ModelRatingComfort { get; set; }
        public double ModelRatingValueForMoney { get; set; }
        public double ModelRatingFuelEconomy { get; set; }
        public double ModelRatingOverall { get; set; }
        public double ModelReviewCount { get; set; }
        public string BikeName { get; set; }
        public bool IsFuturistic { get; set; }
        public bool IsNew { get; set; }
        public bool IsUsed { get; set; }
        public string ModelBasePrice { get; set; }
        public string ModelHighendPrice { get; set; }

        public string DefaultCity {get { return System.Configuration.ConfigurationManager.AppSettings["DefaultCity"]; } }
        /// <summary>
        /// Modified By : Suresh Prajapati on 22 Aug 2014
		/// Summary : to retrieve isnew and isused flag
        /// </summary>
        /// <param name="modelId"></param>
        public void GetDetailsByModel(string modelId)
        {
            Database db = new Database();
            SqlCommand cmd = new SqlCommand("GetModelDescription");
            cmd.CommandType = CommandType.StoredProcedure;
            HttpContext.Current.Trace.Warn("modelId : " + modelId);
            HttpContext.Current.Trace.Warn("DefaultCity : " + DefaultCity);
            cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
            cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = DefaultCity;
            SqlDataReader dr = null;
            try
            {
                dr = db.SelectQry(cmd);

                if (dr.Read())
                {
                  
                    MakeName = dr["MakeName"].ToString();                   
                    ModelName = dr["ModelName"].ToString();
                    ModelMaskingName = dr["ModelMaskingName"].ToString();
                    MakeMaskingName = dr["MakeMaskingName"].ToString();
                    HostUrl = dr["HostURL"].ToString();
                    LargePic = dr["LargePic"].ToString();
                    SmallPic = dr["SmallPic"].ToString();                    
                    ModelRatingLooks = dr["Looks"].ToString() != "" ? Convert.ToDouble(dr["Looks"]) : 0;
                    ModelRatingPerformance = dr["Performance"].ToString() != "" ? Convert.ToDouble(dr["Performance"]) : 0;
                    ModelRatingComfort = dr["Comfort"].ToString() != "" ? Convert.ToDouble(dr["Comfort"]) : 0;
                    ModelRatingValueForMoney = dr["ValueForMoney"].ToString() != "" ? Convert.ToDouble(dr["ValueForMoney"]) : 0;
                    ModelRatingFuelEconomy = dr["FuelEconomy"].ToString() != "" ? Convert.ToDouble(dr["FuelEconomy"]) : 0;
                    ModelRatingOverall = dr["ReviewRate"].ToString() != "" ? Convert.ToDouble(dr["ReviewRate"]) : 0;
                    ModelReviewCount = dr["ReviewCount"].ToString() != "" ? Convert.ToDouble(dr["ReviewCount"]) : 0;
                    BikeName = MakeName + " " + ModelName;
                    IsFuturistic = Convert.ToBoolean(dr["Futuristic"]);
                    IsNew = Convert.ToBoolean(dr["New"]);
                    IsUsed = Convert.ToBoolean(dr["Used"]);
                    ModelBasePrice = dr["MinPrice"].ToString();
                    ModelHighendPrice = dr["MaxPrice"].ToString();
                    
                }
                //if (LargePic != string.Empty)
                //    LargePic = ImagingFunctions.GetImagePathCWImg("/bikes/", HostUrl) + LargePic;

                //if (SmallPic != string.Empty)
                //    SmallPic = ImagingFunctions.GetImagePathCWImg("/bikes/", HostUrl) + SmallPic;
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn("GetModelDetails: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("GetModelDetails: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if(dr != null)
                {
                    dr.Close();
                }
                db.CloseConnection();
            }			
        }

        /// <summary>
        /// Modified By : Suresh Prajapati on 22 Aug 2014
		/// Summary : to retrieve isnew and isused flag
        /// </summary>
        /// <param name="VersionId"></param>
        public void GetDetailsByVersion(string VersionId)
        {
            Database db = new Database();
            SqlCommand cmd = new SqlCommand("GetVersionDescription");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@VesrionId", SqlDbType.Int).Value = VersionId;
            cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = DefaultCity;
            SqlDataReader dr = null;
            try
            {
                dr = db.SelectQry(cmd);

                if (dr != null && dr.Read())
                {

                    MakeName = dr["MakeName"].ToString();
                    ModelName = dr["ModelName"].ToString();
                    ModelMaskingName = dr["ModelMaskingName"].ToString();
                    MakeMaskingName = dr["MakeMaskingName"].ToString();
                    VersionName = dr["VersionName"].ToString();
                    HostUrl = dr["HostURL"].ToString();
                    LargePic = dr["LargePic"].ToString();
                    SmallPic = dr["SmallPic"].ToString();
                    ModelRatingLooks = dr["Looks"].ToString() != "" ? Convert.ToDouble(dr["Looks"]) : 0;
                    ModelRatingPerformance = dr["Performance"].ToString() != "" ? Convert.ToDouble(dr["Performance"]) : 0;
                    ModelRatingComfort = dr["Comfort"].ToString() != "" ? Convert.ToDouble(dr["Comfort"]) : 0;
                    ModelRatingValueForMoney = dr["ValueForMoney"].ToString() != "" ? Convert.ToDouble(dr["ValueForMoney"]) : 0;
                    ModelRatingFuelEconomy = dr["FuelEconomy"].ToString() != "" ? Convert.ToDouble(dr["FuelEconomy"]) : 0;
                    ModelRatingOverall = dr["ReviewRate"].ToString() != "" ? Convert.ToDouble(dr["ReviewRate"]) : 0;
                    ModelReviewCount = dr["ReviewCount"].ToString() != "" ? Convert.ToDouble(dr["ReviewCount"]) : 0;                    
                    BikeName = MakeName + " " + ModelName + " " + VersionName;
                    IsFuturistic = Convert.ToBoolean(dr["Futuristic"]);
                    IsNew = Convert.ToBoolean(dr["New"]);
                    IsUsed = Convert.ToBoolean(dr["Used"]);
                    ModelBasePrice = dr["MinPrice"].ToString();
                    ModelHighendPrice = dr["MaxPrice"].ToString();
                }                
                //if (LargePic != string.Empty)
                //    LargePic = ImagingFunctions.GetImagePathCWImg("/bikes/", HostUrl) + LargePic;

                //if (SmallPic != string.Empty)
                //    SmallPic = ImagingFunctions.GetImagePathCWImg("/bikes/", HostUrl) + SmallPic;
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn("GetDetailsByVersion: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("GetDetailsByVersion: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
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
        }
        
    }//class
}//namespace