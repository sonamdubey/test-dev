import React from 'react'
import {Link} from 'react-router-dom'
import Footer from '../Shared/Footer'
import LazyLoad from 'react-lazy-load'
import {isServer} from '../../utils/commonUtils'
import CategoryHeader from '../Shared/CategoryHeader'
import Spinner from '../Shared/Spinner'
import Breadcrumb from '../Shared/Breadcrumb'
import {Status,GA_PAGE_MAPPING} from '../../utils/constants'
import {formVideoUrl,formVideoImageUrl} from './VideosCommonFunc'

import {pushVideoDetailUrl} from './VideosCommonFunc'
class VideosByCategoryComponent extends React.Component{
	constructor(props) {
		super(props);
		
		this.onClickVideoUrl = this.onClickVideoUrl.bind(this);

		if(typeof(gaObj)!="undefined")
		{
		    gaObj = GA_PAGE_MAPPING["VideoCategoryPage"];
		}
	}
	componentDidMount() {
		if(this.props.VideosByCategory.Status !== Status.Fetched && this.props.VideosByCategory.Status !== Status.IsFetching) {
			//call actioncreator
			this.props.fetchVideosByCategory(this.props.match.params.categoryId,null);
		}
		document.getElementsByTagName("body")[0].classList.add("hide-bw-header");
	}
	componentWillUnmount() {
		document.getElementsByTagName("body")[0].classList.remove("hide-bw-header");
	}
	onClickVideoUrl(video,event) {
		event.preventDefault();
		pushVideoDetailUrl(this,video);
	}
	renderVideoItem(video) {
		if(!video)
			return null;
		var videoUrl = formVideoUrl(video.VideoTitleUrl,video.BasicId);
		return (
			<li>
                <div className="misc-container margin-bottom10">
                    <Link to={videoUrl} className="misc-list-image" onClick={this.onClickVideoUrl.bind(this,video)}>
						<div className="misc-container__image">
	                    	<LazyLoad>
		                        <img src={formVideoImageUrl(video.VideoId,'mqdefault.jpg')} alt={video.VideoTitle} title={video.VideoTitle} border="0" />
		                   	</LazyLoad>
						</div>
                    </Link>
                    <Link to={videoUrl} title={video.VideoTitle} className="misc-list-title" onClick={this.onClickVideoUrl.bind(this,video)}>{video.VideoTitle}</Link>
                </div>
                <div className="views-count-container">
                    <span className="bwmsprite video-views-icon">
					</span><span className="text-light-grey">Views:</span>
					<span>{video.Views}</span>
                </div>
                <div className="views-count-container padding-left20 border-light-left">
                    <span className="bwmsprite video-likes-icon"></span>
					<span className="text-light-grey">Likes:</span>
					<span>{video.Likes}</span>
                </div>
                <div className="clear"></div>
            </li>
        )
	}
	renderVideoList(videos){
		if(videos && videos.length>0) {
			return (
				<section className="bg-white padding-top50 bottom-shadow margin-bottom30">
		            <div className="container">
		                <ul id="listVideos1" className="miscWrapper">
		    				{videos.map(function(video){
		    					return this.renderVideoItem(video);
		    				}.bind(this))}
		                </ul>
		            </div>
		        </section>
				)
		}
		else {
			return <Spinner/>;
		}
	}
	render() {
		if(!this.props.VideosByCategory)
			return null;
		var title = !this.props.VideosByCategory.SectionTitle ? '' : this.props.VideosByCategory.SectionTitle+' Video';
		return (
			<div>
				<CategoryHeader PageHeading={title}/>
				{this.renderVideoList(this.props.VideosByCategory ? this.props.VideosByCategory.Videos : null)}
				{!this.props.VideosByCategory || !this.props.VideosByCategory.Videos || this.props.VideosByCategory.Videos.length == 0 ? 
						null :
						<div>
							<Breadcrumb breadcrumb={[{Href : '/m/',Title : 'Home'},{Href : '/m/bike-videos/',Title : 'Videos'},{Href : '' , Title : title}]}/>
							<Footer/>
						</div>}
			</div>
			)
	}
}


module.exports = VideosByCategoryComponent;
