﻿
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pager;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class UpcomingPageVM : ModelBase
    {
        public BrandWidgetVM Brands { get; set; }
        public IEnumerable<UpcomingBikeEntity> UpcomingBikeModels { get; set; }
        public bool HasBikes { get; set; }
        public uint TotalBikes { get; set; }
        public IEnumerable<int> YearsList { get; set; }
        public IEnumerable<BikeMakeEntityBase> MakesList { get; set; }
        public PagerEntity Pager { get; set; }
        public bool HasPages { get { return (Pager != null && Pager.TotalResults > 0); } }
    }
}
