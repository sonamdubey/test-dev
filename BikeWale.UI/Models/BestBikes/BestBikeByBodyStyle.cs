using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    public class BestBikeByBodyStyle
    {
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;

        public EnumBikeBodyStyles BodyStyleType;

        public uint topCount { get; set; }
        public BestBikeByBodyStyle(IBikeModelsCacheRepository<int> objBestBikes)
        {

            _objBestBikes = objBestBikes;
        }

        /// <summary>
        /// Modified By : Snehal Dange on 24th Nov 2017
        /// Description: Added EnumBikeBodyStyles  for each category
        /// Modified by : Pratibha Verma on 27 Mar 2018
        /// Description : Added method call to get MinSpecs data from grpc
        /// </summary>
        /// <returns></returns>
        public BestBikeByCategoryVM GetData()
        {
            BestBikeByCategoryVM objData = null;
            try
            {
                objData = new BestBikeByCategoryVM();

                objData.objBestScootersList = FetchBestBikesList(EnumBikeBodyStyles.Scooter);
                if (objData.objBestScootersList != null && objData.objBestScootersList.Any())
                {
                    IEnumerable<VersionMinSpecsEntity> scooterVersionMinSpecs = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(objData.objBestScootersList.Select(m => m.VersionId));
                    foreach (var bestScooter in objData.objBestScootersList)
                    {
                        VersionMinSpecsEntity scooterMinSpecs = scooterVersionMinSpecs.FirstOrDefault(x => x.VersionId.Equals(bestScooter.VersionId));
                        bestScooter.MinSpecsList = scooterMinSpecs != null ? scooterMinSpecs.MinSpecsList : null;
                    }
                }
                objData.objBestSportsBikeList = FetchBestBikesList(EnumBikeBodyStyles.Sports);
                if (objData.objBestSportsBikeList != null && objData.objBestSportsBikeList.Any())
                {
                    IEnumerable<VersionMinSpecsEntity> sportsVersionMinSpecs = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(objData.objBestSportsBikeList.Select(m => m.VersionId));
                    foreach (var bestSports in objData.objBestSportsBikeList)
                    {
                        VersionMinSpecsEntity sportsMinSpecs = sportsVersionMinSpecs.FirstOrDefault(x => x.VersionId.Equals(bestSports.VersionId));
                        bestSports.MinSpecsList = sportsMinSpecs != null ? sportsMinSpecs.MinSpecsList : null;
                    }
                }
                objData.objBestCruiserBikesList = FetchBestBikesList(EnumBikeBodyStyles.Cruiser);
                if (objData.objBestCruiserBikesList != null && objData.objBestCruiserBikesList.Any())
                {
                    IEnumerable<VersionMinSpecsEntity> cruiserVersionMinSpecs = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(objData.objBestCruiserBikesList.Select(m => m.VersionId));
                    foreach (var bestCruiser in objData.objBestCruiserBikesList)
                    {
                        VersionMinSpecsEntity cruiserMinSpecs = cruiserVersionMinSpecs.FirstOrDefault(x => x.VersionId.Equals(bestCruiser.VersionId));
                        bestCruiser.MinSpecsList = cruiserMinSpecs != null ? cruiserMinSpecs.MinSpecsList : null;
                    }
                }
                objData.objBestMileageBikesList = FetchBestBikesList(EnumBikeBodyStyles.Mileage);
                if (objData.objBestMileageBikesList != null && objData.objBestMileageBikesList.Any())
                {
                    IEnumerable<VersionMinSpecsEntity> mileageVersionMinSpecs = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(objData.objBestMileageBikesList.Select(m => m.VersionId));
                    foreach (var bestMileage in objData.objBestMileageBikesList)
                    {
                        VersionMinSpecsEntity mileageMinSpecs = mileageVersionMinSpecs.FirstOrDefault(x => x.VersionId.Equals(bestMileage.VersionId));
                        bestMileage.MinSpecsList = mileageMinSpecs != null ? mileageMinSpecs.MinSpecsList : null;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.GetData()");
            }
            return objData;
        }


        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model FetchBestBikesList;
        /// </summary>
        private IEnumerable<BestBikeEntityBase> FetchBestBikesList(EnumBikeBodyStyles BodyStyleType)
        {
            IEnumerable<BestBikeEntityBase> objBikesList = null;
            try
            {


                objBikesList = _objBestBikes.GetBestBikesByCategory(BodyStyleType);

                if (objBikesList != null)
                {
                    objBikesList = objBikesList.Reverse();
                    objBikesList = objBikesList.Take((int)topCount);
                    if (objBikesList.Any())
                    {
                        objBikesList.First().CurrentPage = BodyStyleType;
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("FetchBestBikesList BodyStyle:{0}", BodyStyleType));
            }
            return objBikesList;
        }
    }
}