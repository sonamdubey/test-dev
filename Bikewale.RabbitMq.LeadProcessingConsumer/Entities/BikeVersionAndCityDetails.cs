using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.RabbitMq.LeadProcessingConsumer.Entities
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 22 Jun 2018
    /// Description : Entity to hold basic data of a bike version and city
    /// </summary>
    public class BikeVersionAndCityDetails
    {
        public string MakeName{ get; set; }
        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
    }
}
