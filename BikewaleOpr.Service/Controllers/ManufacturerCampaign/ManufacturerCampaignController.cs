using Bikewale.Notifications;
using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Http;


using BikewaleOpr.Entity.ContractCampaign;


using System.Web;
using BikewaleOpr.Entities;

namespace BikewaleOpr.Service.Controllers
{
    public class ManufacturerCampaignController : ApiController
    {
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        public ManufacturerCampaignController(IManufacturerCampaign objManufacturerCampaign)
        {
            _objManufacturerCampaign = objManufacturerCampaign;
        }
        [HttpPost]
        public List<ManufactureDealerCampaign> GetManufactureCampaigns(string dealerId)
        {
            List<ManufactureDealerCampaign> _objMfgList = new List<ManufactureDealerCampaign>();
            try
            {
                _objMfgList = _objManufacturerCampaign.SearchManufactureCampaigns(Convert.ToUInt32(dealerId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SearchManufactureCampaign");
                objErr.SendMail();
            }
            return _objMfgList;
        }

        [HttpPost]
        public IHttpActionResult GetstatuschangeCampaigns(string id, string isactive)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objManufacturerCampaign.statuschangeCampaigns(Convert.ToUInt32(id), Convert.ToUInt32(isactive));
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Trace.Warn("UpdateBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, "statuschangeCampaigns");
                objErr.SendMail();
                return InternalServerError();
            }
            return Ok(isSuccess);
        }

    }
}
