using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using Bikewale.UI.Entities.Insurance;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Insurance
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Date : 23 Nov 2015
    /// Description : To call Policyboss Model API.
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class InsuranceModelsController : CompressionApiController//ApiController
    {
        string _applicationid = BWConfiguration.Instance.ApplicationId;
        IEnumerable<ModelDetail> modelDetail = null;
        IDictionary<string, string> _headerParameters;

        /// <summary>
        /// Description : Send Client Models detail on specified Make ID.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ModelDetail>))]
        public IHttpActionResult Get(int makeId)
        {
            _headerParameters = new Dictionary<string, string>();
            _headerParameters.Add("clientid", "5");
            _headerParameters.Add("platformid", "2");
            try
            {
                string apiUrl = String.Format("/api/insurance/models/{0}", makeId);

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //modelDetail = objClient.GetApiResponseSync<IEnumerable<ModelDetail>>(BWConfiguration.Instance.CwApiHostUrl, BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, modelDetail, _headerParameters);
                    modelDetail = objClient.GetApiResponseSync<IEnumerable<ModelDetail>>(APIHost.CW, BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, modelDetail, _headerParameters);
                }

                return Ok(modelDetail);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Insurance.Get");
               
                return InternalServerError();
            }
        }
    }
}
