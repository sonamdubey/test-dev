using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.ManufacturerCampaign
{
    /// <summary>
    /// Created by : Sajal Gupta on 30/08/2016
    /// Description : This entity holds data that is returned by fetchcampaigndetails method;
    /// </summary>
    public class ManufacturerCampaignEntity
    {
        public string CampaignDescription;
        public string CampaignMaskingNumber;
        public int IsActive;
        public string TemplateHtml;
        public int PageId;
        public int IsDefault;
        public int TemplateId;
    }
}



