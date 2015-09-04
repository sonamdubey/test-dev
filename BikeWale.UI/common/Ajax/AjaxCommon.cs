using System;
using System.Web;
using AjaxPro;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.Feedback;
using Bikewale.BAL.Feedback;
using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using Bikewale.Common;
using System.Configuration;
using Bikewale.BAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using System.Collections.Generic;


namespace Bikewale.Ajax
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 8/2/2012
    /// Class written for common ajax functions to be used
    /// </summary>
    public class AjaxCommon
    {
        /// <summary>
        ///  Written By : Ashish G. Kamble on 8/2/2012
        ///  PopulateWhere to get model id and model name to fill the drop down list
        /// </summary>
        /// <param name="requestType">Pass value as New or Used or Upcoming or PQ</param>
        /// <param name="makeId"></param>
        /// <returns>PopulateWhere will return model id and model name in json format</returns>
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
        ///  Written By : Ashish G. Kamble on 8/2/2012
        ///  PopulateWhere to get version id and version name to fill the drop down list
        /// </summary>
        /// <param name="requestType">Pass value as New or Used or Upcoming or PQ</param>
        /// <param name="modelId"></param>
        /// <returns>PopulateWhere will return version id and version name in json format</returns>
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
        ///     Function will return cities list in the json string format
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="requestType">It should be ALL</param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetCities(string requestType, string stateId)
        {
            string jsonCities = string.Empty;
            DataTable dt = null;

            StateCity obj = new StateCity();
            dt = obj.GetCities(stateId, requestType);

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonCities = JSON.GetJSONString(dt);
            }

            return jsonCities;
        }   // end of GetCities method

        /// <summary>
        ///     Function will find new bike dealers in the city on the basis of city id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Function returns bike id and name in form of json string</returns>
        [AjaxPro.AjaxMethod()]
        public string GetNewBikeDealersInCity(string cityId)
        {
            string jsonBikes = string.Empty;
            DataTable dt = null;

            LocateDealers ld = new LocateDealers();

            dt = ld.GetNewBikeDealerInCity(cityId);

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonBikes = JSON.GetJSONString(dt);
            }

            return jsonBikes;
        }

        /// <summary>
        ///    function will return Make id and name for locat dealer passing parameter city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Function returns bike id and name in form of json string</returns>
        [AjaxPro.AjaxMethod()]
        public string GetMakes(string cityId)
        {
            string jsonBikes = string.Empty;
            DataTable dt = null;

            LocateDealers ld = new LocateDealers();

            dt = ld.GetNewBikeMakes(cityId,"new");

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonBikes = JSON.GetJSONString(dt);
            }

            return jsonBikes;
        }

        [AjaxPro.AjaxMethod()]
        public string GetCurrentUserName()
        {
            return CurrentUser.Name;
        }

        [AjaxPro.AjaxMethod()]
        public string GetCurrentUserId()
        {
            return CurrentUser.Id;
        }

        /// <summary>
        ///     Written By : Ashish G. Kamble on 5 Nov 2012
        ///     Summary : Function will send a get passowrd link to customer.
        /// </summary>
        /// <param name="email">Email on which get password link to be send.</param>
        /// <returns>Return true if password link is send else false.</returns>
        [AjaxPro.AjaxMethod()]
        public bool SendCustomerPwd(string email)
        {
            bool retVal = false;
            
            RegisterCustomer objCust = new RegisterCustomer();
            retVal = objCust.SendCustomerPassword(email.Trim());

            return retVal;
        }

        /// <summary>
        ///  Written By : Ashwini Todkar on 11/Oct/2013
        ///  PopulateWhere to get model id and model Mapping name to fill the drop down list
        /// </summary>
        /// <param name="requestType">Pass value as New or Used or Upcoming or PQ</param>
        /// <param name="makeId"></param>
        /// <returns>PopulateWhere will return model id and model mapping name in json format</returns>
        [AjaxPro.AjaxMethod()]
        public string GetModelsWithMappingName(string requestType, string makeId)
        {
            string jsonModels = string.Empty;
            DataTable dt = null;

            MakeModelVersion mmv = new MakeModelVersion();

            dt = mmv.GetModelsWithMappingName(makeId, requestType);

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonModels = JSON.GetJSONString(dt);
            }

            return jsonModels;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 June 2014
        /// Summary : To get dealer Cities list by make id
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetDealersCitiesListByMakeId(uint makeId)
        {
            string JsonCity = string.Empty;
            DataTable dt = null;

            LocateDealers ld = new LocateDealers();

            dt = ld.GetDealersCitiesListByMakeId(makeId);

            if (dt != null && dt.Rows.Count > 0)
            {
                JsonCity = JSON.GetJSONString(dt);
            }

            return JsonCity;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Oct 2014
        /// Summary    : PopulateWhere to save emi request and register customer tif user is new else update customer details
        /// </summary>
        /// <param name="custName">Customer Name</param>
        /// <param name="email">Customer Email</param>
        /// <param name="mobile">Customer Mobile</param>
        /// <param name="modelId"></param>
        /// <param name="selectedCityId"></param>
        /// <param name="leadtype"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool SaveEMIAssistaneRequest(string custName, string email, string mobile,  string modelId, string selectedCityId, string leadtype)
        {
         
            string _customerId = string.Empty;
            BWCommon objCommon = new BWCommon();

            //RegisterCustomer objRC = new RegisterCustomer();
            //_customerId = objRC.IsRegisterdCustomer(email.ToLower());
            //if (string.IsNullOrEmpty(_customerId))
                //_customerId = objRC.RegisterUser(custName, email.ToLower(), mobile, "", "", "");
            //else
               // objRC.UpdateCustomerMobile(mobile, email.ToLower(), custName);

            return objCommon.SaveEMIAssistaneRequest(custName, email, mobile, modelId, selectedCityId, leadtype);
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 22nd Oct 2014
        /// Summary : function to get areas list by city id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetAreas(string cityId)
        {
            string jsonCities = string.Empty;
            DataTable dt = null;

            StateCity obj = new StateCity();
            dt = obj.GetAreas(cityId);

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonCities = JSON.GetJSONString(dt);
            }

            return jsonCities;
        }   // end of GetCities method

        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 Jan 2015
        /// Summary : To save customer Feedback
        /// Modified By : Sadhana Upadhyay on 26 Aug 2015
        /// Summary : To send Email to multiple People
        /// </summary>
        /// <param name="feedbackType"></param>
        /// <param name="feedbackComment"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool SaveCustomerFeedback(ushort feedbackType, string feedbackComment, ushort platformId ,string pageUrl)
        {
            bool isSaved = false;
            string[] feedbackEmailTo = ConfigurationManager.AppSettings["feedbackEmailTo"].Split(';');
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IFeedback, Feedback>();
                    IFeedback objFeedback = container.Resolve<IFeedback>();

                    isSaved = objFeedback.SaveCustomerFeedback(feedbackComment, feedbackType, platformId, pageUrl);
                    HttpContext.Current.Trace.Warn("ismail Send ", isSaved.ToString());
                    if (isSaved)
                    {
                        ComposeEmailBase objEmail = new FeedbackMailer(pageUrl, feedbackComment);
                        objEmail.Send(feedbackEmailTo[0], "BikeWale User Feedback", "", feedbackEmailTo, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Common.ErrorClass objErr = new Bikewale.Common.ErrorClass(ex, "AjaxCommon.SaveCustomerFeedback");
                objErr.SendMail();
            }

            return isSaved;
        }

        /// <summary>
        ///    function will return Make id and name for locat dealer passing parameter city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Function returns bike id and name in form of json string</returns>
        [AjaxPro.AjaxMethod()]
        public string GetBikeMakes(string requestType)
        {
            string jsonBikes = string.Empty;
            DataTable dt = null;

            MakeModelVersion mmv = new MakeModelVersion();
            dt = mmv.GetMakes(requestType);

            if (dt != null && dt.Rows.Count > 0)
            {
                jsonBikes = JSON.GetJSONString(dt);
            }

            return jsonBikes;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 20 june 2015
        /// Summary : Function to get the list of bike models for the given series id
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]      
        public string GetBikeModelsBySeriesId(uint seriesId)
        {
            string models = string.Empty;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeSeries<BikeSeriesEntity, uint>, BikeSeries<BikeSeriesEntity, uint>>();
                    IBikeSeries<BikeSeriesEntity, uint> objSeries = container.Resolve<IBikeSeries<BikeSeriesEntity, uint>>();

                    List<BikeModelEntityBase> objModels = objSeries.GetModelsListBySeriesId(seriesId);

                    if (objModels != null)
                    {
                        models = JavaScriptSerializer.Serialize(objModels);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Common.ErrorClass objErr = new Bikewale.Common.ErrorClass(ex, "AjaxCommon.GetBikeModelsBySeriesId");
                objErr.SendMail();
            }

            return models;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 21 June 2015
        /// Summary : Function to get the models list. ModelId, ModelName, MaskingName
        /// </summary>
        /// <param name="requestType">PriceQuote,New,Used,Upcoming,RoadTest,ComparisonTest,All,UserReviews,NewBikeSpecs,UsedBikeSpecs,NewBikeSpecification</param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetModelsNew(string requestType, string makeId)
        {
            string models = string.Empty;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, uint>, BikeModels<BikeModelEntity, uint>>();
                    IBikeModels<BikeModelEntity, uint> objSeries = container.Resolve<IBikeModels<BikeModelEntity, uint>>();

                    EnumBikeType bikeType = (EnumBikeType)Enum.Parse(typeof(EnumBikeType), requestType, true);

                    List<BikeModelEntityBase> objModels = objSeries.GetModelsByType(bikeType, Convert.ToInt32(makeId));

                    if (objModels != null)
                    {
                        models = JavaScriptSerializer.Serialize(objModels);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Common.ErrorClass objErr = new Bikewale.Common.ErrorClass(ex, "AjaxCommon.GetModelsNew");
                objErr.SendMail();
            }

            return models;
        }
    }   // End of class
}   // End of namespace