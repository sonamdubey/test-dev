
using BikewaleOpr.Entities.PopularComparisions;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.PopularComparisions
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th Oct 2016 
    /// Description : Interface for bike comparision list 
    /// </summary>
    public interface IPopularBikeComparisions
    {
        IEnumerable<PopularBikeComparision> GetBikeComparisions();
        bool SaveBikeComparision(ushort compareId, uint versionId1, uint versionId2, bool isActive);
        bool UpdatePriorities(string prioritiesList);
        bool DeleteCompareBike(uint deleteId);
    }
}
