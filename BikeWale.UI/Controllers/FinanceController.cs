using Bikewale.Entities.Dealer;
using Bikewale.Entities.Location;
using Bikewale.Models.Finance;
using Bikewale.Utility;
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

		//[Router("m/finance")]
		//public ActionResult Index_Mobile_Pwa() {

		//}


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
            GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
            if (location != null)
                viewModel.ObjLead.CityId = location.CityId;
            viewModel.objLeadJson = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel.ObjLead);
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
            GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
            if (location != null)
                viewModel.ObjLead.CityId = location.CityId;
            viewModel.objLeadJson = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel.ObjLead);
            return View(viewModel);
        }

        #endregion
    }
}
