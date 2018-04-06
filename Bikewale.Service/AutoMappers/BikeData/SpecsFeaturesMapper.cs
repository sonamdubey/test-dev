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
                        SpecsItem specsItem = objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.BrakeType));
                        if (specsItem != null)
                        {
                            objVersion.BrakeType = specsItem.Value;
                        }
                        objVersion.AlloyWheels = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.AlloyWheels)));
                        objVersion.ElectricStart = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.ElectricStart)));
                        objVersion.AntilockBrakingSystem = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.AntilockBrakingSystem)));
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
                        SpecsItem specsItem = objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.BrakeType));
                        if (specsItem != null)
                        {
                            objVersionDetail.BrakeType = specsItem.Value;
                        }
                        objVersionDetail.AlloyWheels = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.AlloyWheels)));
                        objVersionDetail.ElectricStart = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.ElectricStart)));
                        objVersionDetail.AntilockBrakingSystem = CheckBoolSpecItem(objVersionMinSpec.MinSpecsList.FirstOrDefault(specItem => specItem.Id.Equals((int)EnumSpecsFeaturesItem.AntilockBrakingSystem)));
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

        public static IEnumerable<MostPopularBikes> ConvertToMostPopularBikes(IEnumerable<MostPopularBikes> popularBikeListDto, IEnumerable<MostPopularBikesBase> popularBikeList)
        {
            try
            {
                if (popularBikeList != null)
                {
                    IEnumerator<MostPopularBikesBase> popularBikeEnumerator = popularBikeList.GetEnumerator();
                    float specValue;

                    IEnumerable<PropertyInfo> minSpecPropList = new MinSpecs().GetType().GetProperties();
                    IEnumerator<PropertyInfo> minSpecPropEnumerator = minSpecPropList.GetEnumerator();
                    IEnumerable<SpecsItem> specItemList;
                    foreach (var dtoBike in popularBikeListDto)
                    {
                        if (popularBikeEnumerator.MoveNext())
                        {
                            dtoBike.Specs = new MinSpecs();
                            specItemList = popularBikeEnumerator.Current.MinSpecsList;
                            foreach (var spec in specItemList)
                            {
                                specValue = float.TryParse(spec.Value, out specValue) ? specValue : 0;
                                if (minSpecPropEnumerator.MoveNext())
                                {
                                    minSpecPropEnumerator.Current.SetValue(dtoBike.Specs, specValue);
                                }
                            }
                            minSpecPropEnumerator.Reset();
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

        private static bool CheckBoolSpecItem(SpecsItem specItem)
        {
            return specItem != null && specItem.Value.Equals("1") ? true : false;
        }


    }
}