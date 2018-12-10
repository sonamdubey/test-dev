using Carwale.BL.Elastic.NewCarSearch;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.Search.Model;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Home;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Carwale.Service.Controllers
{
    public class NewCarsSearchController : ApiController
    {
        protected INewCarSearchAppAdapter adapter;
        protected readonly IUnityContainer _container;
        public NewCarsSearchController(INewCarSearchAppAdapter adapter,IUnityContainer container)
        {
            this.adapter = adapter;
            _container = container;
        }

        [HttpGet]
        [Route("api/newcars/search/models/")]
        public IHttpActionResult Models()
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            var pageNo = queryString["pageNo"];
            var pageSize = queryString["pageSize"];
            if (string.IsNullOrWhiteSpace(pageNo) || string.IsNullOrWhiteSpace(pageSize))
                return BadRequest("page");

            try
            {
                string domainName = string.Format("{0}{1}{2}",this.Request.RequestUri.Scheme, Uri.SchemeDelimiter , this.Request.RequestUri.Host);
                var DTO = this.adapter.GetModels(queryString);
                if (DTO.TotalModels > Convert.ToInt32(pageSize) * Convert.ToInt32(pageNo))
                {
                    queryString["pageNo"] = Convert.ToString(Convert.ToInt32(pageNo) + 1);
                    DTO.NextPageUrl = string.Format("{0}/api/newcars/search/models/?{1}" , domainName,HttpUtility.UrlDecode(queryString.ToString()));
                }
                return Ok(DTO);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return InternalServerError();
        }


        [HttpGet]
        [Route("api/v2/newcars/search/models/")]
        public IHttpActionResult Modelsv2()
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            var pageNo = queryString["pageNo"];
            var pageSize = queryString["pageSize"];
            if (string.IsNullOrWhiteSpace(pageNo) || string.IsNullOrWhiteSpace(pageSize))
                return BadRequest("page");

            try
            {
                string domainName = string.Format("{0}{1}{2}", this.Request.RequestUri.Scheme, Uri.SchemeDelimiter, this.Request.RequestUri.Host);
                IServiceAdapterV2 newCarSearchAdaptor = _container.Resolve<IServiceAdapterV2>("NewCarSearchAppAdapter");
                var DTO = newCarSearchAdaptor.Get<NewCarSearchDtoV2,NameValueCollection>(HttpUtility.ParseQueryString(this.Request.RequestUri.Query));
                if (DTO.TotalModels > Convert.ToInt32(pageSize) * Convert.ToInt32(pageNo))
                {
                    queryString["pageNo"] = Convert.ToString(Convert.ToInt32(pageNo) + 1);
                    DTO.NextPageUrl = string.Format("{0}/api/v2/newcars/search/models/?{1}", domainName, HttpUtility.UrlDecode(queryString.ToString()));
                }
                return Ok(DTO);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return InternalServerError();
        }



        [HttpGet]
        [Route("api/newcars/search/versions/")]
        public IHttpActionResult Versions()
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

            if (string.IsNullOrWhiteSpace(queryString["modelId"]))
                return BadRequest("modelId");

            try
            {
                return Ok(this.adapter.GetVersions(queryString));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarsSearchController.Versions()");
                objErr.LogException();
            }
            return InternalServerError();
        }
    }
}
