using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.BAL;
using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.common.ContractCampaignAPI;
using BikewaleOpr.Common;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Entities.ContractCampaign;
using BikewaleOpr.Entity.ElasticSearch;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ContractCampaign;
using BikeWaleOpr.Classified;
using Enyim.Caching;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace BikeWaleOpr.Common
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 2 Apr 2013
    /// Summary : Class is used for writing common ajax functions using ajaxpro.
    /// </summary>
    public class AjaxCommon
    {
        protected static MemcachedClient _mc = null;
        protected bool _isMemcachedUsed = false;

        private readonly IBikeSeries _series = null;

        private readonly IBikeESRepository _bikeESRepository;

        private readonly string _indexName;

        public AjaxCommon()
        {
            _isMemcachedUsed = bool.Parse(ConfigurationManager.AppSettings.Get("IsMemcachedUsed"));
            if (_mc == null && _isMemcachedUsed)
            {
                _mc = new MemcachedClient("memcached");
            }

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeSeriesRepository, BikeSeriesRepository>()
                    .RegisterType<IBikeModelsRepository, BikeModelsRepository>()
                .RegisterType<IBikeSeries, BikewaleOpr.BAL.BikeSeries>()
                .RegisterType<IBikeESRepository, BikeESRepository>()
                .RegisterType<IBikeBodyStylesRepository, BikeBodyStyleRepository>()
            .RegisterType<IBikeBodyStyles, BikeBodyStyles>();
                _series = container.Resolve<IBikeSeries>();
                _indexName = ConfigurationManager.AppSettings["MMIndexName"];
                _bikeESRepository = container.Resolve<IBikeESRepository>();
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
            //    ErrorClass.LogError(err, "AjaxCommon.GetModels");
            //    
            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn("AjaxCommon.GetModels Ex : ", err.Message);
            //    ErrorClass.LogError(err, "AjaxCommon.GetModels");
            //    
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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
        //        ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
        //        
        //    }
        //    return isSuccess;
        //}//End of UpdateMakeMaskingName

        /// <summary>
        ///  Written By : Ashwini Todkar on 7 oct 2013
        ///  Method to update masking name in BikeModel Table and insert old masking Name to OldMaskingLog
        ///  Modified by : Vivek Singh Tomar on 12th Dec 2017
        ///  Description : Update masking name of model when masking name is updated
        /// </summary>
        /// <param name="maskingName">passed as model masking name for url formation to bikemodel table</param>
        /// <param name="updatedBy"> passed which user has updated last time</param>
        /// <param name="modelId">identify which model mask name is changed</param>
        /// <returns>nothing</returns>

        [AjaxPro.AjaxMethod()]
        public Tuple<bool, string> UpdateModelMaskingName(string maskingName, string updatedBy, string modelId, string oldMaskingName, string makeMasking, string makeName, string modelName, uint makeId)
        {
            Tuple<bool, string> response = new Tuple<bool, string>(false, "Something went wrong. Please try again.");
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();
                if (!_series.IsSeriesMaskingNameExists(makeId, maskingName))
                {
                    bool isSuccess = mmv.UpdateModelMaskingName(maskingName, updatedBy, modelId);
                    if (isSuccess)
                    {
                        response = new Tuple<bool, string>(true, "Masking Name Updated Successfully.");
                        BikewaleOpr.Cache.BwMemCache.ClearMaskingMappingCache();

                        IEnumerable<string> emails = Bikewale.Utility.GetEmailList.FetchMailList();
                        string oldUrl = string.Format("{0}/{1}-bikes/{2}/", BWOprConfiguration.Instance.BwHostUrl, makeMasking, oldMaskingName);
                        string newUrl = string.Format("{0}/{1}-bikes/{2}/", BWOprConfiguration.Instance.BwHostUrl, makeMasking, maskingName);
                        foreach (var mailId in emails)
                        {
                            SendEmailOnModelChange.SendModelMaskingNameChangeMail(mailId, makeName, modelName, oldUrl, newUrl);
                        }

                        // function to update model masking name in elastic search

                        UpdateBikeESIndex(makeId, modelId, maskingName);
                    }
                    else
                    {
                        response = new Tuple<bool, string>(false, "Masking Name Should be Unique.");
                    }
                }
                else
                {
                    response = new Tuple<bool, string>(false, "Given Model Masking Name already exists as Series Masking Name. Please change series masking name first then try again.");
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return response;
        }   // End of UpdateModelMaskingName

        /// <summary>
        /// Modified by : Vivek Singh Tomar on 13th Dec 2017
        /// Description : Update the Elastic Search Index for given make and model
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="maskingName"></param>
        private void UpdateBikeESIndex(uint makeId, string modelId, string maskingName)
        {
            try
            {
                string id = string.Format("{0}_{1}", makeId, modelId);
                BikeList bike = _bikeESRepository.GetBikeESIndex(id, _indexName);
                if (bike != null && bike.payload != null)
                {
                    bike.payload.ModelMaskingName = maskingName;
                    _bikeESRepository.UpdateBikeESIndex(id, _indexName, bike);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWaleOpr.Common.AjaxCommon : UpdateESIndex, makeId = {0}, modelId = {1}, maskingName = {2}", makeId, modelId, maskingName));
            }
        }

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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
        //        ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //        
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

                //remove memcache keys for all cities
                for (int i = 0; i < 1500; i++)
                {
                    MemCachedUtil.Remove("BW_PopularSimilarBikes_CityId_v2_" + i);
                    MemCachedUtil.Remove("BW_PopularScootersComparison_CityId_v2_" + i);
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Craeted By : Sanjay Soni on 6th Oct 2014
        /// Description : To Approve Classified Sell Inquiry Listings
        /// Modified By : Aditi Srivastava on 20 Oct 2016
        /// Description : changed input parameters to inquiry id, bike name and profile id
        /// </summary>
        /// <param name="profileId"></param>
        [AjaxPro.AjaxMethod()]
        public bool ApproveListing(int inquiryId, string bikeName, string profileId)
        {
            bool isSuccess = false;
            try
            {
                ClassifiedCommon listing = new ClassifiedCommon();
                isSuccess = listing.ApproveListing(inquiryId, bikeName, profileId);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Craeted By : Sanjay Soni on 6th Oct 2014
        /// Description : To Discard Classified Sell Inquiry Listings
        /// Modified By : Aditi Srivastava on 20 Oct 2016
        /// Description : changed input parameters to inquiry id, bike name and profile id
        /// </summary>
        /// <param name="profileId"></param>
        [AjaxPro.AjaxMethod()]
        public bool DiscardListing(int inquiryId, string bikeName, string profileId)
        {
            bool isSuccess = false;
            try
            {
                ClassifiedCommon listing = new ClassifiedCommon();
                isSuccess = listing.DiscardListing(inquiryId, bikeName, profileId);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(ex, "BikewaleOpr.AjaxCommon.GetPriceQuoteCities");

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
                ErrorClass.LogError(ex, "BikewaleOpr.AjaxCommon.GetAreas");

            }
            return jsonCities;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 July 2016
        /// Description :   Release Number
        /// </summary>
        /// <param name="maskingNumber"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool ReleaseNumber(uint dealerId, int campaignId, string maskingNumber)
        {
            bool isSuccess = false;
            try
            {
                if (campaignId > 0 && !String.IsNullOrEmpty(maskingNumber))
                {
                    ManageDealerCampaign objMa = new ManageDealerCampaign();
                    if (objMa.ReleaseCampaignMaskingNumber(campaignId))
                    {
                        CwWebserviceAPI callApp = new CwWebserviceAPI();
                        callApp.ReleaseMaskingNumber(dealerId, Convert.ToInt32(CurrentUser.Id), maskingNumber);
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                ErrorClass.LogError(ex, "BikewaleOpr.AjaxCommon.MapCampaign");

            }
            return isSuccess;
        }

        /// <summary>
        /// Get dealer masking numbers from free pool
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public IEnumerable<MaskingNumber> GetDealerMaskingNumbers(uint dealerId)
        {
            try
            {
                IEnumerable<MaskingNumber> numbersList = null;
                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IContractCampaign, ContractCampaign>();
                    IContractCampaign objCC = container.Resolve<IContractCampaign>();

                    numbersList = objCC.GetAllMaskingNumbers(Convert.ToUInt32(dealerId));

                    if (numbersList != null && numbersList.Any())
                    {
                        return numbersList;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.AjaxCommon.GetDealerMaskingNumbers");

            }
            return null;
        }




        /// <summary>
        ///  Written By : Sangram Nandkhile on 25 Mar 2016
        ///  Method to Map campaign againts contract
        ///  Modified by    :   Sumit Kate on 12 July 2016
        ///  Description    :   Call the ManageDealerCampaign class method to map the dealer Campaigns
        /// </summary>
        [AjaxPro.AjaxMethod()]
        public bool MapCampaign(int contractId, int dealerId, int campaignId, int userId, string oldMaskingNumber, string maskingNumber, string dealerMobile)
        {
            bool isSuccess = false;
            ContractCampaignInputEntity ccInputs = new ContractCampaignInputEntity();
            try
            {

                ManageDealerCampaign objMa = new ManageDealerCampaign();
                ccInputs.ConsumerId = dealerId;
                ccInputs.DealerType = 2;
                ccInputs.LeadCampaignId = campaignId;
                ccInputs.LastUpdatedBy = userId;
                ccInputs.OldMaskingNumber = oldMaskingNumber;
                ccInputs.MaskingNumber = maskingNumber;
                ccInputs.NCDBranchId = -1;
                ccInputs.ProductTypeId = 3;
                ccInputs.Mobile = dealerMobile;
                ccInputs.SellerMobileMaskingId = -1;
                uint _dealerId = Convert.ToUInt32(ccInputs.ConsumerId);
                isSuccess = objMa.MapContractCampaign(contractId, ccInputs.LeadCampaignId);

                CwWebserviceAPI callApp = new CwWebserviceAPI();
                callApp.ReleaseMaskingNumber(_dealerId, ccInputs.LastUpdatedBy, ccInputs.OldMaskingNumber);
                callApp.AddCampaignContractData(ccInputs);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                ErrorClass.LogError(ex, "BikewaleOpr.AjaxCommon.MapCampaign");

            }
            return isSuccess;
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
                ErrorClass.LogError(ex, "BikewaleOpr.AjaxCommon.GetDealerCampaigns");

            }
            return jsonDealerCampaigns;
        }
    }   // End of class
}   // End of namespace
