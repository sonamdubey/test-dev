using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.CarData;

namespace Carwale.Entity.CMS.Articles
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 27 Aug 2014
    /// </summary>
    [Serializable]
    public class VehicleTag
    {
        public CarMakeEntityBase MakeBase { get; set; }
        public CarModelEntityBase ModelBase { get; set; }
        public CarVersionEntity VersionBase { get; set; }
    }
}
