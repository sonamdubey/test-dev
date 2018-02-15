import React from 'react'
import PropTypes from 'prop-types'
import { isServer } from '../../utils/commonUtils'
import ImageCard from './ImageCard'
import { moreImagesUrl } from './WidgetsCommon'
import { Status } from '../../utils/constants'
import {Link} from 'react-router-dom'

class BikeImageCarouselComponent extends React.Component {

    constructor(props) {
		super(props);

        this.Status = this.props.BikeImagesListData? this.props.BikeImagesListData.Status: null;
        this.renderImageCardList = this.renderImageCardList.bind(this);
        if(isServer()) {
            if(this.BikeImagesList) {
                this.Status = Status.Fetched;
            }
        }
    }

    componentDidMount() {
        if(this.Status !== Status.Fetched && this.Status != Status.IsFetching) {
            this.props.fetchBikeImagesList(...this.props.FetchArgs);
        }
  
    }

	callParentLogAndScrollHandler() {
	    if(this.Status == Status.Fetched) {
	        if(typeof this.props.logAndScrollHandler == 'function') {
	            this.props.logAndScrollHandler('ImageCarouselComponent'); 
	        }
	    }
	}

	renderImageCardList(bikesList) {
	    var list = null;
	    if(bikesList && bikesList.length > 0) {
	        list = bikesList.map(function(bike) {
	            return (<div className="carousel-slide">
                    <a href={bike.ModelImagePageUrl} title={bike.BikeName + " Images"} className="block">
                        <ImageCard bike={bike}/>
                    </a>
                </div>);
		    });
	    }
		return list;
	}

	render() {
	    var bikesList = this.props.BikeImagesListData? this.props.BikeImagesListData.BikeImagesList: null;
		return (
			<section>
				<div className="container margin-bottom15">
					<h2 className="image-carousel__title">Images</h2>
					<div className="carousel-wrapper carousel__recently-added">
						{this.renderImageCardList(bikesList)}
					</div>
                    <div className="padding-left10 view-all-btn-container margin-top10 padding-bottom20">
			            <a href={moreImagesUrl()} title="Bike Images" className="btn view-all-target-btn">View more images<span className="bwmsprite teal-right"></span></a>
			        </div>
				</div>
			</section>
		)
	}
}

module.exports = BikeImageCarouselComponent
