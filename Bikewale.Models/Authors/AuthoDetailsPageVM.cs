using Bikewale.Entities.Authors;
using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Models.Authors
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 20th Sep 2017
    /// Summary : VM for author details page
    /// </summary>
    public class AuthoDetailsPageVM: ModelBase
    {
        public AuthorEntity Author { get; set; }
        public IEnumerable<ArticleSummary> ExpertReviewsList { get; set; }
        public IEnumerable<ArticleSummary> NewsList { get; set; }
        public IEnumerable<AuthorEntityBase> OtherAuthors { get; set; } 
    }
}
