using Bikewale.Service.Areas.HelpPage.ModelDescriptions;
using Bikewale.Service.Areas.HelpPage.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace Bikewale.Service.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();

            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(url);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult SearchApiDoc()
        {
            return View(new SearchDoc());
        }

        [System.Web.Http.HttpPost]
        public ActionResult ControllerHelp(string url)
        {
            Collection<ApiDescription> model = null;
            IGrouping<HttpControllerDescriptor, ApiDescription> groupApi = null;
            Collection<ApiDescription> DocumentationProvider = null;
            if (!String.IsNullOrEmpty(url))
            {
                DocumentationProvider = Configuration.Services.GetApiExplorer().ApiDescriptions;
                ILookup<HttpControllerDescriptor, ApiDescription> apiGroups = DocumentationProvider.ToLookup(api => api.ActionDescriptor.ControllerDescriptor);
                string controller = url.Split('/')[1];
                IGrouping<HttpControllerDescriptor, ApiDescription> group = apiGroups.FirstOrDefault(m => m.Key.ControllerName.Equals(controller));
                if (group != null)
                {
                    return View(group);
                }
            }
            return View(ErrorViewName);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Search(SearchDoc search)
        {
            string apiMethodURL = String.Empty;
            if (search != null && !String.IsNullOrEmpty(search.method) && !String.IsNullOrEmpty(search.Url))
            {
                apiMethodURL = String.Format("{0}-{1}", search.method, search.Url);
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiMethodURL);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }
            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }

    public class SearchDoc
    {
        [Display(Name = "HTTP PopulateWhere")]
        public string method { get; set; }
        [Display(Name = "Web API Url")]
        public string Url { get; set; }
    }
}