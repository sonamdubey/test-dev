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
