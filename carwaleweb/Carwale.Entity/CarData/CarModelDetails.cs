using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarModelDetails : CarModelSummary
    {
        public string MakeName { get; set; }
        public string ModelImageSmall { get; set; }
        public string ModelImageLarge { get; set; }        
        public int Used { get; set; }
        public bool Futuristic { get; set; }
        public int RootId { get; set; }
        public string RootName { get; set; }
        public int VideoCount { get; set; }
        public int CarCount { get; set; }
        public string OriginalImgPath { get; set; }
        public byte SubSegmentId { get; set; }
        public int ModelPopularity { get; set; }
        public int PopularVersion { get; set; }
        public int PhotoCount { get; set; }
        public bool Is360ExteriorAvailable { get; set; }
        public bool Is360OpenAvailable { get; set; }
        public bool IsMetalicColor { get; set; }
        public bool IsSolidColor { get; set; }
        public DateTime? ModelLaunchDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool Imported { get; set; }
        public DateTime? DiscontinuationDate { get; set; }
        public bool Indian { get; set; }
        public bool Is360InteriorAvailable { get; set; }
        public List<ModelColors> ModelColors { get; set; }
        public bool IsModelColorPhotosAvailable { get; set; } 
        public short ReplacedModelId { get; set; }
        
    }

    public class CarModelURI
    {     
        public string CarMakeIds { get; set; }
        public string FuelTypeIds { get; set; }
        public string TransmissionTypeIds { get; set; }
        public string BodyStyleIds { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string SortCriteria { get; set; }
        public string SortOrder { get; set; }
        public string StartIndex { get; set; }
        public string LastIndex { get; set; }
    }
}
