import React from 'react'
import { isServer } from '../../utils/commonUtils'
import ImageCard from './ImageCard'
import Footer from '../Shared/Footer'
import Breadcrumb from '../Shared/Breadcrumb'
import { moreImagesUrl } from './WidgetsCommon'
import { Status } from '../../utils/constants'
import {Link} from 'react-router-dom'

class BikeImageCarouselComponent extends React.Component {

    constructor(props) {
		super(props);

        this.Status = this.props.BikeImagesListData? this.props.BikeImagesListData.Status: null;
        this.BikeImagesList = this.props.BikeImagesListData? this.props.BikeImagesListData.BikeImagesList: null;
        this.renderImageCardList = this.renderImageCardList.bind(this);
        this.callParentLogAndScrollHandler = this.callParentLogAndScrollHandler.bind(this);
        if(isServer()) {
            if(this.BikeImagesList) {
                this.Status = Status.Fetched;
            }
        }
    }

    callParentLogAndScrollHandler() {
        if(this.Status == Status.Fetched) {
            if(typeof this.props.logAndScrollHandler == 'function') {
                this.props.logAndScrollHandler('BikeImageCarouselComponent'); 
            }
        }
    }

    componentWillUpdate(nextProps, nextState) {
        this.Status = nextProps.BikeImagesListData? nextProps.BikeImagesListData.Status: null;
        this.BikeImagesList = nextProps.BikeImagesListData? nextProps.BikeImagesListData.BikeImagesList: null;
    }

    componentDidUpdate(prevProps, prevState) {
        this.callParentLogAndScrollHandler();
    }

    componentDidMount() {
        this.callParentLogAndScrollHandler();
        if(this.Status !== Status.Fetched && this.Status != Status.IsFetching) {
            this.props.fetchBikeImagesList(...this.props.FetchArgs);
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
		if( this.Status !== Status.Fetched || !this.BikeImagesList || this.BikeImagesList.length === 0 ) {
	        return null;
	    }
	    return (
            <div>
			    <section>
				    <div className="container margin-bottom15">
					    <h2 className="image-carousel__title">Bike Images</h2>
					    <div className="carousel-wrapper carousel__recently-added">
						    {this.renderImageCardList(this.BikeImagesList)}
					    </div>
                        <div className="padding-left10 view-all-btn-container margin-top10 padding-bottom20">
			                <a href={moreImagesUrl()} title="Bike Images" className="btn view-all-target-btn">View more images<span className="bwmsprite teal-right"></span></a>
			            </div>
				    </div>
			    </section>
                <Breadcrumb breadcrumb={[{Href : '/m/',Title : 'Home'},{Href : '',Title : 'Videos'}]}/>
				<Footer />
            </div>
		)
	}
}

module.exports = BikeImageCarouselComponent
