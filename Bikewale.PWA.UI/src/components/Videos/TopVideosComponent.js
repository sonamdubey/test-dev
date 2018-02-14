import React from 'react'

import {Status} from '../../utils/constants'
import Spinner from '../Shared/Spinner'
import JumbotronVideos from './JumbotronVideos'
import ExpertReviewsVideoList from './ExpertReviewsVideoList'
import {isServer} from '../../utils/commonUtils'

import {formVideoUrl,pushVideoDetailUrl,pushVideosByCategoryUrl} from './VideosCommonFunc'
class TopVideosComponent extends React.Component {
	constructor(props) {
		super(props);
		if(isServer()) {
			this.props.Status = Status.Fetched;
		}
		this.onClickVideoUrl = this.onClickVideoUrl.bind(this);
		this.onClickMoreVideoUrl = this.onClickMoreVideoUrl.bind(this);
		this.callParentLogAndScrollHandler = this.callParentLogAndScrollHandler.bind(this);
	}
	onClickMoreVideoUrl(sectionTitle,moreVideosUrl) {
		pushVideosByCategoryUrl(this,sectionTitle,moreVideosUrl);
	}
	onClickVideoUrl(video) {
		pushVideoDetailUrl(this,video);

	}

	componentDidMount() {
		this.callParentLogAndScrollHandler();
		if(this.props.Status !== Status.Fetched && this.props.Status !== Status.IsFetching) {
			this.props.fetchTopVideosData();
		}
	}
	componentDidUpdate(prevProps, prevState) {
		this.callParentLogAndScrollHandler();
	}
	callParentLogAndScrollHandler() {
		if(this.props.Status == Status.Fetched) {
			if(typeof this.props.logAndScrollHandler == 'function') {
				this.props.logAndScrollHandler('TopVideosComponent'); 
			}
		}
	}
	render() {
		if(this.props.Status !== Status.Fetched || !this.props.TopVideos)
			return <Spinner/>;
		var topVideos = this.props.TopVideos;

		return (
			<div>
				<JumbotronVideos LandingFirstVideo={topVideos.LandingFirstVideos} onClickVideoUrl={this.onClickVideoUrl}/>
				<ExpertReviewsVideoList VideoListData={topVideos.ExpertReviews} onClickVideoUrl={this.onClickVideoUrl} onClickMoreVideoUrl={this.onClickMoreVideoUrl}/>
			</div>
		)
	}
}

module.exports = TopVideosComponent;
