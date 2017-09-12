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
        uint AddSeries(BikeSeriesEntity bikeSeries, long UpdatedBy);
        bool EditSeries(BikeSeriesEntity bikeSeries, long UpdatedBy);
        bool DeleteSeries(uint bikeSeriesId);
    }
}
