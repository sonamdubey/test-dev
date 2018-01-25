import {combineReducers} from 'redux-immutable'
import {VideosLanding} from './videosLandingReducer'
import {VideosByCategory} from './videosByCategoryReducer'
import {VideoDetail} from './videoDetailReducer'
var Videos = combineReducers({
	VideosLanding,
	VideosByCategory,
	VideoDetail
});
module.exports = Videos;