using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Pager;
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
        public IEnumerable<ModelImages> BikeModelsPhotos { get; set; }
        public int TotalBikeModels { get; set; }
        public OtherMakesVM OtherPopularMakes { get; set; }
        public IEnumerable<ModelImages> PopularSportsModelsImages { get; set; }
        public PagerEntity Pager { get; set; }
        public bool IsMobile { get; set; }
    }
}
