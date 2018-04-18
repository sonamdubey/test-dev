using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Model.v3;
using Bikewale.DTO.Version;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bikewale.Service.AutoMappers.BikeData
{
    public class SpecsFeaturesMapper
    {
        public static VersionMinSpecs ConvertToVersionMinSpecs(BikeVersionMinSpecs objVersionMinSpec)
        {
            try
            {
                if (objVersionMinSpec != null)
                {
                    VersionMinSpecs objVersion = new VersionMinSpecs();
                    objVersion.VersionId = objVersionMinSpec.VersionId;
                    objVersion.VersionName = objVersionMinSpec.VersionName;
                    objVersion.ModelName = objVersionMinSpec.ModelName;
                    objVersion.Price = objVersionMinSpec.Price;
                    if (objVersionMinSpec.MinSpecsList != null)
                    {
                        SpecsItem specsItem = objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItems.RearBrakeType));
                        if (specsItem != null)
                        {
                            objVersion.BrakeType = specsItem.Value;
                        }
                        objVersion.AlloyWheels = IsAlloy(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItems.WheelType)));
                        objVersion.ElectricStart = IsElectric(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItems.StartType)));
                        objVersion.AntilockBrakingSystem = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItems.AntilockBrakingSystem)));
                    }
                    return objVersion;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Service.AutoMappers.BikeData.ConvertToVersionMinSpecs( BikeVersionMinSpecs {0})", objVersionMinSpec));
            }
            return null;
        }

        public static VersionDetail ConvertToVersionDetail(BikeVersionMinSpecs objVersionMinSpec)
        {
            try
            {
                if (objVersionMinSpec != null)
                {
                    VersionDetail objVersionDetail = new VersionDetail();
                    objVersionDetail.VersionId = objVersionMinSpec.VersionId;
                    objVersionDetail.VersionName = objVersionMinSpec.VersionName;
                    objVersionDetail.Price = objVersionMinSpec.Price;
                    if (objVersionMinSpec.MinSpecsList != null)
                    {
                        SpecsItem specsItem = objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItems.RearBrakeType));
                        if (specsItem != null)
                        {
                            objVersionDetail.BrakeType = specsItem.Value;
                        }
                        objVersionDetail.AlloyWheels = IsAlloy(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItems.WheelType)));
                        objVersionDetail.ElectricStart = IsElectric(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItems.StartType)));
                        objVersionDetail.AntilockBrakingSystem = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItems.AntilockBrakingSystem)));
                    }
                    return objVersionDetail;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Service.AutoMappers.BikeData.ConvertToVersionDetail( BikeVersionMinSpecs {0})", objVersionMinSpec));
            }
            return null;
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 07 Apr 2018
        /// Description : Convertor Function to convert to MostPopularBikes from MostPopularBikeBase
        ///               Underlying assumption that order of calling minSpecs will be in the converting order
        /// </summary>
        /// <param name="popularBikeListDto"></param>
        /// <param name="popularBikeList"></param>
        /// <returns></returns>
        public static IEnumerable<MostPopularBikes> ConvertToMostPopularBikes(IEnumerable<MostPopularBikes> popularBikeListDto, IEnumerable<MostPopularBikesBase> popularBikeList)
        {
            try
            {
                if (popularBikeList != null)
                {
                    IEnumerator<MostPopularBikesBase> popularBikeEnumerator = popularBikeList.GetEnumerator();
                    float specValue;
                    IEnumerable<SpecsItem> specItemList;
                    foreach (var dtoBike in popularBikeListDto)
                    {
                        if (popularBikeEnumerator.MoveNext())
                        {
                            dtoBike.Specs = new MinSpecs();
                            specItemList = popularBikeEnumerator.Current.MinSpecsList;
                            if (specItemList != null)
                            {
                                IEnumerator<SpecsItem> minSpecEnumerator = specItemList.GetEnumerator();
                                if (minSpecEnumerator.MoveNext())
                                {
                                    specValue = float.TryParse(minSpecEnumerator.Current.Value, out specValue) ? specValue : 0;
                                    dtoBike.Specs.Displacement = specValue;
                                }
                                if (minSpecEnumerator.MoveNext())
                                {
                                    specValue = float.TryParse(minSpecEnumerator.Current.Value, out specValue) ? specValue : 0;
                                    dtoBike.Specs.FuelEfficiencyOverall = specValue.Equals(0) ? null : (float?)specValue;
                                }
                                if (minSpecEnumerator.MoveNext())
                                {
                                    specValue = float.TryParse(minSpecEnumerator.Current.Value, out specValue) ? specValue : 0;
                                    dtoBike.Specs.MaxPower = specValue;
                                }
                                if (minSpecEnumerator.MoveNext())
                                {
                                    specValue = float.TryParse(minSpecEnumerator.Current.Value, out specValue) ? specValue : 0;
                                    dtoBike.Specs.MaximumTorque = specValue;
                                }
                                if (minSpecEnumerator.MoveNext())
                                {
                                    specValue = float.TryParse(minSpecEnumerator.Current.Value, out specValue) ? specValue : 0;
                                    dtoBike.Specs.KerbWeight = specValue;
                                }
                            }
                        }
                    }
                    return popularBikeListDto;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Service.AutoMappers.BikeData.ConvertToMostPopularBikes( {0}, {1})", popularBikeListDto, popularBikeList));
            }
            
            return null;
        }

        private static bool IsAlloy(SpecsItem specItem)
        {
            return specItem != null && specItem.Value.Equals("Alloy") ? true : false;
        }

        private static bool IsElectric(SpecsItem specItem)
        {
            return specItem != null && specItem.Value.Equals("Electric Start") ? true : false;
        }

        private static bool CheckBoolSpecItem(SpecsItem specItem)
        {
            return specItem != null && specItem.Value.Equals("1") ? true : false;
        }


    }
}