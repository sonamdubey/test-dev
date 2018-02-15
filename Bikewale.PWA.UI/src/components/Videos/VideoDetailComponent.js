import React from 'react'
import {withRouter} from 'react-router-dom'
import VideoDetail from './VideoDetail'
import VideoModelSlug from './VideoModelSlug'
import { isServer } from '../../utils/commonUtils'
import { scrollPosition , resetScrollPosition , isBrowserWithoutScrollSupport } from '../../utils/scrollUtils'
import {endTimer} from '../../utils/timing'
import {Status,GA_PAGE_MAPPING} from '../../utils/constants'
import Spinner from '../Shared/Spinner'
import Breadcrumb from '../Shared/Breadcrumb'
import Footer from '../Shared/Footer'
import SpinnerRelative from '../Shared/SpinnerRelative'
import CarouselBikeItem from '../Shared/CarouselBikeItem'
import CarouselVideoItem from '../Shared/CarouselVideoItem'
import CarouselContainer from '../Shared/CarouselContainer'
import {pushVideoDetailUrl} from './VideosCommonFunc'
class VideoDetailComponent extends React.Component {
	constructor(props) {
		super(props);
		

		this.fetchModelSlug = this.fetchModelSlug.bind(this);
		this.fetchVideoDetailApi = this.fetchVideoDetailApi.bind(this);
		this.fetchRelatedInfo = this.fetchRelatedInfo.bind(this);
		this.logger = this.logger.bind(this);
		this.scrollToPosition = this.scrollToPosition.bind(this);
		this.renderCarousel = this.renderCarousel.bind(this);

		if(typeof(gaObj)!="undefined")
		{
		    gaObj = GA_PAGE_MAPPING["VideoDetailsPage"];
		}
	}
	fetchVideoDetailApi() {
		if(this.props.VideoInfoStatus !== Status.Fetched && this.props.VideoInfoStatus !== Status.IsFetching) {
			this.props.fetchVideoDetail(this.props.match.params.basicId);
		}
	}
	fetchModelSlug(props) {
		if(props.ModelInfoStatus !== Status.IsFetching && props.ModelInfoStatus !== Status.Fetched) {
					props.fetchModelSlug(props.match.params.basicId);
				}
	}
	fetchRelatedInfo(props) {
		if(props.RelatedInfoApi && props.VideoInfoStatus === Status.Fetched && props.RelatedInfoStatus !== Status.Fetched && props.RelatedInfoStatus !== Status.IsFetching) {
			props.fetchRelatedInfo(props.RelatedInfoApi);
		}
	}
	componentWillReceiveProps(nextProps) {
		try {

			var prevUrlParam = this.props.match.params;
			var nextUrlParam = nextProps.match.params;
			
			if(prevUrlParam.basicId === nextUrlParam.basicId) {
				if(nextProps) { // hit api for relatedinfoapi
					this.fetchRelatedInfo(nextProps);
				}
				return;
			}
			if(nextProps.VideoDetail && nextUrlParam.basicId!==nextProps.VideoDetail.BasicId) { // same component on screen but new basicid
				var initialDataDict = !nextProps.InitialDataDict || nextProps.InitialDataDict.length==0 ? null : nextProps.InitialDataDict[nextUrlParam.basicId];
				this.props.fetchVideoDetail(initialDataDict || nextUrlParam.basicId);
				this.props.fetchModelSlug(nextUrlParam.basicId);
				if(isBrowserWithoutScrollSupport()) {
		            window.scrollTo(0, 0); 
		        }
			}
			else if(nextProps && nextProps.VideoInfoStatus != Status.IsFetching)  { // different basicid loaded but videodetail is null
				var initialDataDict = !nextProps.InitialDataDict || nextProps.InitialDataDict.length==0 ? null : nextProps.InitialDataDict[nextUrlParam.basicId];
				this.props.fetchVideoDetail(initialDataDict || nextUrlParam.basicId);
				this.props.fetchModelSlug(nextUrlParam.basicId);
				if(isBrowserWithoutScrollSupport()) {
		            window.scrollTo(0, 0); 
		        }
			}	
			 
		}
		catch(err) {
			console.log(err);
		}
		
			
			
	}
	componentDidUpdate() {
		var BasicIsFromUrl = this.props.match.params.basicId;
		var basicIdFromStore = this.props.VideoDetail ? this.props.VideoDetail.BasicId : null;
		// var nextUrlParam = nextProps.match.params;
		if(BasicIsFromUrl == basicIdFromStore) {
			this.logger();
			this.scrollToPosition();
		}
			
			

			
	}
	scrollToPosition() {
		if(scrollPosition.x >= 0 && scrollPosition.y >= 0) { // needs to be scrolled
            if(this.props && 
				this.props.VideoInfoStatus ==  Status.Fetched && 
				this.props.ModelInfoStatus == Status.Fetched &&
				this.props.RelatedInfoStatus == Status.Fetched) {
			  // checks whether ready to scroll
                window.scrollTo(scrollPosition.x,scrollPosition.y);
                resetScrollPosition();
                
            }
        }
	}
	componentDidMount() {
		this.logger()
		this.fetchVideoDetailApi();
		this.fetchModelSlug(this.props);
		if(isBrowserWithoutScrollSupport()) {
            window.scrollTo(0, 0); 
        }

	}
	componentWillUnmount(){
		this.props.resetVideoDetail();
	}
	logger() {
		try {
			if(this.props && 
				this.props.VideoInfoStatus ==  Status.Fetched && 
				this.props.ModelInfoStatus == Status.Fetched &&
				this.props.RelatedInfoStatus == Status.Fetched) {
				endTimer("component-render");
			}
		}
		catch(err) {

		}
	}
	
	renderCarousel(relatedInfo,relatedInfoApi) {	
		var carouselComponentList = [];
		for(var i =0;i<relatedInfoApi.length;i++) {
			var data=relatedInfo[i];
			var carouselData ={
				Heading : data.Heading,
				CompleteListUrl : data.CompleteListUrl,
				CompleteListUrlAlternateLabel : data.CompleteListUrlAlternateLabel
			}
			var carouselComponent=null;
			switch(relatedInfoApi[i].Type) {
				case 0:
					carouselData.List = data.BikesList;
					carouselData.WrapperCssClass = "new-bike-carousel";
					carouselComponent = <CarouselContainer carouselData={carouselData} childComponent={CarouselBikeItem}/>;
					break;
				case 1:
					carouselData.List = data.VideosList;
					carouselData.WrapperCssClass = "video-swiper";
					carouselComponent = <CarouselContainer carouselData={carouselData} childComponent={CarouselVideoItem}/>;
					break;
			}

			if(carouselComponent){
				carouselComponentList.push(carouselComponent);	
			}
			
		}
		return carouselComponentList;


	}
	render() {
		var video = this.props.VideoDetail;
		if(isServer()) {
			video = this.props.VideoInfo.VideoInfo;
		}

		if(!isServer() && !video && !this.props.match.params && this.props.match.params.basicId !== video.BasicId) {
			return <SpinnerRelative/>;
		}
		var initialData = !this.props.InitialDataDict ? null : this.props.InitialDataDict[this.props.match.params.basicId];
		
		if(this.props.VideoInfoStatus !== Status.Fetched && !isServer()) {
			if(!initialData)
				return <Spinner/>;
		}
		var videoUrl=null , videoTitle=null,displayDate=null;
		if(initialData) {
			
			videoUrl =  initialData.VideoUrl;
			videoTitle = initialData.VideoTitle;
			displayDate = initialData.DisplayDate;	
			
		}
		else {
				videoUrl =  video.VideoUrl;
				videoTitle = video.VideoTitle;
				displayDate = video.DisplayDate;
			
		}
		var modelInfo = this.props.ModelInfo;
		var relatedInfo = this.props.RelatedInfo;
		var relatedInfoApi = this.props.RelatedInfoApi;
		if(isServer()) {
			relatedInfoApi = this.props.VideoInfo.RelatedInfoApi;
				
		}
		return(
			<div>
				<VideoDetail videoDetail = {video} initialData={{videoUrl:videoUrl , videoTitle :videoTitle , displayDate:displayDate}}/>
				{ isServer() || (this.props.ModelInfoStatus == Status.Fetched && modelInfo)? 
						<VideoModelSlug modelInfo={modelInfo} />
						: null}
				{(isServer() || (this.props.RelatedInfoStatus == Status.Fetched && relatedInfo && relatedInfoApi)) ?
						this.renderCarousel(relatedInfo,relatedInfoApi)
						: null}				
				{isServer() || this.props.VideoInfoStatus === Status.Fetched ? 
					<div>
						<Breadcrumb breadcrumb={[{Href : '/m/',Title : 'Home'},{Href : '/m/bike-videos/',Title : 'Videos'},{Href : '/m/bike-videos/category'+video.SectionUrl , Title : video.SectionTitle},{Href : '' , Title : videoTitle}]}/>
						<Footer/>
					</div>
					: null}
			</div>
		)


	}
}


module.exports = withRouter(VideoDetailComponent);

