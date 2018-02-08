import React from 'react'

import {Status} from '../../utils/constants'
import Footer from '../Shared/Footer'
import Breadcrumb from '../Shared/Breadcrumb'
import {isServer} from '../../utils/commonUtils'
import ModelBrandsList from './ModelBrandsList'
import VideoCarouselByCategory from './VideoCarouselByCategory'
import ImageCard from '../Shared/ImageCard'
import {toJS} from '../../immutableWrapperContainer'
import {formMoreVideosUrl,pushVideoDetailUrl,pushVideosByCategoryUrl} from './VideosCommonFunc'

class OtherVideosComponent extends React.Component{
	constructor(props) {
		super(props);
		if(isServer()) {
			this.props.Status = Status.Fetched;
		}
		this.onClickMoreVideoUrl = this.onClickMoreVideoUrl.bind(this);
		this.onClickVideoUrl = this.onClickVideoUrl.bind(this);
		this.callParentLogAndScrollHandler = this.callParentLogAndScrollHandler.bind(this);
	}
	callParentLogAndScrollHandler() {
		if(this.props.Status == Status.Fetched) {
			if(typeof this.props.logAndScrollHandler == 'function') {
				this.props.logAndScrollHandler('OtherVideosComponent'); // 1 for signifying second api inset of 2 on videos page
			}
		}
	}
	onClickMoreVideoUrl(sectionTitle,moreVideosUrl) {
		pushVideosByCategoryUrl(this,sectionTitle,moreVideosUrl);
		//TODO call next api
	}
	onClickVideoUrl(video) {
		pushVideoDetailUrl(this,video);
		//call apis of video page
	}
	componentDidUpdate() {
		this.callParentLogAndScrollHandler();
	}
	componentDidMount() {
		this.callParentLogAndScrollHandler();
		if(this.props.Status !== Status.Fetched && this.props.Status !== Status.IsFetching) {
			this.props.fetchOtherVideosData();
		}
	}

	render(){
		if(this.props.Status !== Status.Fetched || !this.props.OtherVideos)
			return null;

		return (
			<div>
				<VideoCarouselByCategory VideoListData={this.props.OtherVideos.FirstRide} onClickVideoUrl={this.onClickVideoUrl} onClickMoreVideoUrl={this.onClickMoreVideoUrl}/>
				<VideoCarouselByCategory VideoListData={this.props.OtherVideos.LaunchAlert} onClickVideoUrl={this.onClickVideoUrl} onClickMoreVideoUrl={this.onClickMoreVideoUrl}/>
				<ImageCard />
				<VideoCarouselByCategory VideoListData={this.props.OtherVideos.FirstLook} onClickVideoUrl={this.onClickVideoUrl} onClickMoreVideoUrl={this.onClickMoreVideoUrl}/>
				<ModelBrandsList Brands={this.props.OtherVideos.Brands} />
				<VideoCarouselByCategory VideoListData={this.props.OtherVideos.PowerDriftBlockbuster} onClickVideoUrl={this.onClickVideoUrl} onClickMoreVideoUrl={this.onClickMoreVideoUrl}/>
				<VideoCarouselByCategory VideoListData={this.props.OtherVideos.MotorSports} onClickVideoUrl={this.onClickVideoUrl} onClickMoreVideoUrl={this.onClickMoreVideoUrl}/>
				<VideoCarouselByCategory VideoListData={this.props.OtherVideos.PowerDriftSpecials} onClickVideoUrl={this.onClickVideoUrl} onClickMoreVideoUrl={this.onClickMoreVideoUrl}/>
				<VideoCarouselByCategory VideoListData={this.props.OtherVideos.PowerDriftTopMusic} onClickVideoUrl={this.onClickVideoUrl} onClickMoreVideoUrl={this.onClickMoreVideoUrl}/>
				<VideoCarouselByCategory VideoListData={this.props.OtherVideos.Miscellaneous} onClickVideoUrl={this.onClickVideoUrl} onClickMoreVideoUrl={this.onClickMoreVideoUrl}/>
				<Breadcrumb breadcrumb={[{Href : '/m/',Title : 'Home'},{Href : '',Title : 'Videos'}]}/>
				<Footer/>
			</div>
		)
	}
}

module.exports = OtherVideosComponent;