﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>
    [Serializable]
    public class ArticleBase
    {
        public ulong BasicId { get; set; }
        public string Title { get; set; }
        public string ArticleUrl { get; set; }
    }
}
