using System.Collections.Generic;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Created By: Prasad Gawde
    /// </summary>
    public class PwaNewsReducer
    {
        public PwaNewsArticleListReducer NewsArticleListReducer { get; private set; }
        public PwaNewsDetailReducer NewsDetailReducer { get; private set; }

        public PwaNewsReducer()
        {
            NewsArticleListReducer = new PwaNewsArticleListReducer();
            NewsDetailReducer = new PwaNewsDetailReducer();
        }
    }

    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    public class PwaNewsArticleListReducer
    {
        public PwaArticleListData ArticleListData { get; private set; }
        public PwaNewBikesListData NewBikesListData { get; private set; }

        public PwaNewsArticleListReducer()
        {
            ArticleListData = new PwaArticleListData();
            NewBikesListData = new PwaNewBikesListData();
        }

    }

    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    public class PwaArticleListData
    {       
        public PwaContentBase ArticleList { get; set; }
    }

    /// <summary>
    ///  Created By: Prasad Gawde
    ///  Modified by : Ashutosh Sharma on 23 Feb 2018
    ///  Description : Added BikeMakeList.
    /// </summary>
    public class PwaNewBikesListData
    {
        public List<PwaBikeNews> NewBikesList { get; set; }
        public IEnumerable<PwaBikeMakeEntity> BikeMakeList { get; set; }
    }

    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    public class PwaNewsDetailReducer
    {
        public PwaArticleDetailData ArticleDetailData { get; set; }
        public PwaNewBikesListData NewBikesListData { get; set; }
        public PwaRelatedModelObject RelatedModelObject { get; set; }

        public PwaNewsDetailReducer()
        {
            ArticleDetailData = new PwaArticleDetailData();
            NewBikesListData = new PwaNewBikesListData();
            RelatedModelObject = new PwaRelatedModelObject();
        }
    }

    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    public class PwaArticleDetailData
    {
        public PwaArticleDetails ArticleDetail { get; set; }
    }

    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    public class PwaRelatedModelObject
    {
        public PwaBikeInfo ModelObject { get; set; }
    }
}