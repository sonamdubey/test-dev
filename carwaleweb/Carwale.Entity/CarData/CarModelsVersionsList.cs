using Carwale.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    public class CarModelsVersionsList : MakeModelEntity
    {
        public string DisplayName { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }
        public List<IdName> Versions { get; set; }
    }
}
