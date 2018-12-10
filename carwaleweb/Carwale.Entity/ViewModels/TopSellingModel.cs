using Carwale.Entity.CarData;
using Carwale.Entity.Classified.PopularUsedCars;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CompareCars;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Carwale.Entity.ViewModels
{
    public class TopSellingModel
    {
        public List<TopSellingCarModel> TopSelling;
        public List<Sponsored_Car> SponsoredPopularCars;
    }
}