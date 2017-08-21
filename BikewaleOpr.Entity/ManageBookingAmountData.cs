using BikewaleOpr.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 5th Aug 2017
    /// Summary : Entity to hold Manage booking amount page data
    /// </summary>
    public class ManageBookingAmountData
    {
        public IEnumerable<BookingAmountEntity> BookingAmountList { get; set; }
        public IEnumerable<Entities.BikeData.BikeMakeEntityBase> BikeMakeList { get; set; }
        public string UpdateMessage { get; set; }
    }
}
