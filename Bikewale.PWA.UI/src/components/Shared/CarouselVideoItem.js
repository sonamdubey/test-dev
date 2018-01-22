import React from 'react'
import LazyLoad from 'react-lazy-load'
import {Link} from 'react-router-dom'
import {formVideoUrl , formVideoImageUrl} from '../Videos/VideosCommonFunc'
class CarouselVideoItem extends React.Component {
    constructor(props) {
        super(props);
        
    }
	onClickItem(item,event) {
		if(this.props.onClickItem && typeof(this.props.onClickItem) === 'function') {
            event.preventDefault();
        	this.props.onClickItem(item);
		}
	}
	
	render() {
		if(!this.props.item) return null;
        var item = this.props.item;
		return(
            <li key={item.BasicId} className="carousel-slide">
                <div className="carousel-card">
                    <Link to={formVideoUrl(item.VideoTitleUrl,item.BasicId)} title={item.VideoTitle} className="card-target-block" onClick={this.onClickItem.bind(this,item)}>
                        <div className="card-image-block">
                            <LazyLoad>
                                <img className="swiper-lazy" src={formVideoImageUrl(item.VideoId,'mqdefault.jpg')} alt={item.VideoTitle} />
                            </LazyLoad>
                            <span className="swiper-lazy-preloader"></span>
                            <span className="black-overlay">
                                <span className="bwmsprite video-play-icon"></span>
                            </span>
                        </div>
                        <div className="card-details-block">
                            <h3 className="font12 text-black margin-bottom5 text-truncate-2">{item.VideoTitle}</h3>
                            <div className="views-count-container">
                                <span className="bwmsprite calender-grey-sm-icon"></span>
                                <span className="article-stats-content">{item.DisplayDate}</span>
                            </div>
                            <div className="views-count-container">
                                <span className="bwmsprite video-views-icon"></span>
                                <span className="article-stats-content">{item.Views}</span>
                            </div>
                            <div className="clear"></div>
                        </div>
                    </Link>
                </div>
            </li>);
	}
}

module.exports = CarouselVideoItem;
