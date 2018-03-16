using System;
using System.Collections.Generic;
using System.Linq;
using ApiGatewayLibrary;
using VehicleData.Service.ProtoClass;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using Google.Protobuf.Collections;

namespace Bikewale.BAL.GrpcFiles.Specs_Features
{
    public static class SpecsFeaturesServiceGateway
    {
        public static SpecsFeaturesEntity Call()
        {
            try
            {
                CallAggregator ca = new CallAggregator();
                ca.AddCall("vehicledata_local", "GetVehicleDataForVersionId", new VehicleDataRequest
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
                specsFeaturesEntity = new SpecsFeaturesEntity();
                if (vehicleDataValue != null)
                {
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
    }
}
