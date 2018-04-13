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
using VehicleData.Service.ProtoClass;

namespace Bikewale.BAL.ApiGateway.Adapters.BikeData
{
    /// <summary>
    /// Created By : Deepak Israni on 12 April 2018
    /// Description: Adapter class responsible for executing GRPC method to get related data for
    /// </summary>
    public class GetCustomDataTypesByItemIdAdapter : AbstractApiGatewayAdapater<GetCustomDataType_Input, IEnumerable<SpecsCustomDataType>, CustomDataTypeList>
    {
        /// <summary>
        /// Constructor will set all dependencies required to get the data from APIGateway
        /// </summary>
        public GetCustomDataTypesByItemIdAdapter()
        {
            ModuleName = BWConfiguration.Instance.SpecsFeaturesServiceModuleName;
            MethodName = "GetCustomDataTypesByItemId";
        }

        /// <summary>
        /// Created By : Deepak Israni on 12 April 2018
        /// Description: Function converts the input into an input compatible with the GRPC service.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IMessage BuildRequest(GetCustomDataType_Input input)
        {
            return new GrpcInt()
            {
                Value = (int) input.InputType
            };
        }

        /// <summary>
        /// Created By : Deepak Israni on 12 April 2018
        /// Description: Function converts the output of the GRPC service into a compatible Bikewale entity.
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Created By : Deepak Israni on 12 April 2018
        /// Description: Function performs operations and binds data returned from GRPC service to a bikewale compatible data type.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
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
