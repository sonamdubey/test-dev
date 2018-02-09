using Bikewale.Entities.BikeData;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for bike makes data.
    /// Modified By :   Sumit Kate on 16 Nov 2015
    /// Summary :   Added new function UpcomingBikeMakes
    /// Modified By : Sangram Nandkhile on 20 Jun 2016
    /// Summary :   Added new function GetDiscontinuedBikeModelsByMake
    /// Modified by :   Sumit Kate on 16 Sep 2016
    /// Description :   Added new function GetOldMaskingNames
    /// Modified by :   Sangram Nandkhile on 10 Mar 2017
    /// Description :   Added new function GetScooterMakes
    /// Modified BY : Snehal Dange on 22nd Nov 2017
    /// Description :  Added GetMakeFooterCategoriesandPrice
    /// Modified by: Snehal Dange on 13th Dec 2017
    /// Descrtiption: Method to get lsit of brands where showroom is present for a particular city
    /// Modified By: Snehal Dange on 14th Dec 2017
    /// Descritpion: Method to get list of  makes in which service center is present for city
    /// Modified by : Snehal Dange on 16th Jan 2017
    /// Description: Method to get ResearchMoreAboutMake widget data (with and without city)
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IBikeMakes<T, U> : IRepository<T, U>
    {
        List<BikeMakeEntityBase> GetMakesByType(EnumBikeType requestType);
        BikeDescriptionEntity GetMakeDescription(U makeId);
        BikeMakeEntityBase GetMakeDetails(uint makeId);
        IEnumerable<BikeMakeEntityBase> UpcomingBikeMakes();
        IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId);
        IEnumerable<BikeMakeModelBase> GetAllMakeModels();
        Hashtable GetOldMaskingNames();
        BikeDescriptionEntity GetScooterMakeDescription(uint makeId);
        IEnumerable<BikeMakeEntityBase> GetScooterMakes();
        MakeSubFooterEntity GetMakeFooterCategoriesandPrice(uint makeId);
        IEnumerable<BikeMakeEntityBase> GetDealerBrandsInCity(uint cityId);
        IEnumerable<BikeMakeEntityBase> GetServiceCenterBrandsInCity(uint cityId);
        ResearchMoreAboutMake ResearchMoreAboutMake(uint makeId);
        ResearchMoreAboutMake ResearchMoreAboutMakeByCity(uint makeId, uint cityId);
    }
}