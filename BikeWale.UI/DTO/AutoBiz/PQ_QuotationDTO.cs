using BikeWale.DTO.AutoBiz;
using System.Collections.Generic;

namespace Bikewale.DTO.AutoBiz
{
    /// <summary>
    /// PQ Quotation Entity
    /// Modified By     : Sumit Kate
    /// Modified Date   : 08 Oct 2015
    /// Description     : Added PQ_BikeVarient List to send the quotation for other available varients
    /// </summary>
    public class PQ_QuotationEntityDTO
    {
        public List<PQ_PriceDTO> PriceList { get; set; }

        public List<string> Disclaimer { get; set; }

        public MakeEntityBaseDTO objMake { get; set; }
        public ModelEntityBaseDTO objModel { get; set; }
        public VersionEntityBaseDTO objVersion { get; set; }
        public List<OfferEntityDTO> objOffers { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public IEnumerable<PQ_BikeVarientDTO> Varients { get; set; }
    }
}
