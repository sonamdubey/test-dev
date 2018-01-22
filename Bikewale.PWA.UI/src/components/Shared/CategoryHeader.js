import React from 'react'

class CategoryHeader extends React.Component{
	onBackButtonClick() {
		window.history.back();
	}
	render() {
		if(!this.props.PageHeading)
			return null;
		return (
			<header id="categoryHeader" className="fixed-header">
		        <div className="category-back-btn">
		            <span className="bwmsprite fa-arrow-back" onClick={this.onBackButtonClick.bind()}></span>
		        </div>
		        <h1 className="header-title">{this.props.PageHeading}</h1>
		    </header>
			)
	}
}

module.exports = CategoryHeader;
