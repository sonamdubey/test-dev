using System;
using System.Collections.Generic;
using System.Linq;
using ApiGatewayLibrary;
using VehicleData.Service.ProtoClass;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using Google.Protobuf.Collections;
using Bikewale.Utility;

namespace Bikewale.BAL.GrpcFiles.Specs_Features
{
    public static class SpecsFeaturesServiceGateway
    {
        private static readonly IEnumerable<EnumSpecsFeaturesItem> _specsFeaturesItemIds;

        static SpecsFeaturesServiceGateway()
        {
            _specsFeaturesItemIds = new List<EnumSpecsFeaturesItem>
            {
                EnumSpecsFeaturesItem.Displacement,
                EnumSpecsFeaturesItem.FuelEfficiencyOverall,
                EnumSpecsFeaturesItem.MaxPower,
                EnumSpecsFeaturesItem.KerbWeight
            };
        }
        /// <summary>
        /// Created By  : Rajan Chauhan on 23 Mar 2018
        /// Summary     : private method to get SpecsFeatures data of specified verisonIds
        /// </summary>
        /// <param name="versionIds"></param>
        /// <returns></returns>
        private static SpecsFeaturesEntity GetVersionSpecsFeatures(IEnumerable<int> versionIds)
        {
            try
            {
                if (versionIds != null)
                {
                    CallAggregator ca = new CallAggregator();
                    ca.AddCall(BWConfiguration.Instance.SpecsFeaturesServiceModuleName, "GetVehicleDataForVersionId", new VehicleDataRequest
                    {
                        VersionIds = { versionIds },
                        ApplicationId = 2,
                        ItemGroupTypes = string.Format("{0},{1}",(int)ItemGroupTypes.Individual, (int)ItemGroupTypes.IndividuallyShown)
                    });
                    var apiData = ca.GetResultsFromGateway();

                    if (apiData != null && apiData.OutputMessages != null && apiData.OutputMessages.Count > 0)
                    {
                        return ConvertToSpecsFeaturesEntity(Utilities.ConvertBytesToMsg<VehicleDataValue>(apiData.OutputMessages[0].Payload));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("SpecsFeaturesServiceMethods.GetVersionSpecsFeatures(IEnumerable<int> {0})", versionIds));
            }
            return null;
        }

        public static SpecsFeaturesEntity GetVersionsSpecsFeatures(IEnumerable<uint> versionIds)
        {
            try
            {
                if (versionIds != null)
                {
                    return GetVersionSpecsFeatures(versionIds.Select(versionId => (int)versionId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("SpecsFeaturesServiceMethods.GetVersionsSpecsFeatures(IEnumerable<uint> {0})", versionIds));
            }
            return null;

        }

        public static SpecsFeaturesEntity GetVersionsSpecsFeatures(IEnumerable<int> versionIds)
        {
            try
            {
                if (versionIds != null)
                {
                    return GetVersionSpecsFeatures(versionIds.Select(versionId => versionId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("SpecsFeaturesServiceMethods.GetVersionSpecsFeatures(IEnumerable<int> {0})", versionIds));
            }
            return null;

        }

        private static SpecsFeaturesEntity ConvertToSpecsFeaturesEntity(VehicleDataValue vehicleDataValue)
        {
            SpecsFeaturesEntity specsFeaturesEntity = null;
            try
            {
                if (vehicleDataValue != null)
                {
                    specsFeaturesEntity = new SpecsFeaturesEntity();
                    var specs = new List<SpecsFeaturesCategory>();
                    foreach (var category in vehicleDataValue.Specifications)
                    {
                        specs.Add(new SpecsFeaturesCategory
                        {
                            DisplayText = category.Name,
                            SpecsItemList = ConvertToSpecFeaturesItemList(category.Items)
                        });
                    }
                    specsFeaturesEntity.Specs = specs;
                    var specsCategory = vehicleDataValue.Features.FirstOrDefault();
                    if (specsCategory != null)
                    {
                        var features = ConvertToSpecFeaturesItemList(specsCategory.Items);
                        specsFeaturesEntity.Features = features;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToSpecsFeaturesEntity");
            }
            return specsFeaturesEntity;
        }

        private static IEnumerable<SpecsFeaturesItem> ConvertToSpecFeaturesItemList(RepeatedField<Item> items)
        {
            try
            {
                if (items != null)
                {
                    var itemList = new List<SpecsFeaturesItem>();
                    foreach (var item in items)
                    {
                        itemList.Add(new SpecsFeaturesItem
                        {
                            DisplayText = item.Name,
                            Icon = item.Icon,
                            Id = item.Id,
                            ItemValues = item.ItemValues,
                            UnitTypeText = item.UnitTypeName
                        });
                    }
                    return itemList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToSpecFeaturesItemList");
            }
            return null;
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 22 Mar 2018
        /// Summary     : To get List of versionMinSpecs when given input versionIds and minSpecsIds
        /// </summary>
        /// <param name="versionIds"></param>
        /// <param name="minSpecsIds"></param>
        /// <returns></returns>
        public static IEnumerable<VersionMinSpecsEntity> GetVersionsMinSpecs(IEnumerable<int> versionIds, IEnumerable<EnumSpecsFeaturesItem> specsIds)
        {
            try
            {
                if (versionIds != null && specsIds != null)
                {
                    CallAggregator ca = new CallAggregator();
                    ca.AddCall(BWConfiguration.Instance.SpecsFeaturesServiceModuleName, "VersionsDataByItemIds", new VersionsDataByItemIdsRequest
                    {
                        ItemIds = { specsIds.Select(specId => (int)specId) },
                        VersionIds = { versionIds },
                        ApplicationId = 2
                    });
                    var apiData = ca.GetResultsFromGateway();
                    if (apiData != null && apiData.OutputMessages != null && apiData.OutputMessages.Count > 0)
                    {
                        return ConvertToVersionMinSpecsList(Utilities.ConvertBytesToMsg<VersionItemsDataResponse>(apiData.OutputMessages[0].Payload));
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("SpecsFeaturesServiceMethods.GetVersionsMinSpecs(IEnumerable<int> {0}, IEnumerable<EnumMinSpecs> {1})", versionIds, specsIds));
            }
            return null;
        }

        public static IEnumerable<VersionMinSpecsEntity> GetVersionsMinSpecs(IEnumerable<int> versionIds)
        {
            try
            {
                if (versionIds != null)
                {
                    return GetVersionsMinSpecs(versionIds, _specsFeaturesItemIds);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("SpecsFeaturesServiceMethods.GetVersionsMinSpecs(IEnumerable<int> {0})", versionIds));
            }
            return null;
        }

        private static IEnumerable<VersionMinSpecsEntity> ConvertToVersionMinSpecsList(VersionItemsDataResponse vehicleItemsDataResponse)
        {
            try
            {
                if (vehicleItemsDataResponse != null)
                {
                    IList<VersionMinSpecsEntity> versionMinSpecsList = new List<VersionMinSpecsEntity>();
                    foreach (var versionItemsData in vehicleItemsDataResponse.VersionItemsDataList)
                    {
                        versionMinSpecsList.Add(new VersionMinSpecsEntity
                        {
                            VersionId = versionItemsData.Id,
                            MinSpecsList = ConvertToMinSpecsList(versionItemsData.ItemList)
                        });
                    }

                    return versionMinSpecsList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToVersionMinSpecsList");
            }
            return null;
        }

        private static IEnumerable<SpecsItem> ConvertToMinSpecsList(RepeatedField<ItemData> itemDataList)
        {
            IList<SpecsItem> specItemList = null;
            try
            {
                if (itemDataList != null)
                {
                    specItemList = new List<SpecsItem>();
                    foreach (var itemData in itemDataList)
                    {
                        specItemList.Add(new SpecsItem
                        {
                            Id = itemData.ItemId,
                            Icon = itemData.Icon,
                            Name = itemData.ItemName,
                            Value = itemData.Value,
                            UnitType = itemData.UnitType
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToMinSpecsList");
            }
            return specItemList;
        }
    }
}
