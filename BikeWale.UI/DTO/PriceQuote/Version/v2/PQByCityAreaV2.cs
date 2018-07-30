
using Bikewale.DTO.Campaign;
using Bikewale.DTO.DealerLocator;
using Bikewale.DTO.Model.v3;
using Bikewale.DTO.PriceQuote.v2;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Bikewale.DTO.PriceQuote.Version.v2
{
    /// <summary>
    /// Created By : vivek gupta on 17 june 2016, added DealerPackageType,SecondaryDealerCount,OfferList
    /// </summary>
    public class PQByCityAreaDTOV2
    {
        [JsonProperty("isCityExists")]
        public bool IsCityExists { get; set; }

        [JsonProperty("isAreaExists")]
        public bool IsAreaExists { get; set; }

        [JsonProperty("isAreaSelected")]
        public bool IsAreaSelected { get; set; }

        [JsonProperty("isExShowroomPrice")]
        public bool IsExShowroomPrice { get; set; }

        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }

        [JsonProperty("pqId")]
        public ulong PqId { get; set; }

        [JsonProperty("versionList")]
        public IEnumerable<VersionDetail> VersionList { get; set; }

        [JsonProperty("primaryDealer")]
        public DealerBase PrimaryDealer { get; set; }

        [JsonProperty("primaryDealerOffers")]
        public IEnumerable<DPQOffer> PrimaryDealerOffers { get; set; }

        [JsonProperty("isPremium")]
        public bool IsPremium { get; set; }

        [JsonProperty("secondaryDealerCount")]
        public int SecondaryDealerCount { get; set; }

        // Updated by: Sangram Nandkhile on 13 Oct 2017
        // Added Campaign base DTO
        [JsonProperty("campaign")]
        public CampaignBaseDto Campaign { get; set; }
    }
}
