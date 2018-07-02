using Bikewale.Notifications;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Interface.Location;
using System;
using System.Linq;
using System.Xml;

namespace BikewaleOpr.Models.ManagePrices
{
    /// <summary>
    /// Created By : Prabhu Puredla on 18 May 2018
    /// Description : View model for bulk price upload
    /// </summary>
    public class BulkPriceModel
    {
        private readonly IBulkPriceRepository _bulkPriceRepos;
        private readonly IBikeMakesRepository _bikeMakeRepos;
        private readonly ILocation _location;
        private readonly IBulkPrice _bulkPrice;

        public BulkPriceModel(IBulkPriceRepository bulkPriceRepos, IBikeMakesRepository bikeMakeRepos, ILocation location, IBulkPrice bulkPrice)
        {
            _bulkPriceRepos = bulkPriceRepos;
            _bikeMakeRepos = bikeMakeRepos;
            _location = location;
            _bulkPrice = bulkPrice;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 18 May 2018
        /// Description : Method to fetch mapped bikes for a make, all Makes and States for dropdown from repository
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>       
        public MappedBikesVM GetMappedBikesData(uint makeId)
        {
            MappedBikesVM mappedBikesVM = new MappedBikesVM();
            try
            {
                mappedBikesVM.MappedBikes = _bulkPriceRepos.GetMappedBikesData(makeId);
                mappedBikesVM.MakesListVM = GetMakesAndStates();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Models.ManagePrices.BulkPriceModel.GetMappedBikesData");
            }
            return mappedBikesVM;
        }

        /// <summary>
        ///  Created By : Prabhu Puredla on 22 May 2018
        /// Description : Method to fetch all mapped cities for a state, all Makes and States for dropdown
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public MappedCitiesVM GetMappedCitiesData(uint stateId)
        {
            MappedCitiesVM mappedCitiesVM = new MappedCitiesVM();
            try
            {
                mappedCitiesVM.MappedCities = _bulkPriceRepos.GetMappedCitiesData(stateId);
                mappedCitiesVM.MakesListVM = GetMakesAndStates();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Models.ManagePrices.BulkPriceModel.GetMappedCitiesData");
            }
            return mappedCitiesVM;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 18 May 2018
        /// Description : Method to fetch all Makes and States for dropdown from repository
        /// </summary>
        /// <returns></returns>
        public MakesAndStatesVM GetMakesAndStates()
        {
            MakesAndStatesVM makesAndStatesVM = new MakesAndStatesVM();
            try
            {
                makesAndStatesVM.BikeMakes = _bikeMakeRepos.GetMakes("NEW");
                makesAndStatesVM.States = _location.GetStates();
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Models.ManagePrices.BulkPriceModel.GetMakesAndStates");
            }            
            return makesAndStatesVM;
        }

        /// <summary>
        ///  Created By : Prabhu Puredla on 18 May 2018
        /// Description : Method to fetch  makes and states for dropdown from repository
        /// </summary>
        /// <returns></returns>
        public UploadPricesVM GetUploadPricesVM()
        {
            UploadPricesVM uploadPricesVM = new UploadPricesVM()
            { 
                MakesAndStatesListVM = GetMakesAndStates(),
            };
            return uploadPricesVM;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Get processed data for OemPrices 
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public CompositeBulkPriceVM GetProcessedData(uint makeId, XmlReader xmlData)
        {
            try
            {
                CompositeBulkPriceEntity compositeBulkPriceEntity = _bulkPrice.GetProcessedData(makeId, xmlData);
                
                if (compositeBulkPriceEntity != null)
                {
                    CompositeBulkPriceVM compositeBulkPriceVM = new CompositeBulkPriceVM()
                    {
                        UnmappedBikes = compositeBulkPriceEntity.UnmappedBikes.OrderBy(s => s).ToList(),
                        UnmappedCities = compositeBulkPriceEntity.UnmappedCities.OrderBy(s => s).ToList(),
                        BikeModelList = compositeBulkPriceEntity.BikeModelList,
                        States = compositeBulkPriceEntity.States,
                        UpdatedPriceList = compositeBulkPriceEntity.UpdatedPriceList,
                        UnmappedOemPricesList = compositeBulkPriceEntity.UnmappedOemPricesList
                    };                       
                    return compositeBulkPriceVM;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Models.ManagePrices.BulkPriceModel.GetProcessedData");
            }
            return null;
        }
    }
}