﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.CMS.Articles
{
    public class CMSArticleSummaryMin : CMSArticleBase
    {
        public ushort CategoryId { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgUrl { get; set; }
        public string AuthorName { get; set; }
        public DateTime DisplayDate { get; set; }
    }
}
