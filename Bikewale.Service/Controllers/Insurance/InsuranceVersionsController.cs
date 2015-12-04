using Bikewale.Notifications;
using Bikewale.UI.Entities.Insurance;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Insurance
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Date : 23 Nov 2015
    /// Description : To call Policyboss Model API.
    /// </summary>
    public class InsuranceVersionsController : ApiController
    {
        string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        string _requestType = "application/json";
        IEnumerable<VersionDetail> versionDetail = null;
        IDictionary<string, string> _headerParameters;

        /// <summary>
        /// Description : Send Client Versions detail on specified model ID.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<VersionDetail>))]
        public IHttpActionResult Get(int modelId)
        {
            _headerParameters = new Dictionary<string, string>();    
            _headerParameters.Add("clientid", "5");
            _headerParameters.Add("platformid", "2");
            try
            {
                string apiUrl = "/api/insurance/versions/" + modelId;
                versionDetail = BWHttpClient.GetApiResponseSync<IEnumerable<VersionDetail>>(_cwHostUrl, _requestType, apiUrl, versionDetail, _headerParameters);
                return Ok(versionDetail);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.Insurance.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
