using Carwale.BL.Advertizing.Apps;
using Carwale.Entity.Enum;
using Carwale.Notifications;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Carwale.Service.Controllers.Advertisment
{
    public class ESController : ApiController
    {
        [ActionName("leaderboard")]
        public HttpResponseMessage GetESAd(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage();
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                if (!RegExValidations.IsPositiveNumber(nvc["screenId"].ToString()))
                {
                    response.Content = new StringContent("Bad Request");
                    return response;
                }

                int sourceId = Convert.ToInt16(request.Headers.GetValues("SourceId").First());
               
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ESCampaignBL>();
                    ESCampaignBL es = container.Resolve<ESCampaignBL>();
                    response.Content = new StringContent(JsonConvert.SerializeObject(es.GetAd(sourceId)));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ESController.GetESAd()");
                objErr.LogException();
            }
            return response;
        }

    }
}
