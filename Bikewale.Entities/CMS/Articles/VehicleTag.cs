using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Entities.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>    
    public class VehicleTag
    {
        public BikeMakeEntityBase MakeBase { get; set; }
        public BikeModelEntityBase ModelBase { get; set; }
        public BikeVersionEntityBase VersionBase { get; set; }
    }
}
