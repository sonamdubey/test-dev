using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Models.BikeSeries
{
    public class SeriesPage
    {
        private readonly IBikeSeriesCacheRepository _seriesCache;

        public SeriesPage(IBikeSeriesCacheRepository seriesCache)
        {
            _seriesCache = seriesCache;

        }

        public SeriesPageVM GetData()
        {
            SeriesPageVM objSeriesPage = null;
            try
            {
                objSeriesPage = new SeriesPageVM();
                objSeriesPage.SeriesModels = _seriesCache.GetModelsListBySeriesId(2);

                GetBikesToCompare(objSeriesPage);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.GetData");
            }
            return objSeriesPage;
        }
        /// <summary>
        /// Created By :- Subodh Jain 17-11-2013
        /// Summary :- GetCompareBikes Details
        /// </summary>
        /// <param name="objSeriesPage"></param>
        private void GetBikesToCompare(SeriesPageVM objSeriesPage)
        {
            objSeriesPage.ObjModel = new BikeSeriesCompareVM();
            objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs = _seriesCache.GetBikesToCompare(3);

            IList<string> objList = new List<string>();
            objList.Add("Price");
            objList.Add("Displacement");
            objList.Add("Weight");
            objList.Add("Fuel Tank Capacity");
            objList.Add("Mileage");
            objList.Add("Seat Height");
            objList.Add("Brake Type");
            objList.Add("Gears");
            objList.Add("Max Power");


            objSeriesPage.ObjModel.ObjBikeSpecs = new BikeSpecs();
            var objData = objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.ToList();

            objSeriesPage.ObjModel.ObjBikeSpecs.MaxPower = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.MaxPower != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.MaxPower)).Count() + 1);
            objSeriesPage.ObjModel.ObjBikeSpecs.Mileage = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.Mileage != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.Mileage)).Count() + 1);
            objSeriesPage.ObjModel.ObjBikeSpecs.Weight = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.Weight != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Min(m => m.Weight)).Count() + 1);
            objSeriesPage.ObjModel.ObjBikeSpecs.FuelCapacity = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.FuelCapacity != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.FuelCapacity)).Count() + 1);
            objSeriesPage.ObjModel.ObjBikeSpecs.Displacement = (ushort)(objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.TakeWhile(x => x.Displacement != objSeriesPage.ObjModel.BikeSeriesCompareBikeWithSpecs.Max(m => m.Displacement)).Count() + 1);





            objSeriesPage.ObjModel.BikeCompareSegments = objList;




        }

    }
}