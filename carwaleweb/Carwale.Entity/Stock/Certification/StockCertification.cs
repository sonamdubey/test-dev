using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Carwale.Entity.Stock.Certification
{
    [JsonObject, Serializable, Validator(typeof(StockCertificationValidator))]
    public class StockCertification
    {
        public int InquiryId { get; set; }

        public bool IsDealer { get; set; }

        public bool? IsActive { get; set; }

        public decimal? OverallScore { get; set; }

        public int? OverallScoreColorId { get; set; }

        [JsonIgnore]
        public string OverallScoreColor { get; set; }

        public string OverallCondition { get; set; }

        public string CarExteriorImageUrl { get; set; }

        [JsonIgnore]
        public string ExteriorOriginalImgPath { get; set; }

        public string ReportUrl { get; set; }

        public string DetailsPageUrl { get; set; }

        public List<StockCertificationItem> Description { get; set; }
    }
}
