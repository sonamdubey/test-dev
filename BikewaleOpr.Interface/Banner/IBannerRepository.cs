using BikewaleOpr.Entity;
using BikewaleOpr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.Banner
{
    public interface IBannerRepository
    {
        BannerVM GetBannerDetails(uint bannerId);

        uint SaveBannerBasicDetails(BannerVM objBanner);

        bool SaveBannerProperties(BannerDetails objBanner, uint paltformId,uint campaignId);
    }
}
