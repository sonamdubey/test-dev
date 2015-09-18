using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary : Class to hold the properties for bike versions list on the models page.
    /// </summary>
    public class BikeVersionsListEntity
    {
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public string ModelName { get; set; }
        public UInt64 Price { get; set; }
      //  public string MaskingName { get; set; }

    }
}
