using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.DealerCampaign;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Models.DealerCampaign;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 10 May 2017
    /// Summary : Controller have all methods related to dealer campaigns
    /// </summary>
    public class DealerCampaignController : Controller
    {        
        private readonly CampaignServingAreasModel objModel = null;        

        /// <summary>
        /// Constuctor to initialize the dependencies
        /// </summary>
        /// <param name="campaignRepo"></param>
        public DealerCampaignController(IDealerCampaignRepository campaignRepo, ICommuteDistance distance)
        {            
            objModel = new CampaignServingAreasModel(campaignRepo, distance);
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
        [Route("dealercampaign/servingareas/dealerid/{dealerId}/")]
        public ActionResult CampaignServingAreas(uint dealerId)
        {
            CampaignServingAreasVM objVM = null;

            if (dealerId > 0)
            {
                objVM = objModel.GetPageData(dealerId);
            }
            else
            {
                Redirect("/pagenotfound.aspx");
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
        [Route("dealercampaign/servingareas/dealerid/{dealerId}/add/"), HttpPost]
        public ActionResult MapCampaignAreas(uint dealerId, ushort campaignServingStatus, ushort servingRadius, string cityIdList)
        {
            objModel.MapCampaignAreas(dealerId, campaignServingStatus, servingRadius, cityIdList);

            return RedirectToAction("CampaignServingAreas");
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
        [Route("dealercampaign/servingareas/dealerid/{dealerId}/add/additional/"), HttpPost]
        public ActionResult MapAdditionalAreas(uint dealerId, string areaIdList)
        {
            objModel.MapAdditionalAreas(dealerId, areaIdList);

            return RedirectToAction("CampaignServingAreas");
        }
        #endregion


        #region RemmoveAdditionallyMappedAreas action method
        /// <summary>
        /// Written By : Ashish G. Kamble on 12 May 2017
        /// Summary : Action to remove multiple areas mapped with the dealers location
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areaIdList">Comma separated area id list. e.g. areaid1, areaid2, areaid3</param>
        /// <returns></returns>
        [Route("dealercampaign/servingareas/dealerid/{dealerId}/remove/additional/"), HttpPost]
        public ActionResult RemmoveAdditionallyMappedAreas(uint dealerId, string areaIdList)
        {
            objModel.RemmoveAdditionallyMappedAreas(dealerId, areaIdList);

            return RedirectToAction("CampaignServingAreas");
        }
        #endregion


    }   // class
}   // namespace