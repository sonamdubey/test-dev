using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class ModelMenuDTO
    {
        public short? VariantCount { get; set; }
        public short? ExpertReviewCount { get; set; }
        public short? ColorCount { get; set; }
        public bool? IsMileageAvailable { get; set; }
        public short? PhotoCount { get; set; }
        public ModelMenuEnum ActiveSection { get; set; }
    }

    public class ModelMenuDTO_V1
    {
        public bool IsVariantAvailable { get; set; }
        public bool IsExpertReviewAvailable { get; set; }
        public bool IsColorAvailable { get; set; }
        public bool IsMileageAvailable { get; set; }
        public bool isPhotoAvailable { get; set; }
        public short? PhotoCount { get; set; }
        public bool Is360Available { get; set; }
        public string CarName { get; set; }
        public string ModelName { get; set; }
        public string MakeName { get; set; }
        public string MaskingName { get; set; }
        public ModelMenuEnum ActiveSection { get; set; }
        public string TrackingLabel { get; set; }
        public short? VideoCount { get; set; }
        public bool IsColorsImagesAvailable { get; set; }
        public ThreeSixtyViewCategory Default360Category { get; set; }
        public bool IsPriceInCityPage { get; set; }
    }
}
