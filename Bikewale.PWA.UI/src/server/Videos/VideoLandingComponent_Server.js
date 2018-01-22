import React from 'react'
import TopVideosComponent from '../../components/Videos/TopVideosComponent'
import OtherVideosComponent from '../../components/Videos/OtherVideosComponent'
import {startTimer} from '../../utils/timing'

import { scrollPosition , resetScrollPosition } from '../../utils/scrollUtils'

var childComponentCount;
var childComponent; 
// 0th index -> top videos
// 1st index -> other videos

// export function to reset api count
class VideoLandingComponent_Server extends React.Component {
	render() {
		var TopVideos = !this.props || !this.props.TopVideos ? null : this.props.TopVideos;
		var OtherVideos = !this.props || !this.props.OtherVideos ? null : this.props.OtherVideos;
		
		return(
			<div>
				<TopVideosComponent TopVideos={TopVideos} />
				<OtherVideosComponent OtherVideos={OtherVideos}  />
			</div>
		)
		
		

	}
}

module.exports = VideoLandingComponent_Server;



