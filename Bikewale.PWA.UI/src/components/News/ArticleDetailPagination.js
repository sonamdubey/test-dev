import React from 'react'
import {Link} from 'react-router-dom'
import {mapNewsArticleDataToInitialData} from './NewsCommon'
class ArticleDetailPagination extends React.Component {
	propTypes: {
		prevArticle : React.PropTypes.object,
		nextArticle : React.PropTypes.object
	};
	onArticlePaginationClickEvent(article,event) {
		event.preventDefault();
		
		var articleInitialData = mapNewsArticleDataToInitialData(article);
        this.props.onArticlePaginationClickEvent(articleInitialData);
	}
	renderLinkContent(article,articleTypeText) {
		if(articleTypeText == "Previous") {
			return (
				<div>
					<span className="bwmsprite prev-arrow"></span>
		            <div className="next-prev-article-box inline-block padding-left5">
		                <span className="font12 text-light">Previous</span><br />
		                <span className="next-prev-article-title next-page-title">{article.Title}</span>
		            </div>
	        	</div>
			)
		}
		else if(articleTypeText == "Next") {
			return (
				<div>	
					<div className="next-prev-article-box inline-block padding-right5">
                        <span className="font12 text-light">Next</span>
                        <span className="next-prev-article-title next-page-title">{article.Title}</span>
                    </div>
                    <span className="bwmsprite next-arrow"></span>
				</div>
			)
		}
	}
	renderLinkForAdjacentArticle (article,articleTypeText) {
		if(!article) {
			return false;
		}
		if(article.CategoryName == "NEWS") {
			return (
				<Link to={article.ArticleUrl} title={article.Title} className="text-default next-prev-article-target" onClick={this.onArticlePaginationClickEvent.bind(this,article)}>
                    {this.renderLinkContent(article,articleTypeText)}    
                </Link>
			)
		}
		else {
			return (
				<a href={article.ArticleUrl} title={article.Title} className="text-default next-prev-article-target">
                    {this.renderLinkContent(article,articleTypeText)}    
                </a>
			)
		}
	}
	render() {
		var prevArticle = this.props.prevArticle;
		var nextArticle = this.props.nextArticle;
	
		return (
			<div className="border-solid-top padding-top10">
                <div className="grid-6 alpha border-solid-right">
                	{this.renderLinkForAdjacentArticle(prevArticle,"Previous")}
                </div>
                <div className="grid-6 omega rightfloat">
                	{this.renderLinkForAdjacentArticle(nextArticle,"Next")}
                </div>
                <div className="clear"></div>
            </div>
        )
        
	} 

}

module.exports = ArticleDetailPagination;

