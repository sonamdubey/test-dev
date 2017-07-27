using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    public class ComparisonController : Controller
    {

        /// <summary>
        /// Created by :- Sushil Kumar on 27th July 2017
        /// summary :- Get list of sponsored comparisons
        /// </summary>
        /// <returns></returns>
        [Route("comparison/sponsored/manage/")]
        public ActionResult ManageSponsoredCamparison()
        {
            return View();
        }

        /// <summary>
        /// Created by :- Sushil Kumar on 27th July 2017
        /// summary :- Add or update sponsord comparisons
        /// </summary>
        /// <returns></returns>
        [Route("comparison/sponsored/add/")]
        public ActionResult AddorUpdateSponsoredComparison(uint? comparisonId)
        {
            if(comparisonId.HasValue && comparisonId.Value > 0)
            {

            }
            return View();
        }

        /// <summary>
        /// Created by :- Sushil Kumar on 27th July 2017
        /// summary :- Add or update sponsord comparisons
        /// </summary>
        /// <returns></returns>
        [Route("comparison/sponsored/add/rules/")]
        public ActionResult AddSponsoredComparisonRules(uint comparisonId)
        {
            if (comparisonId > 0)
            {

            }
            return View();
        }
    }
}