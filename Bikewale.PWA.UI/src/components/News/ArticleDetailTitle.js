import React from 'react'

class ArticleDetailTitle extends React.Component {
	propTypes : {
		title : React.PropTypes.string,
		displaydate : React.PropTypes.string,
		authorName : React.PropTypes.string
	};
	render() {
      var authorLink = "";
		if(this.props.authorMaskingName)
		{
			authorLink = <a href={'/m/authors/' + this.props.authorMaskingName + '/'} > 
							<span className="bwmsprite author-grey-sm-icon"></span>
							<span className="article-stats-content margin-left5 text-link">{this.props.authorName}</span>
						</a>;
		}
		else
		{
		 authorLink = <div>
					<span className="bwmsprite author-grey-sm-icon"></span>
                    <span className="article-stats-content">{this.props.authorName}</span></div>;
		}
		return  (
			<div className="box-shadow article-head card-heading">
                <h1 className="margin-bottom10">{this.props.title}</h1>
                <div className="grid-6 alpha">
                    <span className="bwmsprite calender-grey-sm-icon"></span>
                    <span className="article-stats-content">{this.props.displayDate}</span>
                </div>
               <div className="grid-6 alpha padding-right5">
			   {authorLink}
			   </div>
                <div className="clear"></div>
            </div>
		)
	}
}


module.exports = ArticleDetailTitle;