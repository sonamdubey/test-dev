import {combineReducers} from 'redux-immutable'
import News from './newsReducer'
import Videos from './videosReducer'
import Widgets from './widgetReducer'
import Finance from './SelectBikePopup'

var RootReducer = combineReducers({
	  News,
    Videos,
		Widgets,
	  Finance
})


module.exports = RootReducer;
