using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CentralizedCacheRefresh
{
    public class VersionAttribute
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MaskingName { get; set; }
        public string ModelId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string CityId { get; set; }
    }
}
