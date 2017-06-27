
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities
{
    [Serializable, DataContract]
    public class ManufacturerDealer
    {
        public string Id { get; set; }
        public uint DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerArea { get; set; }
        public string City { get; set; }
        public uint CityId { get; set; }
    }
}
