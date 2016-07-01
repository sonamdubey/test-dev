using BikeWale.Entities.AutoBiz;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// PQ Quotation Entity
    /// Modified By     : Sumit Kate
    /// Modified Date   : 08 Oct 2015
    /// Description     : Added PQ_BikeVarient List to send the quotation for other available varients
    /// </summary>
    public class PQ_QuotationEntity
    {
        public List<PQ_Price> PriceList { get; set; }

        public List<string> Disclaimer { get; set; }

        public MakeEntityBase objMake { get; set; }
        public ModelEntityBase objModel { get; set; }
        public VersionEntityBase objVersion { get; set; }
        public List<OfferEntity> objOffers { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public IEnumerable<PQ_BikeVarient> Varients { get; set; }
    }
}
