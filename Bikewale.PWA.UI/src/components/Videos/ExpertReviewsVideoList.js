import React from 'react'
import LazyLoad from 'react-lazy-load'
import {formMoreVideosUrl,formVideoUrl ,formVideoImageUrl} from './VideosCommonFunc'
import {Link} from 'react-router-dom'
class ExpertReviewsVideoList extends React.Component {
	constructor(props) {
		super(props);
		this.onClickVideoUrl = this.onClickVideoUrl.bind(this);
	}
	onClickVideoUrl(video,event){

		if(this.props.onClickVideoUrl && typeof(this.props.onClickVideoUrl) === 'function') {
			event.preventDefault();
			this.props.onClickVideoUrl(video);
		}
	}
	onClickMoreVideoUrl(event) {
		
		if(this.props.onClickMoreVideoUrl && typeof(this.props.onClickMoreVideoUrl) === 'function') {
			event.preventDefault();
			this.props.onClickMoreVideoUrl(this.props.VideoListData.SectionTitle,this.props.VideoListData.MoreVideosUrl);
		}
	}
	renderVideoItem(video) {
		if(!video)
			return null;
		var videoUrl = formVideoUrl(video.VideoTitleUrl,video.BasicId);
		return (
			<div className="carousel-slide">
				<div className="carousel-card">
					<Link to={videoUrl} onClick={this.onClickVideoUrl.bind(this, video)} className="block">
						<div className="card-image-block">
							<LazyLoad>
								<img src={formVideoImageUrl(video.VideoId,'sddefault.jpg')} alt={video.VideoTitle} title={video.VideoTitle} border="0" />
							</LazyLoad>
						</div>
                        <span className="swiper-lazy-preloader"></span>
                    </Link>
                
					<div className="card-details-block">
						<Link to={videoUrl} className="text-default font14 text-bold text-truncate-2" onClick={this.onClickVideoUrl.bind(this,video)}>{video.VideoTitle}</Link>
						<p className="font12 text-xlight-grey margin-top10 margin-bottom10">{video.DisplayDate}</p>
						<p className="font14 text-light-grey margin-bottom10 line-height17 wrap-text text-truncate-2">
							{!video.ShortDescription ? video.Description : video.ShortDescription} 
							{/*<div dangerouslySetInnerHTML={{__html:video.Description}}/>*/}
						</p>
						<div className="grid-6 alpha omega border-light-right font14">
							<span className="bwmsprite video-views-icon margin-right5"></span><span className="text-default">{video.Views}</span>
						</div>
						<div className="grid-6 omega padding-left10 font14">
							<span className="bwmsprite video-likes-icon margin-right5"></span><span className="text-default">{video.Likes}</span>
						</div>
						<div className="clear"></div>
					</div>
				</div>
            </div>
        )

	}
	render(){
		 if(!this.props.VideoListData || !this.props.VideoListData.Videos || !(this.props.VideoListData.Videos.length>0))
		 	return null;
		 return (
		 	<section className="bg-white">
			    <div id="expertReviewsWrapper" className="container bottom-shadow margin-bottom25">
			        <h2 className="text-center padding-top20 padding-bottom15">{this.props.VideoListData.SectionTitle}</h2>
					<div className="carousel-wrapper carousel-type-video">
						{this.props.VideoListData.Videos.map(function(video){
							return this.renderVideoItem(video);
						}.bind(this))}
					</div>
			        <div className="padding-left10 view-all-btn-container margin-top10 padding-bottom20">
			            <Link to={formMoreVideosUrl(this.props.VideoListData.MoreVideosUrl)} title={this.props.VideoListData.SectionTitle + " Bike Videos"} className="btn view-all-target-btn"  onClick={this.onClickMoreVideoUrl.bind(this)}>View more videos<span className="bwmsprite teal-right"></span></Link>
			        </div>
			    </div>
			</section>

		 	)
	}
}

module.exports = ExpertReviewsVideoList;   
       