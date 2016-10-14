
using Bikewale.Entities.Used;
using System;
namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Oct 2016
    /// Summary: Interface for Sell bikes - DAL
    /// </summary>
    public interface ISellBikesRepository<T, U> : IRepository<T, U>
    {
        T GetById(U inquiryId, UInt64 customerId);
        bool UpdateOtherInformation(SellBikeAdOtherInformation otherInfo, U inquiryId, UInt64 customerId);
    }
}
