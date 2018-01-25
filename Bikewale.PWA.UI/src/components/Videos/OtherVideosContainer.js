import OtherVideosComponent from './OtherVideosComponent'
import { fetchOtherVideosData } from '../../actionCreators/videosLandingActionCreators'
import {fetchVideosByCategory} from '../../actionCreators/videosByCategoryActionCreator'
import  {fetchVideoDetail , fetchModelSlug} from '../../actionCreators/videosDetailActionCreators'
import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {withRouter} from 'react-router-dom'
import {toJS} from '../../immutableWrapperContainer'
var mapStateToProps = function(store) {
	return {
		OtherVideos	: store.getIn(['Videos','VideosLanding','OtherVideos']),
		Status	: store.getIn(['Videos','VideosLanding','OtherVideosStatus'])

	}
}
var mapDispatchToProps = function(dispatch) {
	return {
		fetchOtherVideosData : bindActionCreators(fetchOtherVideosData,dispatch),
		fetchVideoDetail : bindActionCreators(fetchVideoDetail,dispatch),
		fetchVideosByCategory : bindActionCreators(fetchVideosByCategory,dispatch),
		fetchModelSlug : bindActionCreators(fetchModelSlug , dispatch)

	}
}


module.exports = connect(mapStateToProps,mapDispatchToProps)(toJS(withRouter(OtherVideosComponent)));