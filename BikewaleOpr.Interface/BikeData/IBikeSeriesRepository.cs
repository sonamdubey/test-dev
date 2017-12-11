using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 11th Sep 2017
    /// Summary: Interface for Bike series repository
    /// Modified by : Ashutosh Sharma on 12 Sep 2017
    /// Description : Added EditSeries and DeleteSeries
    /// Modified by : Ashutosh Sharma on 30 Nov 2017
    /// Description : Added GetModelIdsBySeries
    /// </summary>
    public interface IBikeSeriesRepository
    {
        IEnumerable<BikeSeriesEntity> GetSeries();
        void AddSeries(BikeSeriesEntity bikeSeries, uint updatedBy);
        IEnumerable<BikeSeriesEntityBase> GetSeriesByMake(int makeId);
        bool EditSeries(BikeSeriesEntity bikeSeries, int updatedBy);
        bool DeleteSeries(uint bikeSeriesId, uint deletedBy);
        int DeleteMappingOfModelSeries(uint modelId);
        bool UpdateSynopsis(int seriesId, int updatedBy, SynopsisData objSynopsis);
        SynopsisData Getsynopsis(int seriesId);
        bool IsSeriesMaskingNameExists(uint makeId, string seriesMaskingName);
        string GetModelIdsBySeries(uint seriesId);
    }
}
