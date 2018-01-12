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
    /// <summary>
    /// Created by : Ashutosh Sharma on 11th Jan 2018
    /// </summary>
    public class PhotosPageVM : ModelBase
    {
        public OtherMakesVM OtherPopularMakes { get; set; }
    }
}
