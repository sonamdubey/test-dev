using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using System;

namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Created by : Vivek Singh Tomar On 5th Aug 2017
    /// Summary : Interface for manage booking amount of opr
    /// </summary>
    public interface IManageBookingAmountPage
    {
        ManageBookingAmountData GetManageBookingAmountData(UInt32 dealerId);
        bool AddBookingAmount(BookingAmountEntity objBookingAmountEntity, string updatedBy);
    }
}
