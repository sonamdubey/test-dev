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
        BikeSeriesEntity AddSeries(uint makeId, string seriesName, string seriesMaskingName, uint updatedBy);
        IEnumerable<BikeSeriesEntityBase> GetSeriesByMake(int makeId);
        bool EditSeries(uint seriesId, string seriesName, string seriesMaskingName, int updatedBy);
        bool DeleteSeries(uint bikeSeriesId);
        bool DeleteMappingOfModelSeries(uint modelId);
    }
}
