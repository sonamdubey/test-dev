
using System.Collections.Generic;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.BikeData;
namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th Oct 2016 
    /// Description : Interface for bike makes repository
    /// Modified by : Sajal Gupta on 10-03-2017
    /// Description : Added Getsynopsis, UpdateSynopsis
    /// Modified by : Sajal Gupta on 20-11-2017
    /// Description : Added GetMakeFooterCategoryData, SaveMakeFooterData, DisableAllMakeFooterCategories
    /// </summary>
    public interface IBikeMakesRepository
    {
        IEnumerable<BikeMakeEntityBase> GetMakes(string requestType);
        IEnumerable<BikeMakeEntityBase> GetMakes(ushort RequestType);
        IEnumerable<BikeMakeEntity> GetMakesList();
        BikeMakeEntity GetMakeDetailsById(uint makeId);
        void AddMake(BikeMakeEntity make, ref short isMakeExist, ref int makeId);
        void UpdateMake(BikeMakeEntity make);
        void DeleteMake(int makeId, int updatedBy);
        SynopsisData Getsynopsis(int makeId);
        void UpdateSynopsis(int makeId, int updatedBy, SynopsisData objSynopsis);
        IEnumerable<BikeModelEntityBase> GetModelsByMake(EnumBikeType requestType, uint makeId);
        IEnumerable<MakeFooterCategory> GetMakeFooterCategoryData(uint makeId);
        void SaveMakeFooterData(uint makeId, uint categoryId, string categorydescription, string userId);
        void DisableAllMakeFooterCategories(uint makeId, string userId);
    }
}
