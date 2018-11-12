import React from 'react'
import {Status} from '../../utils/constants'
import LazyLoad from 'react-lazy-load'
class ModelSlug extends React.Component {
	propTypes : {
		modelInfo : React.PropTypes.object
	};
	renderUsedBikesLink() {
		if(!this.props.modelInfo || !this.props.modelInfo.ModelObject || !this.props.modelInfo.ModelObject.UsedBikesLink) {
			return false;
		}
		var usedBikesLink = this.props.modelInfo.ModelObject.UsedBikesLink;
		return (
			<div>
				<div className="border-solid-bottom margin-top5 margin-bottom10"></div>
				<a href={usedBikesLink.UsedBikesLinkUrl} title="Used" className="block text-default">
	                <span className="used-target-label inline-block">
	                    <span className="font14 text-bold">{usedBikesLink.DescriptionLabel}</span><br />
	                    <span className="font12 text-light-grey">{usedBikesLink.PricePrefix} <span>&#x20B9;</span> {usedBikesLink.Price}</span>
	                </span>
	                <span className="bwmsprite next-grey-icon"></span>
	            </a>
	        </div>
	        )
		
	}
	renderSeriesBikesLink() {
	    if(!this.props.modelInfo || !this.props.modelInfo.ModelObject || !this.props.modelInfo.ModelObject.Series) {
	        return false;
	    }
	    var series = this.props.modelInfo.ModelObject.Series;
	    return (
			<div>
				<div className="border-solid-bottom margin-top5 margin-bottom10"></div>
				<a href={series.SeriesLinkUrl} title={series.SeriesLinkTitle} className="block text-default">
	                <span className="used-target-label inline-block">
	                    <span className="font14 text-bold">{series.DescriptionLabel}</span><br />
	                    <span className="font12 text-light-grey">{series.PricePrefix} <span>&#x20B9;</span> {series.Price}</span>
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
	renderMoreDetailsList() {
		if(!this.props.modelInfo || !this.props.modelInfo.ModelObject || !this.props.modelInfo.ModelObject.MoreDetailsList) {
			return false;
		}

		var moreDetailsList = this.props.modelInfo.ModelObject.MoreDetailsList;
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
		if(!this.props.modelInfo || this.props.modelInfo.Status !== Status.Fetched || !this.props.modelInfo.ModelObject) {
			return false;
		}
		var modelObject = this.props.modelInfo.ModelObject;
		if(!modelObject) {
			return false;
		}
		var ribbonTagComponent = null;
		if(modelObject.Upcoming.toLowerCase() == "true") 
   			ribbonTagComponent = <p className="model-ribbon-tag upcoming-ribbon">Upcoming</p>
    	else if(modelObject.Discontinued.toLowerCase() == "true")
    		ribbonTagComponent = <p className="model-ribbon-tag discontinued-ribbon">Discontinued</p>

    	return (
    		
            <div className="model-more-info-section model-slug-type-news">
                <div className="margin-bottom10">
                	{ribbonTagComponent}
                    <div className="clear"></div>
                    <a href={modelObject.ModelDetailUrl} className="item-image-content vertical-top" title={modelObject.ModelName}>
                    	    <LazyLoad offsetVertical={0}>
		                   	    <img  src={modelObject.ImageUrl} alt={modelObject.ModelName} />
		                   	</LazyLoad>
	                </a>
                    <div className="bike-details-block vertical-top">
                        <a href={modelObject.ModelDetailUrl} className="block margin-bottom5" title={modelObject.ModelName}>
                            <h3 className="text-truncate">{modelObject.ModelName}</h3>
                        </a>
                        <p className="key-size-12 text-truncate">{modelObject.PriceDescription}</p>
                        <div className="value-size-18">
                            <span>&#x20B9;</span>
                            <span> {modelObject.Price}</span>
                        </div>
                    </div>
                </div>

                {this.renderMoreDetailsList()}
                    
                <div className="clear"></div>

                {this.renderSeriesBikesLink()}
            </div>
        )
	}
}

module.exports = ModelSlug