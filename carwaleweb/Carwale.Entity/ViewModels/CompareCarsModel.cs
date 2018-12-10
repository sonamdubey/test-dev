using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Common;
using System.Collections.Generic;

namespace Carwale.Entity.ViewModels
{
    public class CompareCarsModel
    {
        public List<CarMakeEntityBase> Makes { get; set; }
        public List<HotCarComparison> HotComparisons { get; set; }
        public List<CarVersionDetails> VersionsDetails { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public ExperimentAdSlot ExperimentAdSlot { get; set; }
    }
}
