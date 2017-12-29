import React from 'react'

class ModelBrandsList extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
			showOtherBrands : false
		};
		this.renderBrandItem = this.renderBrandItem.bind(this);
		this.renderTopBrands = this.renderTopBrands.bind(this);
		this.renderOtherBrands = this.renderOtherBrands.bind(this);
		this.renderShowMoreBrandsButton = this.renderShowMoreBrandsButton.bind(this);
		this.moreBrandButtonClick = this.moreBrandButtonClick.bind(this);
	}
	moreBrandButtonClick() {
		this.setState({
			showOtherBrands : !this.state.showOtherBrands
		})
	}
	renderBrandItem(brand) {
		if(!brand)
			return null;
		return (<li>
                    <a href={'/m'+brand.Href} title={brand.Title}>
                        <span className="brand-type">
                            <span className={"lazy brandlogosprite brand-"+brand.MakeId}></span>
                        </span>
                        <span className="brand-type-title">{brand.MakeName}</span>
                    </a>
                </li>)
	}
	renderTopBrands(topBrands) {
		if(!topBrands)
			return null;
		return (
			<ul className="text-center">
				{topBrands.map(function(brand){
					return this.renderBrandItem(brand);
				}.bind(this))}
		    </ul>
		)
	}
	renderOtherBrands(otherBrands) {
		if(!otherBrands || !(otherBrands.length>0))
			return null;
		var hideClassName = this.state.showOtherBrands ? '' : 'hide';
		return(
			<ul className={"brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center "+hideClassName}>
				{otherBrands.map(function(brand){
					return this.renderBrandItem(brand);
				}.bind(this))}
			</ul>
		)
	}
	renderShowMoreBrandsButton(otherBrands) {
		if(!otherBrands || otherBrands.length==0) {
			return null;
		}

		return (
			<div className="view-all-btn-container">
		        <a href="javascript:void(0)" onClick={this.moreBrandButtonClick.bind()} className="view-brandType btn view-all-target-btn rotate-arrow" rel="nofollow"><span className="btn-label">{this.state.showOtherBrands?'View less brands':'View more brands'}</span><span className="bwmsprite teal-right"></span></a>
		    </div>
		)
	}
	render(){
		if(!this.props.Brands || !this.props.Brands.TopBrands || !(this.props.Brands.TopBrands.length > 0))
			return null;
		return (
			<div>
				<h2 className="text-center padding-top10 padding-bottom15">Browse videos by brands</h2>
				<div className="container bg-white box-shadow card-bottom-margin brand-container collapsible-brand-content">
				    <div id="brand-type-container" className="brand-type-container">
				    	{this.renderTopBrands(this.props.Brands.TopBrands)}
				    	{this.renderOtherBrands(this.props.Brands.OtherBrands)}
				    </div>
				    {this.renderShowMoreBrandsButton(this.props.Brands.OtherBrands)}

				</div>
			</div>
		)
		
	}
}

module.exports = ModelBrandsList;