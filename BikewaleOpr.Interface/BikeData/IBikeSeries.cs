using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 12 Sep 2017
    /// Summary: Interface for Bike Series BAL
    /// </summary>
    public interface IBikeSeries
    {
        IEnumerable<BikeSeriesEntity> GetSeries();
        void AddSeries(BikeSeriesEntity bikeSeries, uint UpdatedBy);
    }
}
