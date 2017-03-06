
using BikewaleOpr.Entity.BikeData;
namespace BikewaleOpr.Interface.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 Mar 2016
    /// Desc: Interface for Used bike data
    /// </summary>
    public interface IUsedBikes
    {
        bool SendUnitSoldEmail(SoldUnitData dataObject, string currentUserName);
        void SendUploadUsedModelImageEmail(string bikeNames);
    }
}
