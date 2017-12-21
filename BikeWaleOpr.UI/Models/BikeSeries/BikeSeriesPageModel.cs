using Bikewale.Notifications;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;

namespace BikewaleOpr.Models.BikeSeries
{
    public class BikeSeriesPageModel
    {
        private readonly IBikeMakes _makes;
        private readonly IBikeSeries _series;
        private readonly IBikeBodyStyles _bodystyles = null;
        public BikeSeriesPageModel(IBikeMakes makes, IBikeSeries series, IBikeBodyStyles bodystyles)
        { 
            _makes = makes;
            _series = series;
            _bodystyles = bodystyles;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 11th Sep 2017
        /// Summary : Get VM data for bike series page
        /// Modified by : Rajan Chauhan on 12th Dec 2017
        /// Summary : Added bike bodystyles list to objBikeSeriesVM
        /// </summary>
        /// <returns></returns>
        public BikeSeriesPageVM GetData()
        {
            BikeSeriesPageVM objBikeSeriesVM = new BikeSeriesPageVM();
            try
            {
                objBikeSeriesVM.BikeMakesList = _makes.GetMakes((ushort)EnumBikeType.New);
                objBikeSeriesVM.BikeSeriesList = _series.GetSeries();
                objBikeSeriesVM.BikeBodyStylesList = _bodystyles.GetBodyStylesList();
                objBikeSeriesVM.PageTitle = "Manage Bike Series";
                objBikeSeriesVM.UpdatedBy = Convert.ToUInt32(BikeWaleOpr.Common.CurrentUser.Id);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BikeSeriesPageMOdel: GetData");
            }
            return objBikeSeriesVM;
        }
    }
}