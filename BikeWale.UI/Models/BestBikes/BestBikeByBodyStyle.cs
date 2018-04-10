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
                BindMinSpecs(objData.objBestScootersList);
                objData.objBestSportsBikeList = FetchBestBikesList(EnumBikeBodyStyles.Sports);
                BindMinSpecs(objData.objBestSportsBikeList);
                objData.objBestCruiserBikesList = FetchBestBikesList(EnumBikeBodyStyles.Cruiser);
                BindMinSpecs(objData.objBestCruiserBikesList);
                objData.objBestMileageBikesList = FetchBestBikesList(EnumBikeBodyStyles.Mileage);
                BindMinSpecs(objData.objBestMileageBikesList);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.GetData()");
            }
            return objData;
        }

        /// <summary>
        /// Created By : Pratibha Verma on 27 Mar 2018
        /// Summary : Bind MinSpecs to Generic Bike List
        /// </summary>
        private void BindMinSpecs(IEnumerable<BestBikeEntityBase> GenericBikeList)
        {
            try
            {
                if (GenericBikeList != null && GenericBikeList.Any())
                {
                    IEnumerable<VersionMinSpecsEntity> versionMinSpecsEntityList = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(GenericBikeList.Select(m => m.VersionId));
                    if (versionMinSpecsEntityList != null)
                    {
                        IEnumerator<VersionMinSpecsEntity> versionIterator = versionMinSpecsEntityList.GetEnumerator();
                        VersionMinSpecsEntity objVersionMinSpec;
                        foreach (var genericBike in GenericBikeList)
                        {
                            if (versionIterator.MoveNext())
                            {
                                objVersionMinSpec = versionIterator.Current;
                                genericBike.MinSpecsList = objVersionMinSpec != null ? objVersionMinSpec.MinSpecsList : null;
                            }
                        }
                    }
                    
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.BestBikeByBodyStyle.BindMinSpecs({0})", GenericBikeList));
            }

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