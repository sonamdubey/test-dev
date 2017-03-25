using Bikewale.Entities.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.ExpertReviews
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Mar 2017
    /// Summary    : View model for expert reviews widget
    /// </summary>
   public class RecentExpertReviewsVM
   {
       public IEnumerable<ArticleSummary> ArticlesList { get; set; }
       public string MakeName { get; set; }
       public string ModelName { get; set; }
       public string MakeMasking { get; set; }
       public string ModelMasking { get; set; }
       public string LinkTitle { get; set; }
       public string MoreExpertReviewUrl { get; set; }
       public string BikeName { get; set; }
   }
}
