using Bikewale.Notifications;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Dealers;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.BAL.BikePricing
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 03 Aug 2017
    /// Description :   Handles BAL operations for dealer bikeversion availability.
    /// </summary>
    public class VersionAvailability : IVersionAvailability
    {
        private readonly IDealers _dealersRepository;
        public VersionAvailability(IDealers dealersRepositoryObject)
        {
            _dealersRepository = dealersRepositoryObject;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 03 Aug 2017
        /// Description :   Handles BAL operations for saving bikeversion availability.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="bikeVersionIds"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public bool SaveVersionAvailability(uint dealerId, IEnumerable<uint> bikeVersionIds, IEnumerable<uint> numberOfDays)
        {
            bool isSaved = false;
            string bikeVersionIdStrings = null;
            string numberOfDaysStrings = null;

            try
            {
                bikeVersionIdStrings = string.Join<uint>(",", bikeVersionIds);
                numberOfDaysStrings = string.Join<uint>(",", numberOfDays);

                isSaved = _dealersRepository.SaveVersionAvailability(dealerId, bikeVersionIdStrings, numberOfDaysStrings);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format(
                    "SaveBikeAvailability dealerId={0} bikeVersionId={1} numberOfDays={2}", dealerId, bikeVersionIdStrings, numberOfDaysStrings));
            }

            return isSaved;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 03 Aug 2017
        /// Description :   Handles BAL operations for deleting bikeversion availability.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="bikeVersionIds"></param>
        /// <returns></returns>
        public bool DeleteVersionAvailability(uint dealerId, IEnumerable<uint> bikeVersionIds)
        {
            bool isDeleted = false;
            string bikeVersionIdStrings = null;

            try
            {
                bikeVersionIdStrings = string.Join<uint>(",", bikeVersionIds);

                isDeleted = _dealersRepository.DeleteVersionAvailability(dealerId, bikeVersionIdStrings);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format(
                    "DeleteVersionAvailability dealerId={0} bikeVersionId={1}", dealerId, bikeVersionIdStrings));
            }

            return isDeleted;
        }
    }
}
