using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Created By  : Rajan Chauhan on 09 Jan 2017
/// Description : Added OtherPopularMakes
/// </summary>
namespace Bikewale.Models.Photos.v1
{
    public class PhotosPageVM : ModelBase
    {
        public OtherMakesVM OtherPopularMakes { get; set; }
    }
}
