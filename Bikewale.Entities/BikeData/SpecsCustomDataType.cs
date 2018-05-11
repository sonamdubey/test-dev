using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Deepak Israni on 12 April 2018
    /// Description: Entity to bind custom data types returned from GRPC.
    /// </summary>
    public class SpecsCustomDataType
    {
        public ushort Id { get; set; }
        public string Value { get; set; }
    }
}
