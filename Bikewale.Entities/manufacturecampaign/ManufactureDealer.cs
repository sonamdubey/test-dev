
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities
{
    [Serializable, DataContract]
    public class ManufactureDealer
    {
        public string Id { get; set; }
        public string DealerName { get; set; }
        public string DealerArea { get; set; }
        public string City { get; set; }
        public uint CityId { get; set; }
    }
}
