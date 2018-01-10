using Bikewale.Notifications;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.BAL
{
    public class BikeBodyStyles: IBikeBodyStyles
    {
        private readonly IBikeBodyStylesRepository _bikeBodyStylesRepository;
        public BikeBodyStyles(IBikeBodyStylesRepository bikeBodyStylesRepository)
        {
            _bikeBodyStylesRepository = bikeBodyStylesRepository;
        }
        /// <summary>
        /// Created by : Rajan Chauhan on 13th Dec 2017
        /// Description : To Get list of Body Styles
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeBodyStyleEntity> GetBodyStylesList()
        {
            IEnumerable<BikeBodyStyleEntity> objBikeBodyStyleList = null;
            try
            {
                objBikeBodyStyleList = _bikeBodyStylesRepository.GetBodyStylesList();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.BikeBodyStyles.GetBodyStylesList"));
            }
            return objBikeBodyStyleList;
        }
    }
}
