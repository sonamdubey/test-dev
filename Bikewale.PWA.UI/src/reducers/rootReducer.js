import {combineReducers} from 'redux-immutable'
import News from './newsReducer'
import Videos from './videosReducer'
import Widgets from './widgetReducer'
var RootReducer = combineReducers({
	News,
    Videos,
    Widgets
})


module.exports = RootReducer;
