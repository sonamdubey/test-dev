using System;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By  : Rajan Chauhan on 21 Mar 2018
    /// Description : Entity for MinSpecs
    /// </summary>
    [Serializable]
    public class SpecsItem
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public EnumSpecDataType DataType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string UnitType { get; set; }
    }
}
