
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.AdOperations;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.BikeData;
using BikeWaleOpr.Common;
using System;
namespace BikewaleOpr.Models.AdOperation
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Description: Model created to manage ad-operations
    /// </summary>
    public class AdOperation
    {
        private readonly IAdOperation _adOperations = null;
        private readonly IBikeMakesRepository _objBikeMake = null;
        public AdOperation(IAdOperation adOperations, IBikeMakesRepository objBikeMake)
        {
            _adOperations = adOperations;
            _objBikeMake = objBikeMake;

        }

        public AdOperationVM GetData()
        {
            AdOperationVM viewModel = new AdOperationVM();
            GetPromotedBikes(viewModel);
            GetMakes(viewModel);
            return viewModel;

        }


        /// <summary>
        /// Created by : Snehal Dange on 2nd Jan 2018
        /// Desc: Method created to get list of promoted bikes
        /// </summary>
        public void GetPromotedBikes(AdOperationVM viewModel)
        {

            try
            {

                viewModel.PromotedBikeList = _adOperations.GetPromotedBikes();

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, " BikewaleOpr.Models.AdOperation.GetPromotedBikes");
            }

        }

        /// <summary>
        /// Created by : Snehal Dange on 2nd Jan 2018
        /// Desc :  Method created to add promoted bike
        /// </summary>
        /// <param name="objPromotedBike"></param>
        /// <returns></returns>
        public bool SavePromotedBike(PromotedBike objPromotedBike)
        {
            bool status = false;
            try
            {
                status = _adOperations.SavePromotedBike(objPromotedBike);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.Models.AdOperation.SavePromotedBike:MakeId:{0} ,ModelId: {1}", objPromotedBike.Make.MakeId, objPromotedBike.Model.ModelId));
            }
            return status;
        }

        public void GetMakes(AdOperationVM viewModel)
        {
            try
            {
                viewModel.Makes = _objBikeMake.GetMakes("NEW");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.Models.AdOperation.GetMakes"));
            }
        }


    }
}