using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AjaxPro;
using BikeWaleOpr.Common;
using BikewaleOpr.Entities;
namespace BikewaleOpr.Common
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Feb 2016
    /// Description :   AjaxPro Class for Manufacturer Campaign
    /// </summary>
    public class AjaxManufacturerCampaign
    {
        private readonly ManageManufacturerCampaign objDAL = null;
        /// <summary>
        /// Constructor
        /// </summary>
        public AjaxManufacturerCampaign()
        {
            objDAL = new ManageManufacturerCampaign();
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 01 Feb 2016
        /// Ajax Pro Method for GetManufacturerCampaigns
        /// </summary>
        /// <param name="dealerId">Dealer Id</param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string GetManufacturerCampaigns(int dealerId)
        {
            string json = String.Empty;
            IEnumerable<ManufacturerCampaignEntity> manufacturerCampaigns = null;
            try
            {
                if (dealerId > 0)
                {
                    manufacturerCampaigns = objDAL.GetManufacturerCampaigns(dealerId);
                    if (manufacturerCampaigns != null && manufacturerCampaigns.Count() > 0)
                    {
                        json = JavaScriptSerializer.Serialize(manufacturerCampaigns);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return json;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Feb 2016
        /// Saves the Manufacturer Campaign
        /// </summary>
        /// <param name="dealerId">Dealer id</param>
        /// <param name="modelIds">model ids (Comma seperated value)</param>
        /// <param name="description">campaign description</param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool SaveManufacturerCampaign(int dealerId, string modelIds, string description)
        {
            bool isSuccess = false;
            try
            {                
                isSuccess = objDAL.SaveManufacturerCampaign(dealerId, modelIds, description);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Feb 2016
        /// Sets the manufacturer Campaigns as inactive
        /// </summary>
        /// <param name="dealerId">Dealer Id</param>
        /// <param name="campaignIds">Campaign Ids(Comma seperated value)</param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool SetManufacturerCampaignInActive(int dealerId, string campaignIds)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = objDAL.SetManufacturerCampaignInActive(dealerId, campaignIds);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isSuccess;
        }
    }
}