import React from 'react'
import SpinnerRelative from '../Shared/SpinnerRelative'

class ArticleDetailContent extends React.Component {
	propTypes: {
		htmlContent : React.PropTypes.string
	};
	
	render() {
		if(this.props.htmlContent) {
			return (
					<div dangerouslySetInnerHTML={{__html:this.props.htmlContent}}/>
				)
		}
		else 
			return <SpinnerRelative/>;
		
		return false;

	}
}

module.exports = ArticleDetailContent

/*<div className="article-content">
					<img alt={this.props.Title} title={this.props.title} src={this.props.hostUrl+this.props.largePicUrl}/>
					<div dangerouslySetInnerHTML={{__html:this.props.htmlContent}}/>
				</div>*/