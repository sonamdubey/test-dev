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
        public static SpecsFeaturesEntity Call()
        {
            try
            {
                CallAggregator ca = new CallAggregator();
                ca.AddCall(BWConfiguration.Instance.SpecsFeaturesServiceModuleName, "GetVehicleDataForVersionId", new VehicleDataRequest
                {
                    VersionIds = { new List<int> { 5242, 5150 } },
                    ApplicationId = 1
                });
                var apiData = ca.GetResultsFromGateway();

                if (apiData != null && apiData.OutputMessages != null && apiData.OutputMessages.Count > 0)
                {
                    return ConvertToBwSpecsFeaturesEntity(Utilities.ConvertBytesToMsg<VehicleDataValue>(apiData.OutputMessages[0].Payload));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.Call");
            }
            return null;

        }

        private static SpecsFeaturesEntity ConvertToBwSpecsFeaturesEntity(VehicleDataValue vehicleDataValue)
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
                            SpecsItemList = ConvertToBwSpecsFeaturesEntity(category.Items)
                        });
                    }
                    specsFeaturesEntity.Specs = specs;
                    var specsCategory = vehicleDataValue.Features.FirstOrDefault();
                    if (specsCategory != null)
                    {
                        var features = ConvertToBwSpecsFeaturesEntity(specsCategory.Items);
                        specsFeaturesEntity.Features = features;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToBWSpecsFeaturesEntity");
            }
            return specsFeaturesEntity;
        }

        private static IEnumerable<SpecsFeaturesItem> ConvertToBwSpecsFeaturesEntity(RepeatedField<Item> items)
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
                ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToBWSpecFeaturesItemList");
            }
            return null;
        }
        /// <summary>
        /// Created By  : Rajan Chauhan on 22 Mar 2018
        /// Summary     : To get List of versionMinSpecs when given input 
        /// </summary>
        /// <param name="numberOfVersions"></param>
        /// <returns></returns>
        public static IEnumerable<VersionMinSpecsEntity> GetVersionMinSpecs(int numberOfVersions)
        {
            try
            {
                
                CallAggregator ca = new CallAggregator();
                ca.AddCall(BWConfiguration.Instance.SpecsFeaturesServiceModuleName, "VersionsDataByItemIds", new VersionsDataByItemIdsRequest
                {
                    ItemIds = { new List<int> { 6, 14, 12, 249, 250 } },
                    VersionIds = { Enumerable.Range(5000, numberOfVersions).ToList() },
                    ApplicationId = 1
                });
                var apiData = ca.GetResultsFromGateway();
                if (apiData != null && apiData.OutputMessages != null && apiData.OutputMessages.Count > 0)
                {
                    return ConvertToBWVersionMinSpecsList(Utilities.ConvertBytesToMsg<VersionItemsDataResponse>(apiData.OutputMessages[0].Payload));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.GetMinSpecs");
            }
            return null;
        }

        private static IEnumerable<VersionMinSpecsEntity> ConvertToBWVersionMinSpecsList(VersionItemsDataResponse vehicleItemsDataResponse)
        {
            try
            {
                if (vehicleItemsDataResponse != null)
                {
                    IList<VersionMinSpecsEntity>  versionMinSpecsList = new List<VersionMinSpecsEntity>();
                    foreach (var versionItemsData in vehicleItemsDataResponse.VersionItemsDataList)
                    {
                        versionMinSpecsList.Add(new VersionMinSpecsEntity
                        {
                            VersionId = versionItemsData.Id,
                            MinSpecsList = ConvertToBWMinSpecsList(versionItemsData.ItemList)
                        });
                    }

                    return versionMinSpecsList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToBWVersionMinSpecsList");
            }
            return null;
        }

        private static IEnumerable<SpecsItem> ConvertToBWMinSpecsList(RepeatedField<ItemData> itemDataList)
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
                ErrorClass.LogError(ex, "SpecsFeaturesServiceMethods.ConvertToBWMinSpecsList");
            }
            return specItemList;
        }
    }
}
