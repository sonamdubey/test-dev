using System.Collections.Generic;
using Bikewale.Entities.CMS.Photos;

namespace Bikewale.Models.Images
{
    /// <summary>
    /// Created by  :   Pratibha Verma on 8 Feb 2018
    /// Description :   ImageWidget VM
    /// </summary>
    public class ImageWidgetVM
    {
       public IEnumerable<ModelImages> ModelList { get; set; }
    }
}
