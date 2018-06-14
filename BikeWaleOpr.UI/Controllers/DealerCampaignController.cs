using BikewaleOpr.Interface;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerCampaign;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 10 May 2017
    /// Summary : Controller have all methods related to dealer campaigns
    /// </summary>
    [Authorize]
    public class DealerCampaignController : Controller
    {
        private readonly CampaignServingAreasModel objModel = null;

        /// <summary>
        /// Constuctor to initialize the dependencies
        /// </summary>
        /// <param name="campaignRepo"></param>
        public DealerCampaignController(IDealerCampaignRepository campaignRepo, ICommuteDistance distance, ILocation location)
        {
            objModel = new CampaignServingAreasModel(campaignRepo, distance, location);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region CampaignServingAreas action method
        /// <summary>
        /// Written By : Ashish G. Kamble on 12 May 2017
        /// Summary : Action method to manage campaign serving areas.
        /// </summary>
        /// <param name="dealerId">mandatory field</param>
        /// <returns></returns>
        [Route("dealercampaign/servingareas/dealerid/{dealerId}/campaignid/{campaignid}/")]
        public ActionResult CampaignServingAreas(uint dealerId, uint campaignid)
        {
            CampaignServingAreasVM objVM = null;

            if (dealerId > 0 && campaignid > 0)
            {
                objVM = objModel.GetPageData(dealerId, campaignid);
            }
            else
            {
                return HttpNotFound();
            }

            return View(objVM);
        }   // end of CampaignServingAreas 
        #endregion


        #region MapCampaignAreas action method
        /// <summary>
        /// Written By : Ashish G. Kamble on 12 May 2017
        /// Summary : Action to map areas with dealer location.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="campaignServingStatus">Status of the serving areas to the particular campaign.</param>
        /// <param name="servingRadius">Serving radius for the given dealer (campaign serving radius).</param>
        /// <param name="cityIdList">Comma separated city id list. e.g. cityid1, cityid2, cityid3</param>
        /// <returns></returns>
        [Route("dealercampaign/servingareas/dealerid/{dealerId}/campaignid/{campaignid}/add/"), HttpPost]
        public ActionResult MapCampaignAreas(uint dealerId, uint campaignid, ushort campaignServingStatus, ushort servingRadius, string cityIdList, string[] stateIdList)
        {
            if (dealerId > 0 && campaignid > 0 && campaignServingStatus > 0)
            {
                objModel.MapCampaignAreas(dealerId, campaignid, campaignServingStatus, servingRadius, cityIdList, stateIdList);
                TempData["msg"] = "Campaign areas mapped successfully";
            }
            else
            {
                TempData["msg"] = "Please provide valid inputs";
            }

            return RedirectToAction("CampaignServingAreas", routeValues: new { dealerId = dealerId, campaignId = campaignid });
        }
        #endregion


        #region MapAdditionalAreas action method
        /// <summary>
        /// Written By : Ashish G. Kamble on 12 May 2017
        /// Summary : Action to map addtional areas to the dealer's location
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areaIdList">Comma separated area id list. e.g. areaid1, areaid2, areaid3</param>
        /// <returns></returns>
        [Route("dealercampaign/servingareas/dealerid/{dealerId}/campaignid/{campaignid}/add/additional/"), HttpPost]
        public ActionResult MapAdditionalAreas(uint dealerId, uint campaignid, string areaIdList)
        {
            if (dealerId > 0 && campaignid > 0 && !string.IsNullOrEmpty(areaIdList))
            {
                objModel.MapAdditionalAreas(dealerId, areaIdList);
                TempData["msg"] = "Mapped additional areas with dealer location successfully!!!";
            }
            else
            {
                TempData["msg"] = "Please provide valid inputs";
            }

            return RedirectToAction("CampaignServingAreas", routeValues: new { dealerId = dealerId, campaignId = campaignid });
        }
        #endregion


        #region RemmoveAdditionallyMappedAreas action method
        /// <summary>
        /// Written By : Ashish G. Kamble on 12 May 2017
        /// Summary : Action to remove multiple areas mapped with the dealers location
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="removeAreaIdList">Comma separated area id list. e.g. areaid1, areaid2, areaid3</param>
        /// <returns></returns>
        [Route("dealercampaign/servingareas/dealerid/{dealerId}/campaignid/{campaignid}/remove/additional/"), HttpPost]
        public ActionResult RemmoveAdditionallyMappedAreas(uint dealerId, uint campaignid, string removeAreaIdList)
        {
            if (dealerId > 0 && campaignid > 0 && !string.IsNullOrEmpty(removeAreaIdList))
            {
                objModel.RemmoveAdditionallyMappedAreas(dealerId, removeAreaIdList);
                TempData["msg"] = "Removed additional mapped areas with dealer location successfully!!!";
            }
            else
            {
                TempData["msg"] = "Please provide valid inputs";
            }

            return RedirectToAction("CampaignServingAreas", routeValues: new { dealerId = dealerId, campaignId = campaignid });
        }
        #endregion


    }   // class
}   // namespace