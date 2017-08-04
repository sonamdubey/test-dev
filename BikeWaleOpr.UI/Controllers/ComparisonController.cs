using Bikewale.Comparison.Entities;
using Bikewale.Comparison.Interface;
using Bikewale.Notifications;
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
            ManageSponsoredComparisonVM objData = new ManageSponsoredComparisonVM();
            try
            {
                if (string.IsNullOrEmpty(comparisonType))
                {
                    comparisonType = "2"; //default show only active comparisons
                }
               
                objData.Sponsoredcomparisons = _objSponsoredComparison.GetSponsoredComparisons(comparisonType);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Controllers.ComparisonController.ManageSponsoredCamparison");
            }

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
                    if (comparisonId > 0)
                    {
                        return Redirect(string.Format("/comparison/sponsored/{0}/rules/", comparisonId));
                    }
                }
               
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Controllers.ComparisonController.AddorUpdateSponsoredComparison");
            }

            return Redirect("/comparison/sponsored/manage/");
        }

        /// <summary>
        /// Created by :- Sushil Kumar on 27th July 2017
        /// summary :- Add or update sponsord comparisons
        /// </summary>
        /// <returns></returns>
        [Route("comparison/sponsored/{comparisonId}/rules/")]
        public ActionResult AddSponsoredComparisonRules(uint comparisonId)
        {
            ManageSponsoredComparisonRulesVM objData = new ManageSponsoredComparisonRulesVM();
            try
            {
                if (comparisonId > 0)
                {
                    objData.ComparisonId = comparisonId;
                    objData.ComparisonVersionMapping = _objSponsoredComparison.GetSponsoredComparisonSponsoredBike(comparisonId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Controllers.ComparisonController.AddSponsoredComparisonRules_CampaignId_" + comparisonId);
            }
            return View(objData);
        }

        /// <summary>
        /// Created by :- Sushil Kumar on 27th July 2017
        /// summary :- Add or update sponsord comparisons
        /// </summary>
        /// <returns></returns>
        [Route("comparison/sponsored/save/rules/")]
        public ActionResult SaveSponsoredComparisonRules([System.Web.Http.FromBody]VersionTargetMapping objRules)
        {
            bool isSuccess = false;

            try
            {
                if (objRules != null && objRules.ComparisonId > 0)
                {
                    isSuccess = _objSponsoredComparison.SaveSponsoredComparisonBikeRules(objRules);
                    if (isSuccess)
                    {
                        return Redirect(string.Format("/comparison/sponsored/{0}/rules/", objRules.ComparisonId));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Controllers.ComparisonController.SaveSponsoredComparisonRules");
            }

            return Redirect("/comparison/sponsored/manage/");

        }

    }
}