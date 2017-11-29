﻿using Bikewale.Notifications;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;

namespace BikewaleOpr.Models.BikeSeries
{
    public class BikeSeriesPageModel
    {
        private readonly IBikeMakes _makes;
        private readonly IBikeSeries _series;
        public BikeSeriesPageModel(IBikeMakes makes, IBikeSeries series)
        { 
            _makes = makes;
            _series = series;
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
                objBikeSeriesVM.BikeMakesList = _makes.GetMakes((ushort)EnumBikeType.New);
                objBikeSeriesVM.BikeSeriesList = _series.GetSeries();
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