using BikewaleOpr.Entity;
using System.Collections.Generic;

namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Descritpion: Interface created for ad-operations
    /// </summary>
    public interface IAdOperation
    {
        IEnumerable<PromotedBike> GetPromotedBikes();

        //PromotedBike UpdatePromotedBikes();
        bool SavePromotedBike(PromotedBike objPromotedBike);



    }
}
