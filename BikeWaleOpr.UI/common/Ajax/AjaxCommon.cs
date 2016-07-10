using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using AjaxPro;
using BikeWaleOpr.VO;
using BikeWaleOpr.Classified;
using System.Configuration;
using Enyim.Caching;
using BikewaleOpr.Common;
using BikeWaleOPR.Utilities;
using BikeWaleOPR.DAL.CoreDAL;
using System.Data.Common;

namespace BikeWaleOpr.Common
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 2 Apr 2013
    /// Summary : Class is used for writing common ajax functions using ajaxpro.
    /// </summary>
    public class AjaxCommon
    {
        protected static MemcachedClient _mc = null;
        bool _isMemcachedUsed = false;
        public AjaxCommon()
        {
            _isMemcachedUsed = bool.Parse(ConfigurationManager.AppSettings.Get("IsMemcachedUsed"));
            if (_mc == null && _isMemcachedUsed)
            {
                _mc = new MemcachedClient("memcached");
            }
        }
        /// <summary>
        ///  Written By : Ashish G. Kamble on 6 Aug 2013
        ///  Method to get model id and model name to fill the drop down list
        ///  
        /// Modified By : Suresh Prajapati on 06 Dec, 2014
        /// Description : Modified Sequence of method call
        /// </summary>
        /// <param name="requestType">Pass value as New or Used or Upcoming or PQ</param>
        /// <param name="makeId"></param>
        /// <returns>Method will return model id and model name in json format</returns>
        [AjaxPro.AjaxMethod()]
        public string GetModels(string requestType, string makeId)
        {
            string jsonModels = string.Empty;
            DataTable dt = null;

            MakeModelVersion mmv = new MakeModelVersion();

            dt = mmv.GetModels(makeId, requestType);

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonModels = JSON.GetJSONString(dt);
            }

            return jsonModels;
        }

        /// <summary>
        ///     Written By : Ashish G. Kamble on 2 Apr 2013
        ///     Summary : This function returns json string containing the name and id of the models
        /// </summary>
        /// <param name="makeId">Makeid of bike should be supplied. Value should be greater than zero.</param>
        /// <returns>Function returns the json string containing the name and id of the models.</returns>
        [AjaxPro.AjaxMethod()]
        public string GetModels(string makeId)
        {
            throw new Exception("Method not used/commented");

            //string sql = string.Empty, jsonModels = string.Empty;
            //DataTable dt = null;
            //DataSet ds = null;
            //Database db = null;

            //if (String.IsNullOrEmpty(makeId))
            //    return jsonModels;

            //sql = " SELECT ID AS Value, Name AS Text FROM BikeModels WHERE IsDeleted = 0 AND "
            //    + " BikeMakeId =" + makeId  + " ORDER BY Text ";
            //try
            //{
            //    db = new Database();
            //    ds = db.SelectAdaptQry(sql);

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        dt = ds.Tables[0];

            //        jsonModels = JSON.GetJSONString(dt);
            //    }
            //}
            //catch (SqlException err)
            //{
            //    HttpContext.Current.Trace.Warn("AjaxCommon.GetModels Sql Ex : ", err.Message);
            //    ErrorClass objErr = new ErrorClass(err, "AjaxCommon.GetModels");
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn("AjaxCommon.GetModels Ex : ", err.Message);
            //    ErrorClass objErr = new ErrorClass(err, "AjaxCommon.GetModels");
            //    objErr.SendMail();
            //}

            //return jsonModels;
        }   // End of GetModels function

        /// <summary>
        ///  Written By : Ashish G. Kamble on 6 Aug 2013
        ///  Method to get version id and version name to fill the drop down list
        /// </summary>
        /// <param name="requestType">Pass value as New or Used or Upcoming or PQ</param>
        /// <param name="modelId"></param>
        /// <returns>Method will return version id and version name in json format</returns>
        [AjaxPro.AjaxMethod()]
        public string GetVersions(string requestType, string modelId)
        {
            string jsonVersions = string.Empty;
            DataTable dt = null;

            MakeModelVersion mmv = new MakeModelVersion();

            dt = mmv.GetVersions(modelId, requestType);

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonVersions = JSON.GetJSONString(dt);
            }

            return jsonVersions;
        }

        /// <summary>
        /// Written By : Ashwini Todkar
        /// Purpose : Method to update masking name in BikeMake Table and insert old masking Name to OldMaskingLog
        /// </summary>
        /// <param name="maskingName">passed as make masking name for url formation to bikemake table</param>
        /// <param name="updatedBy"> passed which user has updated last time</param>
        /// <param name="makeId">identify which make mask name is changed</param>

        [AjaxPro.AjaxMethod()]
        public bool UpdateMakeMaskingName(string maskingName, string updatedBy, string makeId)
        {
            bool isSuccess = false;
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();
                isSuccess = mmv.UpdateMakeMaskingName(maskingName, updatedBy, makeId);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isSuccess;
        }//End of UpdateMakeMaskingName


        /// <summary>
        /// Written By : Ashwini Todkar on 5 Aug 2014
        /// Purpose : Method to update masking name in BikeSeries Table 
        /// </summary>
        /// <param name="name">passed as series name to bikeseries table</param>   
        /// <param name="maskingName">passed as series masking name for url formation to bikeseries table</param>      
        /// <param name="seriesId">identify which series mask name is changed</param>

        //[AjaxPro.AjaxMethod()]
        //public bool UpdateSeriesMaskingName(string name, string maskingName, string seriesId)
        //{
        //    bool isSuccess = false;
        //    try
        //    {
        //        ManageBikeSeries mbs = new ManageBikeSeries();
        //        //MakeModelVersion mmv = new MakeModelVersion();
        //        isSuccess = mbs.UpdateSeries(name, maskingName, seriesId);
        //    }
        //    catch (Exception err)
        //    {
        //        ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    return isSuccess;
        //}//End of UpdateMakeMaskingName

        /// <summary>
        ///  Written By : Ashwini Todkar on 7 oct 2013
        ///  Method to update masking name in BikeModel Table and insert old masking Name to OldMaskingLog
        /// </summary>
        /// <param name="maskingName">passed as model masking name for url formation to bikemodel table</param>
        /// <param name="updatedBy"> passed which user has updated last time</param>
        /// <param name="modelId">identify which model mask name is changed</param>
        /// <returns>nothing</returns>

        [AjaxPro.AjaxMethod()]
        public bool UpdateModelMaskingName(string maskingName, string updatedBy, string modelId)
        {
            bool isSuccess = false;
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();
                isSuccess = mmv.UpdateModelMaskingName(maskingName, updatedBy, modelId);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isSuccess;
        }   // End of UpdateModelMaskingName

        /// <summary>
        /// Written By : Ashwini todkar 27 dec 2013
        /// Summary    : Method to delete state from database 
        /// </summary>
        /// <param name="stateId"></param>
        [AjaxPro.AjaxMethod()]
        public void DeleteState(string stateId)
        {
            try
            {
                ManageStates objMS = new ManageStates();
                objMS.DeleteState(stateId);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 27 dec 2013
        /// Summary    : Method to delete city from database 
        /// </summary>
        /// <param name="cityId"></param>
        [AjaxPro.AjaxMethod()]
        public void DeleteCity(string cityId)
        {
            try
            {
                ManageCities objMC = new ManageCities();
                objMC.DeleteCity(cityId);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Jan 2014
        /// Summary    : function retrieves city id and name of state 
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetCities(string requestType, int stateId)
        {
            string jsonCities = string.Empty;

            DataSet ds = null;
            DataTable dt = null;

            ManageCities objMC = new ManageCities();

            ds = objMC.GetCities(stateId, requestType);

            dt = ds.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonCities = JSON.GetJSONString(dt);
            }

            return jsonCities;
        }

        /// <summary>
        /// Written By : Sumit Kate
        /// Summary    : function retrieves city id and name of state from CarWale DB
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetCWCities(string requestType, int stateId)
        {
            string jsonCities = string.Empty;

            DataSet ds = null;
            DataTable dt = null;

            ManageCities objMC = new ManageCities();

            ds = objMC.GetCWCities(stateId, requestType);

            dt = ds.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonCities = JSON.GetJSONString(dt);
            }

            return jsonCities;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 25 Feb 2014
        /// Summary    : ajax method to delete bike series 
        /// </summary>
        /// <param name="seriesId"></param>
        //[AjaxPro.AjaxMethod()]
        //public void DeleteSeries(string seriesId)
        //{
        //    try
        //    {
        //        ManageBikeSeries ms = new ManageBikeSeries();
        //        ms.DeleteSeries(seriesId);
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpContext.Current.Trace.Warn("Exception in DeleteSeries", ex.Message);
        //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //}

        /// <summary>
        /// Craeted By : Sadhana Upadhyay on 12th Feb 2014
        /// Description : To delete Compare Bike records
        /// </summary>
        /// <param name="deleteId"></param>
        [AjaxPro.AjaxMethod()]
        public void DeleteCompBikeData(string deleteId)
        {  
            try
            { 
                CompareBike compBike = new CompareBike();
                compBike.DeleteCompareBike(deleteId);

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of DeleteCompBikeData

        /// <summary>
        /// Craeted By : Sadhana Upadhyay on 12th Feb 2014
        /// Description : To update Compare Bike priorities
        /// Modified By : Sadhana Upadhyay on 06 Nov. 2015
        /// Description : Invaliding Cache for compare Bikes Introduce. 
        /// </summary>
        /// <param name="prioritiesList"></param>
        [AjaxPro.AjaxMethod()]
        public void UpdatePriorities(string prioritiesList)
        {
            if (prioritiesList.Length > 0)
                prioritiesList = prioritiesList.Substring(0, prioritiesList.Length - 1);
            try
            {
                CompareBike compBike = new CompareBike();
                compBike.UpdatePriorities(prioritiesList);
                _mc.Remove("BW_CompareBikes");
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of UpdatePriorities

        /// <summary>
        /// Created By : Sadhana Upadhyay on 22 July 2014
        /// Summary : to update priorities for featured bike
        /// </summary>
        /// <param name="prioritiesList"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool SetFeaturedBikePriorities(string prioritiesList)
        {
            bool isSuccess = false;
            if (prioritiesList.Length > 0)
                prioritiesList = prioritiesList.Substring(0, prioritiesList.Length - 1);

            try
            {
                ManageFeaturedBike compBike = new ManageFeaturedBike();
                compBike.SetFeaturedBikePriorities(prioritiesList);
                isSuccess = true;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Craeted By : Sanjay Soni on 6th Oct 2014
        /// Description : To Approve Classified Sell Inquiry Listings
        /// </summary>
        /// <param name="profileId"></param>
        [AjaxPro.AjaxMethod()]
        public bool ApproveListing(int profileId)
        {
            bool isSuccess = false;
            try
            {
                ClassifiedCommon listing = new ClassifiedCommon();
                isSuccess = listing.ApproveListing(profileId);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Craeted By : Sanjay Soni on 6th Oct 2014
        /// Description : To Discard Classified Sell Inquiry Listings
        /// </summary>
        /// <param name="profileId"></param>
        [AjaxPro.AjaxMethod()]
        public bool DiscardListing(int profileId)
        {
            bool isSuccess = false;
            try
            {
                ClassifiedCommon listing = new ClassifiedCommon();
                isSuccess = listing.DiscardListing(profileId);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Craeted By : Sanjay Soni on 6th Oct 2014
        /// Description : To Approve Classified Sell Inquiry Listing's Photos
        /// </summary>
        /// <param name="photoIdList"></param>
        [AjaxPro.AjaxMethod()]
        public bool ApprovePhotos(string photoIdList)
        {
            bool isSuccess = false;

            try
            {
                ClassifiedCommon listing = new ClassifiedCommon();
                listing.ApproveSelectedPhotos(photoIdList);
                isSuccess = true;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Craeted By : Sanjay Soni on 6th Oct 2014
        /// Description : To Discard Classified Sell Inquiry Listing's Photos
        /// </summary>
        /// <param name="photoIdList"></param>
        [AjaxPro.AjaxMethod()]
        public bool DiscardPhotos(string photoIdList)
        {
            bool isSuccess = false;

            try
            {
                ClassifiedCommon listing = new ClassifiedCommon();
                listing.DiscardSelectedPhotos(photoIdList);
                isSuccess = true;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Craeted By : Sanjay Soni on 6th Oct 2014
        /// Description : To Discard Customer Listing's Photos
        /// </summary>
        /// <param name="photoIdList"></param>
        [AjaxPro.AjaxMethod()]
        public bool DiscardCustomers(string CustIdList)
        {
            bool isSuccess = false;
            try
            {
                ClassifiedCommon listing = new ClassifiedCommon();
                listing.DiscardCustomers(CustIdList);
                isSuccess = true;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay ob 28 Oct 2015
        /// Summary : To get City whose model prices are available
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetPriceQuoteCities(string modelId)
        {
            string jsonCities = string.Empty;

            DataSet ds = null;
            DataTable dt = null;
            try
            {
                ManageCities objMC = new ManageCities();

                ds = objMC.GetPriceQuoteCities(Convert.ToUInt32(modelId));

                if (ds != null)
                    dt = ds.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    jsonCities = JSON.GetJSONString(dt);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.AjaxCommon.GetPriceQuoteCities");
                objErr.SendMail();
            }

            return jsonCities;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay ob 28 Oct 2015
        /// Summary : To get area list for cityid
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetAreas(string cityId)
        {
            string jsonCities = string.Empty;

            DataSet ds = null;
            DataTable dt = null;
            try
            {
                ManageArea objMa = new ManageArea();

                ds = objMa.GetArea(Convert.ToUInt32(cityId));

                if (ds != null)
                    dt = ds.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    jsonCities = JSON.GetJSONString(dt);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.AjaxCommon.GetAreas");
                objErr.SendMail();
            }
            return jsonCities;
        }

        /// <summary>
        ///  Written By : Sangram Nandkhile on 25 Mar 2016
        ///  Method to Map campaign againts contract
        /// </summary>
        [AjaxPro.AjaxMethod()]
        public void MapCampaign(string contractId, string campaignId)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_updatebwdealercontractcampaign"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbParamTypeMapper.GetInstance[SqlDbType.Int], contractId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbParamTypeMapper.GetInstance[SqlDbType.Int], campaignId));
                    MySqlDatabase.ExecuteNonQuery(cmd);

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.AjaxCommon.MapCampaign");
                objErr.SendMail();
            }

        }

        /// <summary>
        ///  Created By : Sushil Kumar
        ///  Created On : 18th April 2016
        ///  Description : To get dealer camapigns and contracts mapping based on dealerId
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetDealerCampaigns(string dealerId)
        {
            string jsonDealerCampaigns = string.Empty;
            DataTable dt = null;
            try
            {
                ManageDealerCampaign objMa = new ManageDealerCampaign();

                dt = objMa.GetDealerCampaigns(Convert.ToUInt32(dealerId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    jsonDealerCampaigns = JSON.GetJSONString(dt);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.AjaxCommon.GetDealerCampaigns");
                objErr.SendMail();
            }
            return jsonDealerCampaigns;
        }
    }   // End of class

}   // End of namespace
