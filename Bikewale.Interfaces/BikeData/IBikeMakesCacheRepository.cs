using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 03 Mar 2016
    /// Description :   Bike Makes Cache Repository
    /// Modified By : Sangram Nandkhile on 20 Jun 2016
    /// Summary :   Added new function GetDiscontinuedBikeModelsByMake
    /// Modified by :   Sumit Kate on 16 Sep 2016
    /// Description :   Added new function GetMakeMaskingResponse
    /// Modified by :   Sangram Nandkhile on 10 Mar 2017
    /// Description :   Added new function GetScooterMakes
    /// Modified by :   Aditi Srivastava on 15 Mar 2017
    /// Summary     :   Added new function GetScooterMakeDescription
    /// Modified by : Snehal Dange on 13th Dec 2017
    /// Summary     : Method to get list of  makes in which dealer showroom is present for city
    /// Modified by : Snehal Dange on 14th Dec 2017
    /// Summary     : Method to get list of  makes in which service center is present for city
    /// </summary>
    public interface IBikeMakesCacheRepository
    {
        IEnumerable<BikeMakeEntityBase> GetMakesByType(EnumBikeType makeType);
        IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId);
        BikeDescriptionEntity GetMakeDescription(uint makeId);
        BikeMakeEntityBase GetMakeDetails(uint makeId);
        IEnumerable<BikeMakeModelBase> GetAllMakeModels();
        MakeMaskingResponse GetMakeMaskingResponse(string maskingName);
        IEnumerable<BikeMakeEntityBase> GetScooterMakes();
        BikeDescriptionEntity GetScooterMakeDescription(uint makeId);
        MakeSubFooterEntity GetMakeFooterCategoriesandPrice(uint makeId);
        IEnumerable<BikeMakeEntityBase> GetDealerBrandsInCity(uint cityid);
        IEnumerable<BikeMakeEntityBase> GetServiceCenterBrandsInCity(uint cityId);
    }
}
