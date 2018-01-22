import React from 'react'

import {Status} from '../../utils/constants'
import LazyLoad from 'react-lazy-load'

import {isServer} from '../../utils/commonUtils'
class VideoModelSlug extends React.Component {
	constructor(props) {
		super(props);
		if(isServer()) {
			this.props.Status = Status.Fetched;
		}
		this.renderUsedBikesLink = this.renderUsedBikesLink.bind(this);
		
	}
	
	renderUsedBikesLink(model,usedBikesLink) {

		if(!usedBikesLink) return null;

		return (
			<div>
				<div className="border-solid-bottom margin-top5 margin-bottom10"></div>
				<a href={usedBikesLink.UsedBikesLinkUrl} title={"Used " + model.ModelName} className="block text-default">
	                <span className="used-target-label inline-block">
	                    <span className="font14 text-bold">{usedBikesLink.DescriptionLabel}</span><br />
	                    <span className="font12 text-light-grey">{usedBikesLink.PricePrefix} <span>&#x20B9;</span> {usedBikesLink.Price}</span>
	                </span>
	                <span className="bwmsprite next-grey-icon"></span>
	            </a>
	        </div>
	        )

	}

	getClassNameForBikeInfo(type) {
		switch(type) {
			case 'Expert Reviews' : return 'reviews-sm';
			case 'Images' : return 'photos-sm';
			case 'Specs' : return 'specs-sm';
			case 'News' : return 'news-sm';
			case 'Videos' : return 'videos-sm';
			case 'Dealers' : return 'dealers-sm';
			case 'User Reviews' : return 'user-reviews-sm';
		}
		return '';
	}
	renderMoreDetailsListItem(item) {
		if(!item)
			return false;
		var className = this.getClassNameForBikeInfo(item.Type);
		if(className == '')
			return false;
		return(
				<li key={item.Type}>
                    <a href={item.DetailUrl} title={item.Title}>
                        <span className={"bwmsprite "+className}></span>
                        <span className="icon-label">{item.Type}</span>
                    </a>
                </li>
			)
	}
	renderMoreDetailsList(moreDetailsList) {
		if(!moreDetailsList || (!(moreDetailsList.length > 0)) )
			return false;
		return (
			<ul className="item-more-details-list">
				{
					moreDetailsList.map(function(item) {
						return this.renderMoreDetailsListItem(item);
					}.bind(this))
				}
			</ul>
		)



	}
	render() {

		
		var model = this.props.modelInfo;
		if(!model) return null;

		var ribbonTagComponent = null;

		if(model.Upcoming.toLowerCase() == "true")
   			ribbonTagComponent = <p className="model-ribbon-tag upcoming-ribbon">Upcoming</p>
    	else if(model.Discontinued.toLowerCase() == "true")
    		ribbonTagComponent = <p className="model-ribbon-tag discontinued-ribbon">Discontinued</p>

    	var ratingComponent = null;
    	if(model.Rating && model.Rating.Count > 0) {
    		ratingComponent = (
	    			<div id="reviewRatingsDiv" className="margin-bottom10 position-rel position--left-neg-5">
			            <span className={"rate-count-" + Math.round(model.Rating.Rating)}>
			                <span className="bwmsprite star-icon star-size-16"></span>
			                <span className="font14 text-bold inline-block">{model.Rating.Rating}</span>
			            </span>
			            <span className='font11 text-xx-light inline-block padding-left5'>
			            	({model.Rating.Count + ' ' + (model.Rating.Count > 1  ? 'ratings' : 'rating')})
			            </span>
			            {model.Rating.ReviewCount > 0 ?
			            	<a className='text-xt-light review-left-divider inline-block font13' href={model.Rating.ReviewUrl} title={model.ModelName + " user reviews"}>
			            		{model.Rating.ReviewCount + " " + (model.Rating.ReviewCount > 1 ? "reviews" : "review")}
			            	</a>
			         	    : null}
			        </div>
    			)

    	}

		return (
			<div className="bg-white box-shadow section-bottom-margin padding-15-20">
			    <div className="model-more-info-section">
			        {ribbonTagComponent}
			        <div className="clear"></div>
			        <a href={model.ModelDetailUrl} className="block text-default margin-bottom5" title={model.ModelName}>
			            <h2>{model.ModelName}</h2>
			        </a>
			        {ratingComponent}
			        <div className="misc-container margin-bottom10">
			            <div className="clear"></div>

			            <a href={model.ModelDetailUrl} className="misc-list-image vertical-top" title={model.ModelName}>
							<div className="misc-container__image">
				            	<LazyLoad>
					                <img src={model.ImageUrl} alt={model.ModelName}/>
				                </LazyLoad>
							</div>
			            </a>
			            <div className="misc-details-block">
			                <p className={"price-label-size-12"+ ((model.Upcoming.toLowerCase() == "true") ? "" : " text-truncate")}>{model.PriceDescription}</p>
		                    <div>
		                        <span className='price-value-size-18'>&#x20B9;</span>&nbsp;<span className="price-value-size-18 version-price">{model.Price}</span>
		                    </div>

			            </div>
			        </div>
			        <a href={model.ModelDetailUrl} title={model.ModelName} className="btn btn-white btn-180-34 margin-bottom15">View model details <span className="bwmsprite btn-red-arrow"></span></a>
			        {this.renderMoreDetailsList()}

	                <div className="clear"></div>

	                {this.renderUsedBikesLink(model,model.UsedBikesLink)}


			    </div>
			</div> )
	}
}
module.exports = VideoModelSlug;