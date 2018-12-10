using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Carwale.DTOs.CarData
{

    public abstract class AppDTO { }

    public class SponsoredCarDTO
    {
        [JsonProperty("adSlots")]
        public List<SponsoredAdContentDTO> SponsoredAdContentDTO;
    }

    public class OnlyAppAdsDTO : AppDTO
    {
        [JsonProperty("nativeAds")]
        public List<AdMonetizationDTO> NativeAds;
    }

    public class SponsoredCarHomepageDTO : AppDTO
    {
        [JsonProperty("adSlots")]
        public List<SponsoredAdContentDTO> SponsoredAdContentDTO;

        [JsonProperty("nativeAds")]
        public List<AdMonetizationDTO> NativeAds;
    }
}
