﻿namespace BikewaleOpr.Entities
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
        public string EntryDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
    }
}