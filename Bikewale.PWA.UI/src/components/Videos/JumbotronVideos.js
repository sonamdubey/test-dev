import React from 'react'
import LazyLoad from 'react-lazy-load'
import {Link} from 'react-router-dom'
import {formVideoUrl ,formVideoImageUrl} from './VideosCommonFunc'

class JumbotronVideos extends React.Component {
	constructor(props) {
		super(props);
		this.renderFirstVideo = this.renderFirstVideo.bind(this);
		this.renderOtherVideoList =  this.renderOtherVideoList.bind(this);

	}
	onClickVideoUrl(video,event){

		if(this.props.onClickVideoUrl && typeof(this.props.onClickVideoUrl) === 'function') {
			event.preventDefault();
			this.props.onClickVideoUrl(video);
		}
	}
	renderFirstVideo(video) {
		if(!video) {
			return null;
		}
		return (
			<div className="jumbotron">
                <Link to={formVideoUrl(video.VideoTitleUrl,video.BasicId)} className="jumbotron__link" onClick={this.onClickVideoUrl.bind(this,video)}>
                	<LazyLoad>
                    <img src={formVideoImageUrl(video.VideoId,'sddefault.jpg')} alt={video.VideoTitle} title={video.VideoTitle} border="0" className="jumbotron__image"/>
                    </LazyLoad>
                    <span className="jumbotron__head">{video.VideoTitle}</span>
                </Link>
            </div>
			)
	}
	renderOtherVideoItem(video) {
		if(!video)
			return null;
		var videoUrl = formVideoUrl(video.VideoTitleUrl,video.BasicId);
		return (
			<li className="misc-container">
                <Link to={videoUrl} className="misc-list-image" onClick={this.onClickVideoUrl.bind(this,video)}>
					<div className="misc-container__image">
						<LazyLoad>
		                    <img src={formVideoImageUrl(video.VideoId,'default.jpg')} alt={video.VideoTitle} title={video.VideoTitle} border="0" />
		              	</LazyLoad>
					</div>
                </Link>
                <Link to={videoUrl} className="misc-list-title" onClick={this.onClickVideoUrl.bind(this,video)}>{video.VideoTitle}</Link>
            </li>
       )
	}
	renderOtherVideoList(videoList) {
		if(!videoList || !(videoList.length > 1)){
			return null;
		}
		return (
			<ul className="miscWrapper jumbotron-list bottom-shadow margin-bottom20">
				{videoList.map(function(video,index){
					if(index == 0) return null;
					return this.renderOtherVideoItem(video);
				}.bind(this))}
            </ul>
		)


	}

	render(){
		if(!this.props.LandingFirstVideo || (!(this.props.LandingFirstVideo.length > 0))) {
			return null;
		}
		return (
			<div className="container">
				{this.renderFirstVideo(this.props.LandingFirstVideo[0])}
				{this.renderOtherVideoList(this.props.LandingFirstVideo)}
			</div>
		)
	}
}

module.exports = JumbotronVideos;
