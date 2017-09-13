using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 11th Sep 2017
    /// Summary: Interface for Bike series repository
    /// </summary>
    public interface IBikeSeriesRepository
    {
        IEnumerable<BikeSeriesEntity> GetSeries();
        void AddSeries(BikeSeriesEntity bikeSeries, uint UpdatedBy);
        bool EditSeries(BikeSeriesEntity bikeSeries, int updatedBy);
        bool DeleteSeries(uint bikeSeriesId);
    }
}
