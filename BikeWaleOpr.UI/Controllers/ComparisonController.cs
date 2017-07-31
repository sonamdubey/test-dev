using Bikewale.Comparison.Entities;
using Bikewale.Comparison.Interface;
using BikewaleOpr.Models.Comparison;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    public class ComparisonController : Controller
    {
        private readonly ISponsoredComparisonRepository _objSponsoredComparison = null;
        //private readonly ISponsoredCampaignRepository objSponsoredComparison = null;

        /// <summary>
        /// 
        /// </summary>
        public ComparisonController(ISponsoredComparisonRepository objSponsoredComparison)
        {
            _objSponsoredComparison = objSponsoredComparison;
        }

        /// <summary>
        /// Created by :- Sushil Kumar on 27th July 2017
        /// summary :- Get list of sponsored comparisons
        /// </summary>
        /// <returns></returns>
        [Route("comparison/sponsored/manage/")]
        public ActionResult ManageSponsoredCamparison(string comparisonType)
        {
            if(string.IsNullOrEmpty(comparisonType))
            {
                comparisonType = "2"; //default show only active comparisons
            }

            ManageSponsoredComparisonVM objData = new ManageSponsoredComparisonVM();
            objData.Sponsoredcomparisons = _objSponsoredComparison.GetSponsoredComparisons(comparisonType);

            return View(objData);
        }

        /// <summary>
        /// Created by :- Sushil Kumar on 27th July 2017
        /// summary :- Add or update sponsord comparisons
        /// </summary>
        /// <returns></returns>
        [HttpPost,Route("comparison/sponsored/add/")]
        public ActionResult AddorUpdateSponsoredComparison([System.Web.Http.FromBody] SponsoredComparison objData)
        {
            uint comparisonId = 0;
            try
            {
                if (objData != null)
                {
                    comparisonId = _objSponsoredComparison.SaveSponsoredComparison(objData);
                }
            }
            catch (Exception ex)
            {

            }

            if (comparisonId > 0)
            {
                return Redirect(string.Format("/comparison/sponsored/add/rules/{0}/", objData.Id));
            }
            else
            {
                return Redirect("/comparison/sponsored/manage/");
            }
        }

        /// <summary>
        /// Created by :- Sushil Kumar on 27th July 2017
        /// summary :- Add or update sponsord comparisons
        /// </summary>
        /// <returns></returns>
        [Route("comparison/sponsored/add/rules/{comparisonId}/")]
        public ActionResult AddSponsoredComparisonRules(uint comparisonId)
        {
            if (comparisonId > 0)
            {

            }
            return View();
        }

        
    }
}