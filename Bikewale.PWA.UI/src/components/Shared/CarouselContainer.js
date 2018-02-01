import React from 'react'
import LazyLoad from 'react-lazy-load'
import CarouselBikeItem from './CarouselBikeItem'
import CarouselVideoItem from './CarouselVideoItem'

class CarouselContainer extends React.Component {
	constructor(props) {
		super(props);
	}
	
    render() {
   
    	if(!this.props.carouselData || !this.props.carouselData.List || this.props.carouselData.List.length==0 || !this.props.childComponent) return null;
    	var carouselData = this.props.carouselData;
    	var ChildComponent = this.props.childComponent;
		
		return (
            <div className="container bg-white box-shadow section-bottom-margin carousel-bottom-padding carousel-top-padding">
                <div className="carousel-heading-content">
                    <div className="swiper-heading-left-grid inline-block">
                        <h2>{carouselData.Heading}</h2>
                    </div>
                    <div className="swiper-heading-right-grid inline-block text-right">
                        <a href={carouselData.CompleteListUrl} title={carouselData.CompleteListUrlAlternateLabel} className="btn view-all-target-btn">View all</a>
                    </div>
                </div>
                
                <ul className={"carousel-wrapper "+carouselData.wrapperCssClass}>
                {
                    carouselData.List.map(function(item) {
                        return <ChildComponent item={item}/>
                    }.bind(this))
                }
	            </ul>
            </div>
		)
    }
}

module.exports = CarouselContainer
