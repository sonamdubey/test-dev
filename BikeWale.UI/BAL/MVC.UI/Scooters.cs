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
        public BrandWidget GetScooterBrands(IBikeMakesCacheRepository IScooters, ushort topBrandCount)
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
                ErrorClass.LogError(ex, "ScooterBrands.GetScooterBrands()");
            }
            return objBrand;
        }

        public IEnumerable<BikeMakeEntityBase> GetOtherScooterBrands(IBikeMakesCacheRepository IScooters, uint MakeId, ushort topCount)
        {
            IEnumerable<BikeMakeEntityBase> scooterBrand = null;
            try
            {
                scooterBrand = IScooters.GetScooterMakes();
                scooterBrand = scooterBrand.Where(x => x.MakeId != MakeId).Take(topCount);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ScooterBrands.GetOtherScooterBrands(): MakeId {0}", MakeId));
            }
            return scooterBrand;
        }
    }

}
