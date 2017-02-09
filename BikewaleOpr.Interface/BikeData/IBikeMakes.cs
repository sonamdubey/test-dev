
using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th Oct 2016 
    /// Description : Interface for bike makes 
    /// </summary>
    public interface IBikeMakes
    {
        IEnumerable<BikeMakeEntityBase> GetMakes(string RequestType);
        IEnumerable<BikeMakeEntity> GetMakesList();
        void AddMake(BikeMakeEntity make, ref short isMakeExist, ref int makeId);
        void UpdateMake(BikeMakeEntity make);
        void DeleteMake(int makeId, int updatedBy);
        string Getsynopsis(int makeId);
        void UpdateSynopsis(int makeId, string synopsis, int updatedBy);
    }
}
