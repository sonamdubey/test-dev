﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by: Sangram Nandkhile 27-Jul-2017
    /// Summary: Entity for sponsored campaign
    /// </summary>
    public class SponsoredCampaign
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string ImpressionUrl { get; set; }
        public string ImgImpressionUrl { get; set; }
        public SponsoredCampaignStatus Status { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public uint UpdatedBy { get; set; }

        public uint ComparisonId { get; set; }
    }

}
