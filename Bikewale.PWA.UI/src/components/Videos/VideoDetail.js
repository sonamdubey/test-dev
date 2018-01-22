import React from 'react'

import {Status} from '../../utils/constants'
import SocialMediaSlug from '../Shared/SocialMediaSlug'

import Spinner from '../Shared/Spinner'
import SpinnerRelative from '../Shared/SpinnerRelative'
import {isServer} from '../../utils/commonUtils'


class VideoDetail extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
			shownShortDesc : true
		};
		if(isServer()) {
			this.props.Status = Status.Fetched;
		}
		this.showShortDesc = this.showShortDesc.bind(this);
		
	}

	showShortDesc(event) {
		this.setState({
			shownShortDesc : !this.state.shownShortDesc
		});
	}
	renderDescription(description,shortDescription) {
		if(!shortDescription) {
			return (
				<div className="model-preview-main-content">
	                {description}
	            </div>
	        )
		}

		if(this.state.shownShortDesc) {
			return (
				<div>
					<div className="model-preview-main-content">
	                    {shortDescription}
	                </div>
	                <a href="javascript:void(0)" className="font14 read-more-model-preview" rel="nofollow" onClick={this.showShortDesc.bind()}>Read more</a>

                </div>
                
			)
		}
		else {
			return (
				<div>
					<div className="model-preview-more-content">
	                    {description}
	                </div>
	                <a href="javascript:void(0)" className="font14 read-more-model-preview" rel="nofollow" onClick={this.showShortDesc.bind()}>Collapse</a>
                </div>
				)
		}
	}
	componentDidUpdate() {
		try {
			var container = document.getElementById("g-ytsubscribe");
			if(container && (container.childNodes && container.childNodes.length==0) && gapi) {
			    var options = {
			        'channel': 'powerdriftofficial',
			        'layout': 'default',
			        'count' : 'hidden'
			      };
		    	gapi.ytsubscribe.render(container, options);

			}	
		}
		catch(err) {

		}
	}
	render() {
		var video = this.props.videoDetail;
		// if(!video) return null;
		var initialData = this.props.initialData;
		var videoUrl = !initialData ? "" :initialData.videoUrl;
		var videoTitle = !initialData ? "" :initialData.videoTitle;
		var displayDate = !initialData ? "" :initialData.displayDate;
		if(!videoUrl || !videoTitle ) return null;

		return (
			<div>
				<section className="bg-white">
				    <div className="container margin-bottom10">
				        <div id="embedVideo">
				            <iframe height="180" src={videoUrl} frameborder="0" allowfullscreen></iframe>
				        </div>
				        <div className="border-solid-bottom video-detail">
    		   			    <h1 className="font18">{videoTitle}</h1>
				            <p className="card-list__info">{displayDate}</p>
				            
							{ !video ? <SpinnerRelative/> : (
				            	<div>
						            <div className="card-list__details">
						                {this.renderDescription(video.Description,video.ShortDescription)}
						            </div>
						            <div className="views-count-container font14 leftfloat border-light-right">
						                <span className="bwmsprite video-views-icon margin-right5"></span><span className="text-default comma">{video.Views}</span>
						            </div>
						            <div className="views-count-container font14 leftfloat padding-left10 border-light-right">
						                <span className="bwmsprite video-likes-icon margin-right5"></span><span className="text-default comma">{video.Likes}</span>
						            </div>
						            <div className="font14 leftfloat padding-left10 powerdrift-sub-btn">
						                <script defer src="https://apis.google.com/js/platform.js?onload=onLoadCallback"></script>
						                <div id="g-ytsubscribe" className="g-ytsubscribe" data-channel="powerdriftofficial" data-layout="default" data-count="hidden"></div>
						            </div>
						           
						            <div className="clear"></div>
				           		 	<SocialMediaSlug/>
				            		<div className="clear"></div>
								</div>) }

				        </div>
				    </div>
				</section>
				
			</div>
		)

	}
}

module.exports = VideoDetail;




	// <a href="javascript:void(0);"
        						// onclick={this.renderYT.bind()}>Click here for a button!</a>