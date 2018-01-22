import {combineReducers} from 'redux-immutable'
import News from './newsReducer'
import Videos from './videosReducer'
var RootReducer = combineReducers({
	News,
	Videos
})


module.exports = RootReducer;
