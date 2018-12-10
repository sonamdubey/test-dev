using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CentralizedCacheRefresh
{
    public class ModelRootAttribute
    {
        public string Id { get; set; }
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public string Year { get; set; }
        public string Ids { get; set; }
    }
}
