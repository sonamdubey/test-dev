
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
            viewModel.UserId = Convert.ToUInt32(CurrentUser.Id);
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