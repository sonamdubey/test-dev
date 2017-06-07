
using BikewaleOpr.Entities.PopularComparisions;
using System.Collections.Generic;
using System;
namespace BikewaleOpr.Interface.PopularComparisions
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th Oct 2016 
    /// Description : Interface for bike comparision list 
    /// Modified by sajal gupta on 02-06-2017
    /// Desc : Modified function SaveBikeComparision sugnature
    /// </summary>
    public interface IPopularBikeComparisions
    {
        IEnumerable<PopularBikeComparision> GetBikeComparisions();
        bool SaveBikeComparision(ushort compareId, uint versionId1, uint versionId2, bool isActive, bool isSponsored, DateTime sponsoredStartDate, DateTime sponsoredEndDate);
        bool UpdatePriorities(string prioritiesList);
        bool DeleteCompareBike(uint deleteId);
    }
}
