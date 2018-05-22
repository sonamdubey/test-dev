import TopVideosComponent from './TopVideosComponent'
import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {withRouter} from 'react-router-dom'
import { fetchTopVideosData } from '../../actionCreators/videosLandingActionCreators'
import  {fetchVideoDetail , fetchModelSlug} from '../../actionCreators/videosDetailActionCreators'
import {fetchVideosByCategory} from '../../actionCreators/videosByCategoryActionCreator'
import { toJS } from '../../immutableWrapperContainer'

var mapStateToProps = function(store) {
	return {
		TopVideos : store.getIn(['Videos', 'VideosLanding','TopVideos']),
		Status : store.getIn(['Videos', 'VideosLanding','TopVideosStatus'])
	}
}

var mapDispatchToProps = function(dispatch) {
	return {
		fetchTopVideosData :  bindActionCreators(fetchTopVideosData,dispatch),
		fetchVideosByCategory : bindActionCreators(fetchVideosByCategory,dispatch),
		fetchVideoDetail : bindActionCreators(fetchVideoDetail,dispatch),
		fetchModelSlug : bindActionCreators(fetchModelSlug , dispatch)
	}
}

module.exports = connect(mapStateToProps,mapDispatchToProps)(toJS(withRouter(TopVideosComponent)));
