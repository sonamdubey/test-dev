﻿namespace Bikewale.Entities.PWA.Articles
{
    public class PwaReduxStore
    {
        public PwaNewsReducer NewsReducer { get; private set; }

        public PwaReduxStore()
        {
            NewsReducer = new PwaNewsReducer();
        }

    }

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

    public class PwaArticleListData
    {
        public PwaArticleListData()
        {
            ArticleList = new PwaContentBase();
        }
        public PwaContentBase ArticleList { get; private set; }
    }


    public class PwaNewBikesListData
    {
    //    List<BikeInfo> NewBikesList { get; set; }
    }

    public class PwaNewsDetailReducer
    {
        public PwaArticleDetailData ArticleDetailData { get; set; }
        public PwaNewBikesListData NewBikesListData { get; set; }
        public PwaRelatedModelObject RelatedModelObject { get; set; }
    }

    public class PwaArticleDetailData
    {
    //    public PwaArticleDetail ArticleDetail { get; set; }
    }

    public class PwaRelatedModelObject
    {
     //   public PwaModelObject ModelObject { get; set; }
    } 
}