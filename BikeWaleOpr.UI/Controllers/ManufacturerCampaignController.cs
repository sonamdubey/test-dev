using BikewaleOpr.Models;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    public class ManufacturerCampaignController : Controller
    {
        // GET: ManufacturerCampaign
        public ActionResult SearchManufacturerCampaign()
        {
            SearchCampaign objSearch = new SearchCampaign();
            if (objSearch != null)
            {
                objSearch.GetData();
                return View();
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }   
        }
    }
}