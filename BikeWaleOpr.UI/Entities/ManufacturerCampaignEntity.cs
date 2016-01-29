using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jan 2016
    /// Description :   Manufacturer Campaign Entity
    /// </summary>
    public class ManufacturerCampaignEntity
    {
        public uint CampaignId { get; set; }
        public uint DealerId { get; set; }
        public uint ModelId { get; set; }
        public DateTime EntryDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}