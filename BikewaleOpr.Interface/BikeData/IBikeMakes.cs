
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th Oct 2016 
    /// Description : Interface for bike makes 
    /// Modified by : Sajal Gupta on 10-03-2017
    /// Description : Added Getsynopsis, UpdateSynopsis
    /// </summary>
    public interface IBikeMakes
    {
        IEnumerable<BikeMakeEntityBase> GetMakes(string requestType);
        IEnumerable<BikeMakeEntityBase> GetMakes(ushort RequestType);
        IEnumerable<BikeMakeEntity> GetMakesList();
        void AddMake(BikeMakeEntity make, ref short isMakeExist, ref int makeId);
        void UpdateMake(BikeMakeEntity make);
        void DeleteMake(int makeId, int updatedBy);
        SynopsisData Getsynopsis(int makeId);
        void UpdateSynopsis(int makeId, int updatedBy, SynopsisData objSynopsis);
    }
}
