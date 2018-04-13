using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using Google.Protobuf;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleData.Service.ProtoClass;

namespace Bikewale.BAL.ApiGateway.Adapters.BikeData
{
    public class GetCustomDataTypesByItemIdAdapter : AbstractApiGatewayAdapater<GetCustomDataType_Input, IEnumerable<SpecsCustomDataType>, CustomDataTypeList>
    {
        public GetCustomDataTypesByItemIdAdapter()
        {
            ModuleName = BWConfiguration.Instance.SpecsFeaturesServiceModuleName;
            MethodName = "GetCustomDataTypesByItemId";
        }

        protected override IMessage BuildRequest(GetCustomDataType_Input input)
        {
            return new GrpcInt()
            {
                Value = (int) input.InputType
            };
        }

        protected override IEnumerable<SpecsCustomDataType> BuildResponse(IMessage responseMessage)
        {
            IEnumerable<SpecsCustomDataType> dataTypes = null;

            try
            {
                CustomDataTypeList customDataTypeList = responseMessage as CustomDataTypeList;

                if (customDataTypeList != null)
                {
                    dataTypes = ConvertToSpecsCustomDataType(customDataTypeList.CustomDataTypes);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.GetCustomDataTypesByItemIdAdapter.BuildResponse");
            }

            return dataTypes;
        }

        private IEnumerable<SpecsCustomDataType> ConvertToSpecsCustomDataType(RepeatedField<CustomDataType> items)
        {
            try
            {
                if (items != null && items.Any())
                {
                    var itemList = new List<SpecsCustomDataType>();
                    foreach (var item in items)
                    {
                        itemList.Add(new SpecsCustomDataType
                        {
                            Id = Convert.ToUInt16(item.Id),
                            Value = Convert.ToString(item.Name)
                        });
                    }
                    return itemList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.GetCustomDataTypesByItemIdAdapter.ConvertToSpecsCustomDataType");
            }
            return null;
        }
    }
}
