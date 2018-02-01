import VideosByCategoryComponent from './VideosByCategoryComponent'
import {fetchVideosByCategory} from '../../actionCreators/videosByCategoryActionCreator'
import {fetchVideoDetail,fetchModelSlug} from '../../actionCreators/videosDetailActionCreators'

import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {withRouter} from 'react-router-dom'
import {toJS} from '../../immutableWrapperContainer'

var mapStateToProps = function(store) {
	return {
		VideosByCategory : store.getIn(['Videos','VideosByCategory'])
	}

}

var mapDispatchToProps = function(dispatch) {
	return {
		fetchVideosByCategory : bindActionCreators(fetchVideosByCategory,dispatch),
		fetchVideoDetail : bindActionCreators(fetchVideoDetail,dispatch),
		fetchModelSlug : bindActionCreators(fetchModelSlug , dispatch)
	}

}

module.exports = connect(mapStateToProps,mapDispatchToProps)(toJS(withRouter(VideosByCategoryComponent)));
