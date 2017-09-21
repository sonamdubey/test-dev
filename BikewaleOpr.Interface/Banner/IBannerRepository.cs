using BikewaleOpr.Entity;
using BikewaleOpr.Models;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.Banner
{
    public interface IBannerRepository
    {
        BannerVM GetBannerDetails(uint bannerId);

        uint SaveBannerBasicDetails(BannerVM objBanner);

        bool SaveBannerProperties(BannerDetails objBanner, uint platformId, uint campaignId);

        IEnumerable<BannerProperty> GetBanners(uint bannerStatus);

        bool ChangeBannerStatus(uint bannerId, UInt16 bannerStatus);
    }
}
