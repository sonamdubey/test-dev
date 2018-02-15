import React from 'react'
import TopVideosContainer from './TopVideosContainer'
import OtherVideosContainer from './OtherVideosContainer'
import PopularBikeImageCarouselContainer from '../Widgets/PopularBikeImageCarouselContainer'
import {startTimer , endTimer} from '../../utils/timing'
import {GA_PAGE_MAPPING} from '../../utils/constants'
import { scrollPosition , resetScrollPosition  } from '../../utils/scrollUtils'


if(!process.env.SERVER) {
	require('../../../stylesheet/video.sass');
}

var childComponentCount;
var childComponent; 
// 0th index -> top videos
// 1st index -> other videos

// export function to reset api count
class VideoLandingComponent extends React.Component {
	constructor(props) {
		super(props);
		childComponentCount = 3;
		childComponent = {'TopVideosComponent' : 1 , 'OtherVideosComponent' : 1,'ImageCarouselComponent':2};
		startTimer(1,0); // 1 component to complete + 0 ads
		this.handleTimingAndScrollingForChildComponents = this.handleTimingAndScrollingForChildComponents.bind(this);
		if(typeof(gaObj)!="undefined")
		{
		    gaObj = GA_PAGE_MAPPING["VideosPage"];
		}
	}
	handleTimingAndScrollingForChildComponents(completedComponent) {
		if(!childComponent[completedComponent]) return;
		
		if(childComponent[completedComponent] == 1) {
			childComponent[completedComponent] = 0;
			childComponentCount--;
			if(childComponentCount == 0) {
				//trigger scroll
				if(scrollPosition.x >= 0 && scrollPosition.y >=0) {
					window.scrollTo(scrollPosition.x,scrollPosition.y);
					resetScrollPosition();
				}
				//trigger logging
				endTimer("component-render");
			}
		}
	}
	render() {
		
		return(
			<div className="page-type--landing">
				<TopVideosContainer logAndScrollHandler={this.handleTimingAndScrollingForChildComponents}/>
                <PopularBikeImageCarouselContainer logAndScrollHandler={this.handleTimingAndScrollingForChildComponents}/>
				<OtherVideosContainer logAndScrollHandler={this.handleTimingAndScrollingForChildComponents}/>
			</div>
		)

	}
}

module.exports = VideoLandingComponent;



