using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Interfaces;
using System.Web.Http;
using System.Collections.Specialized;
using System.Net.Http;
using Microsoft.Practices.Unity;
using System.Web;
using Carwale.Notifications;
using Carwale.DAL;
using Carwale.BL;
using Carwale.Entity.DeepLinking;
using Newtonsoft.Json;

namespace Carwale.Service.Controllers
{
    public class DeepLinkingController : ApiController
    {
        //[AthunticateBasic]
        public HttpResponseMessage GetLinkToApp()
        {
            var response = new HttpResponseMessage();

            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                string siteUrl = nvc["siteUrl"].ToString();

                IUnityContainer container = UnityBootstrapper.Resolver.GetContainer();

                IDeepLinking deepLinking = container.Resolve<IDeepLinking>();
                DeepLinkingEntity deepLinkingList = deepLinking.GetLinkToApp(siteUrl);
                response.Content = new StringContent(JsonConvert.SerializeObject(deepLinkingList));

            }
            catch(Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "DeepLinkingController.GetLinkToApp()");
                objErr.LogException();
            }
            return response;
        }

        [HttpGet]
        [Route("api/v2/deeplinking/")]
        public IHttpActionResult V2()
        {

            try
            {               
                IUnityContainer container = UnityBootstrapper.Resolver.GetContainer();
                IDeepLinking deepLinking = container.Resolve<IDeepLinking>();

                return Ok(deepLinking.GetLinkToAppV2(Request.RequestUri.Query));
                
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "DeepLinkingController.GetLinkToApp()");
                objErr.LogException();
            }
            return InternalServerError();
        }


    }
}
