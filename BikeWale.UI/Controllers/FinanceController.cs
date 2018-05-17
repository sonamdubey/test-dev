using Bikewale.Entities.Dealer;
using Bikewale.Entities.Location;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Models;
using Bikewale.Models.Finance;
using Bikewale.Models.Shared;
using Bikewale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Controller for Finance modules
    /// </summary>
    /// <author>
    /// Sangram Nandkhile on 08-Sep-2017
    /// 
    /// </author>
    public class FinanceController : Controller
    {
        private readonly IPWACMSCacheRepository _renderedArticles;

        #region Constructor
        public FinanceController(IPWACMSCacheRepository articles)
        {
            _renderedArticles = articles;
        }
        #endregion

        /// <summary>
        /// Created by  : Rajan Chauhan on 28 Mar 2018
        /// </summary>
        /// <returns></returns>
        [Route("m/finance/")]
        public ActionResult Index_Mobile_Pwa()
        {
            FinanceIndexPage obj = new FinanceIndexPage(_renderedArticles);
            PwaBaseVM objData = obj.GetPwaData();
            return View("~/Views/Shared/Index_Mobile_Pwa.cshtml", objData);
        }


        #region Capital finance

        /// <summary>
        /// index mobile for capital first
        /// Sangram Nandkhile on 08-Sep-2017
        /// </summary>
        /// <returns></returns>
        [Route("m/finance/capitalfirst/")]
        public ActionResult CapitalFirst_Index_Mobile()
        {

            string q = Request.Url.Query;
            bool sendSMStoCustomer = false;
            ushort platformId = 0;            
            CapitalFirstVM viewModel = new CapitalFirstVM();
            viewModel.ObjLead = new ManufacturerLeadEntity();
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(q);
                        
            viewModel.ObjLead.CampaignId = Convert.ToUInt16(queryCollection["campaingid"]);
            viewModel.ObjLead.DealerId = Convert.ToUInt16(queryCollection["dealerid"]);
            viewModel.ObjLead.LeadSourceId = Convert.ToUInt16(queryCollection["leadsourceid"]);
            viewModel.ObjLead.VersionId = Convert.ToUInt16(queryCollection["versionid"]);
            viewModel.ObjLead.PQId = Convert.ToUInt32(queryCollection["pqid"]);
            viewModel.PageUrl = queryCollection["url"];
            viewModel.BikeName = queryCollection["bike"];
            viewModel.LoanAmount = Convert.ToUInt32(queryCollection["loanamount"]);
            viewModel.PlatformId = ushort.TryParse(queryCollection["platformid"], out platformId) ? platformId :(ushort)DTO.PriceQuote.PQSources.Mobile;
            viewModel.ObjLead.BikeName = queryCollection["bike"];
            viewModel.ObjLead.DealerName = queryCollection["dealerName"];
            viewModel.ObjLead.SendLeadSMSCustomer = Boolean.TryParse(queryCollection["sendLeadSMSCustomer"], out sendSMStoCustomer);

            GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
            if (location != null)
                viewModel.ObjLead.CityId = location.CityId;
            viewModel.objLeadJson = JsonConvert.SerializeObject(viewModel.ObjLead);
            return View(viewModel);
        }



        /// <summary>
        /// index desktop for capital first
        /// Sangram Nandkhile on 08-Sep-2017
        /// </summary>
        /// <returns></returns>
        [Bikewale.Filters.DeviceDetection]
        [Route("finance/capitalfirst/")]
        public ActionResult CapitalFirst_Index()
        {

            string q = Request.Url.Query;
            bool sendSMStoCustomer = false;
            CapitalFirstVM viewModel = new CapitalFirstVM();
            viewModel.ObjLead = new ManufacturerLeadEntity();
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(q);
            viewModel.ObjLead.CampaignId = Convert.ToUInt16(queryCollection["campaingid"]);
            viewModel.ObjLead.DealerId = Convert.ToUInt16(queryCollection["dealerid"]);
            viewModel.ObjLead.LeadSourceId = Convert.ToUInt16(queryCollection["leadsourceid"]);
            viewModel.ObjLead.VersionId = Convert.ToUInt16(queryCollection["versionid"]);
            viewModel.ObjLead.PQId = Convert.ToUInt32(queryCollection["pqid"]);
            viewModel.PageUrl = queryCollection["url"];
            viewModel.BikeName = queryCollection["bike"];
            viewModel.PlatformId = (ushort)DTO.PriceQuote.PQSources.Desktop;
            viewModel.LoanAmount = Convert.ToUInt32(queryCollection["loanamount"]);
            viewModel.ObjLead.BikeName = queryCollection["bike"];
            viewModel.ObjLead.DealerName = queryCollection["dealerName"];
            viewModel.ObjLead.SendLeadSMSCustomer = Boolean.TryParse(queryCollection["sendLeadSMSCustomer"], out sendSMStoCustomer);

            GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
            if (location != null)
                viewModel.ObjLead.CityId = location.CityId;
            viewModel.objLeadJson = JsonConvert.SerializeObject(viewModel.ObjLead);
            return View(viewModel);
        }

        #endregion
    }
}
