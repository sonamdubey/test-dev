using Bikewale.Interfaces.Finance;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Models;
using Bikewale.Models.Finance;
using Bikewale.Models.Shared;
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
        private readonly IFinanceCacheRepository _financeCache;
		private readonly IPWACMSCacheRepository _renderedArticles;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="financeCache"></param>
        public FinanceController(IFinanceCacheRepository financeCache, IPWACMSCacheRepository renderedArticles)
        {
			_renderedArticles = renderedArticles;
            _financeCache = financeCache;
        }


		/// Description : Changed route to bike-loan-emi-calculator
		/// </summary>
		/// <returns></returns>
		[Route("m/bike-loan-emi-calculator/")]
		public ActionResult Index_Mobile_Pwa()
		{
			FinanceIndexPage obj = new FinanceIndexPage(_renderedArticles);
			FinanceIndexPageVM objData = obj.GetPwaData();
			return View("~/Views/Shared/Index_Mobile_Pwa.cshtml", objData);
		}


		#region Capital finance

		/// <summary>
		/// index mobile for capital first
		/// Sangram Nandkhile on 08-Sep-2017
        /// Modified by : Snehal Dange on 25th May 2018
        /// Description: Moved logic to financeModel
		/// </summary>
		/// <returns></returns>
		[Route("m/finance/capitalfirst/")]
		public ActionResult CapitalFirst_Index_Mobile()
		{
            CapitalFirstModel obj = new CapitalFirstModel(_financeCache);
            obj.IsMobile = true;
            string q = Request.Url.Query;

            NameValueCollection queryCollection = HttpUtility.ParseQueryString(q);
            CapitalFirstVM viewModel = obj.GetData(queryCollection);
        return View(viewModel);
        
		}

        /// <summary>
        /// index desktop for capital first
        /// Sangram Nandkhile on 08-Sep-2017
        /// Modified by : Snehal Dange on 25th May 2018
        /// Description: Moved logic to financeModel
        /// </summary>
        /// <returns></returns>
        [Bikewale.Filters.DeviceDetection]
        [Route("finance/capitalfirst/")]
        public ActionResult CapitalFirst_Index()
        {
            CapitalFirstModel obj = new CapitalFirstModel(_financeCache);
            string q = Request.Url.Query;

            NameValueCollection queryCollection = HttpUtility.ParseQueryString(q);
            CapitalFirstVM viewModel = obj.GetData(queryCollection);
            return View(viewModel);
        }

        #endregion
    }
}
