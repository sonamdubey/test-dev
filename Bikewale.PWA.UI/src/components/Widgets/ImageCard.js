import React from 'react'
import { createImageUrl } from './WidgetsCommon'
import LazyLoad from 'react-lazy-load'

class ImageCard extends React.Component {
	constructor(props) {
		super(props);
	}

	render() {
        
	    var title = this.props.bike.BikeName +" Images";
	    var imagesCount = this.props.bike.ModelImages.length;
	    var gridSize = imagesCount>=7? 7 : imagesCount>=3? 3 : 1;
	    var imagesList = this.props.bike.ModelImages.slice(0, gridSize);
	    return (
			<div className="model-image__card">
				<ul className="image-grid__list" data-grid={gridSize.toString()}>
                    {imagesList.map(function(bikeImage) { 
                        return (
                            <li className="image-grid-list__item">
						        <LazyLoad>
							        <img className="swiper-lazy" src={createImageUrl(bikeImage.HostUrl, bikeImage.OriginalImgPath)} alt={title} title={title} />
						        </LazyLoad>
					        </li>);
                    })}
				</ul>
				<div className="card-image__details">
					<h3 className="card-details__left-col">
						<span className="card-details__make">{this.props.bike.MakeName}</span>
						<span className="card-details__model">{this.props.bike.ModelName}</span>
					</h3>
					<div className="card-details__right-col">
						<span className="card-details__image-count">{this.props.bike.RecordCount}</span>
					</div>
				</div>
			</div>
		)
	}
}

module.exports = ImageCard
