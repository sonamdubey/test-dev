using Carwale.Entity.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarVersionDetails 
    {
        public string ModelImageSmall { get; set; }
        public string ModelImageLarge { get; set; }
        public string ModelImageXtraLarge { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int New { get; set; }
        public int Used { get; set; }
        public bool Futuristic { get; set; }
        public UInt16 RootId { get; set; }
        public string RootName { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int MinAvgPrice { get; set; }

        public string ModelImage { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
        public float ModelRating { get; set; }
        public int ReviewCount { get; set; }
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        //added by ashish verma on 29/09/2014
        public string SpecSummery { get; set; }
        public decimal ReviewRate { get; set; }
        public string OriginalImgPath { get; set; }
        public string HostURL { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public string ShareUrl { get; set; }
        public byte SegmentId { get; set; }
        public byte SubSegmentId { get; set; }
        public int BodyStyleId { get; set; }
        public PriceOverview PriceOverview { get; set; }
        public EMIInformation EmiInfo { get; set; }
        public int CarTransmission { get; set; }
        public string VersionMasking { get; set; }

        // Added by Meet Shah
        public bool IsDeleted { get; set; }
        public bool IsHybrid { get; set; }
        public bool Indian { get; set; }
        public bool Imported { get; set; }
        public DateTime? DiscontinuationDate { get; set; }
        public DateTime? LaunchDate { get; set; }
        public string UpcomingExpectedLaunch { get; set; }
    }
}
