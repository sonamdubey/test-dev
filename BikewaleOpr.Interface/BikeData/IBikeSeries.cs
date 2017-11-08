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
        BikeSeriesEntity AddSeries(uint makeId, string seriesName, string seriesMaskingName, uint updatedBy, bool isSeriesPageUrl);
        IEnumerable<BikeSeriesEntityBase> GetSeriesByMake(int makeId);
        bool EditSeries(uint seriesId, string seriesName, string seriesMaskingName, int updatedBy, bool isSeriesPageUrl);
        bool DeleteSeries(uint bikeSeriesId, uint deletedBy);
        bool DeleteMappingOfModelSeries(uint modelId);
        SynopsisData Getsynopsis(int seriesId);
        bool UpdateSynopsis(int seriesId, int updatedBy, SynopsisData objSynopsis);
    }
}
