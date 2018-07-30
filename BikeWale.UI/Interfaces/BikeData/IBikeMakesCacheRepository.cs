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
    /// Modified by : Snehal Dange on 17th Jan 2018
    /// Summary     : Created method ResearchMoreAboutMake to get 'research more about make' widget data (without city) 
    /// Modified by : Snehal Dange on 17th Jan 2018
    /// Summary     : Created Method ResearchMoreAboutMakeByCity to get 'research more about make' widget data (with city) 
    /// Modified By : Deepak Israni on 9th Feb 2018
    /// Summary     : Created method GetExpertReviewCountByMake to get expert review count for make and number of models with expert reviews
    /// Modified By : Rajan Chauhan on 5th Apr 2018
    /// Summary     : Change in GetMakeDescription makeId param from uint to int
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
        IEnumerable<BikeMakeEntityBase> GetDealerBrandsInCity(uint cityId);
        IEnumerable<BikeMakeEntityBase> GetServiceCenterBrandsInCity(uint cityId);
        ResearchMoreAboutMake ResearchMoreAboutMake(uint makeId);
        ResearchMoreAboutMake ResearchMoreAboutMakeByCity(uint makeId, uint cityId);
        ExpertReviewCountEntity GetExpertReviewCountByMake(uint makeId);
    }
}
