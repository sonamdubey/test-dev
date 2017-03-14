using Bikewale.Entities.BikeData;
using Bikewale.Entities.SEO;
using Bikewale.Interfaces.BikeData;
using Bikewale.Models.Shared;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BAL.MVC.UI
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 10 Mar 2017
    /// Summary:  BAL helper for scooters brand
    /// </summary>
    public class ScooterBrands
    {
        public BrandWidget GetScooterBrands(IBikeMakesCacheRepository<int> IScooters, ushort topBrandCount)
        {
            BrandWidget objBrand = null;
            try
            {
                IEnumerable<BikeMakeEntityBase> scooterBrand = IScooters.GetScooterMakes();
                objBrand = new BrandWidget()
                {
                    TopBrands = scooterBrand.Take(topBrandCount),
                    OtherBrands = scooterBrand.Skip(topBrandCount)
                };

            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "ScooterBrands.GetScooterBrands()");
            }
            return objBrand;
        }

        public IEnumerable<BikeMakeEntityBase> GetOtherScooterBrands(IBikeMakesCacheRepository<int> IScooters, uint MakeId, ushort topCount)
        {
            IEnumerable<BikeMakeEntityBase> scooterBrand = null;
            try
            {
                scooterBrand = IScooters.GetScooterMakes();
                scooterBrand = scooterBrand.Where(x => x.MakeId != MakeId).Take(topCount);
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, string.Format("ScooterBrands.GetOtherScooterBrands(): MakeId {0}", MakeId));
            }
            return scooterBrand;
        }
    }


    /// <summary>
    /// Created by : Sangram Nandkhile on 14 Mar 2017
    /// Summary:  BAL helper for scooters and make wise scooters
    /// </summary>
    public class ScootersHelper
    {
        public PageMetaTags CreateLandingMetaTags(bool IsDesktop)
        {
            PageMetaTags metas = null;
            try
            {
                metas = new PageMetaTags();
                metas.CanonicalUrl = "https://www.bikewale.com/new-scooters/";
                if (IsDesktop)
                    metas.AlternateUrl = "https://www.bikewale.com/m/new-scooters/";
                metas.Keywords = "Scooters, Scooty, New scooter, New Scooty, Scooter in India, scooty, Scooter comparison, compare scooter, scooter price, scooty price";
                metas.Description = "Find scooters of Honda, Hero, TVS, Vespa and many more brands. Know about prices, images, colours, specs and reviews of scooters in India";
                metas.Title = "New Scooters - Scooters Prices, Reviews, Images, Colours - BikeWale";
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "ScootersHelper.CreateLandingMetaTags()");
            }
            return metas;
        }

        public PageMetaTags CreateMakeWiseMetaTags(bool IsDesktop, string makeMasking, string makeName)
        {
            PageMetaTags metas = null;
            try
            {
                metas = new PageMetaTags();
                metas.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-scooters/", makeMasking);
                if (IsDesktop)
                    metas.AlternateUrl = string.Format("https://www.bikewale.com/m/{0}-scooters/", makeMasking);
                metas.Keywords = string.Format("{0} Scooter, {0} Scooty, Scooter {0}, Scooty Honda, Scooters, Scooty", makeName);
                metas.Description = string.Format("{0} Scooters in India- Find prices, mileage, specifications, versions, news and images of new and upcoming {0} Scooters at BikeWale.",makeName);
                metas.Title = string.Format("{0} Scooters Prices, Mileage, Specs & Images- BikeWale", makeName);
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "ScootersHelper.CreateMakeWiseMetaTags()");
            }
            return metas;
        }

    }
}
