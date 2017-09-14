using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 12 Sep 2017
    /// Summary: Interface for Bike Series BAL
    /// Modified by : Ashutosh Sharma on 12-Sep-2017
    /// Description : Added EditSeries and DeleteSeries
    /// </summary>
    public interface IBikeSeries
    {
        IEnumerable<BikeSeriesEntity> GetSeries();
        void AddSeries(BikeSeriesEntity bikeSeries, uint updatedBy);
        IEnumerable<BikeSeriesEntityBase> GetSeriesByMake(int makeId);
        bool EditSeries(BikeSeriesEntity bikeSeries, int UpdatedBy);
        bool DeleteSeries(uint bikeSeriesId);
    }
}
