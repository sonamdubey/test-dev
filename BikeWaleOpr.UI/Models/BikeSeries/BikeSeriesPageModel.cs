using Bikewale.Notifications;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;

namespace BikewaleOpr.Models.BikeSeries
{
    public class BikeSeriesPageModel
    {
        private readonly IBikeMakes _makes;
        private readonly IBikeSeriesRepository _seriesRepo;
        public BikeSeriesPageModel(IBikeMakes makes, IBikeSeriesRepository seriesRepo)
        { 
            _makes = makes;
            _seriesRepo = seriesRepo;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 11th Sep 2017
        /// Summary : Get VM data for bike series page
        /// </summary>
        /// <returns></returns>
        public BikeSeriesPageVM GetData()
        {
            BikeSeriesPageVM objBikeSeriesVM = new BikeSeriesPageVM();
            try
            {
                objBikeSeriesVM.BikeMakesList = _makes.GetMakes((ushort)EnumBikeType.All);
                objBikeSeriesVM.BikeSeriesList = _seriesRepo.GetSeries();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BikeSeriesPageMOdel: GetData");
            }
            return objBikeSeriesVM;
        }
    }
}