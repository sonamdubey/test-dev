import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'

import {toJS} from '../../immutableWrapperContainer'
import VideoDetailComponent from './VideoDetailComponent'
import { fetchVideoDetail , resetVideoDetail , fetchRelatedInfo , fetchModelSlug } from '../../actionCreators/videosDetailActionCreators'
var mapStateToProps = function(store) {
	return {
		// Status : store.getIn(['Videos','VideoDetail','RelatedInfoStatus']),
		VideoInfoStatus : store.getIn(['Videos','VideoDetail','VideoInfoStatus']),
		VideoDetail : store.getIn(['Videos','VideoDetail','VideoInfo']),
		InitialDataDict : store.getIn(['Videos','VideoDetail','InitialDataDict']),
		RelatedInfoApi : store.getIn(['Videos','VideoDetail','RelatedInfoApi']),
		RelatedInfoStatus : store.getIn(['Videos','VideoDetail','RelatedInfoStatus']),
		RelatedInfo : store.getIn(['Videos','VideoDetail','RelatedInfo']),
		ModelInfoStatus : store.getIn(['Videos','VideoDetail','ModelInfoStatus']),
		ModelInfo : store.getIn(['Videos','VideoDetail','ModelInfo'])

		
	}
}

var mapDispatchToProps = function(dispatch) {
	return {
		fetchVideoDetail : bindActionCreators(fetchVideoDetail,dispatch),
		resetVideoDetail : bindActionCreators(resetVideoDetail,dispatch),
		fetchModelSlug : bindActionCreators(fetchModelSlug , dispatch),
		fetchRelatedInfo : bindActionCreators(fetchRelatedInfo,dispatch)
	}

}

module.exports = connect(mapStateToProps,mapDispatchToProps)(toJS(VideoDetailComponent));
