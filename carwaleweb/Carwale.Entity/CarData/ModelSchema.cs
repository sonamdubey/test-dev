using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    public class ModelSchema
    {
        public CarModelDetails ModelDetails { get; set; }
        public List<ModelColors> ModelColors { get; set; }
        public PageMetaTags PageMetaTags { get; set; }
        public List<MileageDataEntity> MileageData { get; set; }
        public List<string> SeatingCapacity { get; set; }
        public List<string> Drivetrain { get; set; }
    }
}
