import React from 'react'
import {Link} from 'react-router-dom'
import {isServer , CMSUserReviewSlugPosition , isCMSUserReviewSlugClosed} from '../../utils/commonUtils'
import { mapNewsArticleDataToInitialData } from './NewsCommon'
import LazyLoad from 'react-lazy-load'
import UserReviewSlug from './UserReviewSlug'

class ArticleList extends React.Component {
    propTypes : {
        'articleList' : React.PropTypes.array
    };
    onArticleClickEvent (article,event) {
        event.preventDefault();
        var articleInitialData = mapNewsArticleDataToInitialData(article);
        
        this.props.onArticleClickEvent(articleInitialData);
    }
    renderArticleContent(article,index) {
        var imageUrl = (!article.HostUrl || !article.SmallPicUrl) ? 'https://imgd.aeplcdn.com/160x89/bikewaleimg/images/noimage.png?q=70' : article.HostUrl + article.SmallPicUrl;

        return (
                <div className="article-item-content">
                    <div className="article-wrapper">
                        <div className="article-image-wrapper">
                            <LazyLoad offsetVertical={0}>
                                <img src={imageUrl} alt={article.Title} title={article.Title} />
                            </LazyLoad>
                        </div>
                        <div className="article-desc-wrapper">
                            <span className="article-category">{article.CategoryName}</span>
                            <h2 className="font14">{article.Title}</h2>
                        </div>
                    </div>
                    <div className="article-stats-wrapper font12 leftfloat text-light-grey">
                        <span className="bwmsprite calender-grey-icon"></span>
                        <span className="inline-block">{article.DisplayDate}</span>
                    </div>
                    <div className="article-stats-wrapper font12 leftfloat text-light-grey">
                        <span className="bwmsprite author-grey-icon"></span>
                        <span className="inline-block">{article.AuthorName}</span>
                    </div>
                    <div className="clear"></div>
                </div>
            )
    }
    renderArticleLinkTag(article,index) {
        
        if(article.CategoryName == 'NEWS') {
            return (
                <li key={article.BasicId}>
                    <Link to={article.ArticleUrl} title={article.Title} onClick={this.onArticleClickEvent.bind(this,article)}>
                        {this.renderArticleContent(article,index)}
                    </Link>
                </li>
            )
        }
        else {
            return (
                <li key={article.BasicId}>
                    <a href={article.ArticleUrl} title={article.Title}>
                        {this.renderArticleContent(article,index)}
                    </a>
                </li>
            )
        }
    }
    render() {
        var articleComponents = [];
        for(var i = 0 ; i<this.props.articleList.length ; i++){
            if(i == CMSUserReviewSlugPosition && this.props.pageNo == 1) {
                if(isServer() || !isCMSUserReviewSlugClosed()) {
                    articleComponents.push(<UserReviewSlug/>);        
                }                
            }
            articleComponents.push(this.renderArticleLinkTag(this.props.articleList[i],i));
        } 
        return (
            <ul className="article-list">
               {articleComponents}
            </ul>
        )

    }
}

export default ArticleList
