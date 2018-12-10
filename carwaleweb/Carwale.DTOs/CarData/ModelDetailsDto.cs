using Carwale.DTOs.PriceQuote;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class ModelDetailsDto
    {       
        public int MakeId { get; set; }
       
        public int ModelId { get; set; }
       
        public string MakeName { get; set; }
        
        public string ModelName { get; set; }
        
        public string MaskingName { get; set; }        
       
        public string HostUrl { get; set; }
       
        public string OriginalImgPath { get; set; }

        public string Mileage { get; set; }

        public int NewsCount { get; set; }

        public int ExpertReviewCount { get; set; }

        public int PhotoCount { get; set; }

        public int VideoCount { get; set; }

        public List<ModelColors> Colours { get; set; }

        public PriceOverviewDTOV2 CarPriceOverview { get; set; }
    }
}
