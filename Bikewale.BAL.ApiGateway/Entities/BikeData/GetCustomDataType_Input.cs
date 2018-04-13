using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.BAL.ApiGateway.Entities.BikeData
{
    /// <summary>
    /// Created By : Deepak Israni on 12 April 2018
    /// Description: Input wrapper for GetCustomDataTypeByItemIdAdapter
    /// </summary>
    public class GetCustomDataType_Input
    {
        public EnumSpecsFeaturesItems InputType { get; set; }
    }
}
