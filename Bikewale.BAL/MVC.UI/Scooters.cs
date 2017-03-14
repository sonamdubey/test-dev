using Bikewale.Entities.BikeData;
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
}
