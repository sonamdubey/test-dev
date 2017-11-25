﻿using System.Collections.Generic;

namespace Bikewale.Entity.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>

    public class ArticlePageDetails : ArticleSummary
    {
        public List<Page> PageList { get; set; }
        public List<string> TagsList { get; set; }
        public List<VehicleTag> VehiclTagsList { get; set; }

        public ArticleBase NextArticle { get; set; }
        public ArticleBase PrevArticle { get; set; }

        public string MainImgCaption { get; set; }
        public bool IsMainImageSet { get; set; }
    }
}
