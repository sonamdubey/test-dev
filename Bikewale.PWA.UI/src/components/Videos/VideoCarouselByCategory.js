import React from 'react'
import LazyLoad from 'react-lazy-load'
import {formVideoUrl ,formVideoImageUrl, formMoreVideosUrl} from './VideosCommonFunc'
import {Link} from 'react-router-dom'
class VideoCarouselByCategory extends React.Component {
	onClickMoreVideoUrl(event) {
		if(this.props.onClickMoreVideoUrl && typeof(this.props.onClickMoreVideoUrl) === 'function') {
			event.preventDefault();
			this.props.onClickMoreVideoUrl(this.props.VideoListData.SectionTitle,this.props.VideoListData.MoreVideosUrl);
		}
	}
	onClickVideoUrl(video,event){


		if(this.props.onClickVideoUrl && typeof(this.props.onClickVideoUrl) === 'function') {
			event.preventDefault();
			this.props.onClickVideoUrl(video);
		}
	}
	renderVideoItem(video) {
		return (
			<div className="carousel-slide">
				<div className="carousel-card">
	                <Link to={formVideoUrl(video.VideoTitleUrl,video.BasicId)} className="card-target-block" onClick={this.onClickVideoUrl.bind(this,video)}>
			            <div className="card-image-block">
			                    <LazyLoad>
			                    	<img className="swiper-lazy" src={formVideoImageUrl(video.VideoId,'default.jpg')} alt={video.VideoTitle} title={video.VideoTitle} border="0" />
			                    </LazyLoad>
			            </div>
						<span className="swiper-lazy-preloader"></span>

			            <div className="card-details-block">
		                	<div className="card-list-title text-truncate-2">{video.VideoTitle}</div>
			                <p className="card-list__info">{video.DisplayDate}</p>
			                <div className="views-count-container">
			                    <span className="bwmsprite video-views-icon"></span><span className="text-default">{video.Views}</span>
			                </div>
			                <div className="views-count-container padding-left10 border-light-left">
			                    <span className="bwmsprite video-likes-icon"></span><span className="text-default">{video.Likes}</span>
			                </div>
			                <div className="clear"></div>
						</div>
					</Link>
	            </div>
			</div>
		)
	}
	render(){
		if( !this.props.VideoListData || !this.props.VideoListData.Videos || !(this.props.VideoListData.Videos.length>0) )
			return null;

		return (
			<section>
			    <div className="container">
					<div className="carousel-heading-content">
						<h2 className="carousel-heading__title-center">{this.props.VideoListData.SectionTitle}</h2>
					</div>
		            <div className="carousel-wrapper carousel-type-video">
		            	{this.props.VideoListData.Videos.map(function(video){
		            		return this.renderVideoItem(video);
		            	}.bind(this))}
		            </div>
			        <div className="padding-left10 view-all-btn-container margin-top10 padding-bottom20">
			            <Link to={formMoreVideosUrl(this.props.VideoListData.MoreVideosUrl)} title={this.props.VideoListData.SectionTitle+" Bike Videos"} className="btn view-all-target-btn" onClick={this.onClickMoreVideoUrl.bind(this)}>View more videos<span className="bwmsprite teal-right"></span></Link>
			        </div>
			    </div>
			</section>
		)
	}
}

module.exports = VideoCarouselByCategory;
