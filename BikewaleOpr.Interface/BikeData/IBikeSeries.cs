using BikewaleOpr.Entity.BikeData;
using System;
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
        Tuple<bool, string, BikeSeriesEntity> AddSeries(uint makeId, string seriesName, string seriesMaskingName, uint updatedBy, bool isSeriesPageUrl, uint? bodyStyleId);
        IEnumerable<BikeSeriesEntityBase> GetSeriesByMake(int makeId);
        Tuple<bool, string> EditSeries(uint makeId, uint seriesId, string seriesName, string seriesMaskingName, int updatedBy, bool isSeriesPageUrl, uint? bodyStyleId);
        bool DeleteSeries(uint bikeSeriesId, uint deletedBy);
        bool DeleteMappingOfModelSeries(uint modelId);
        SynopsisData Getsynopsis(int seriesId);
        bool UpdateSynopsis(int seriesId, int updatedBy, SynopsisData objSynopsis);
        bool IsSeriesMaskingNameExists(uint makeId, string seriesMaskingName);
    }
}
