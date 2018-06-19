import {combineReducers} from 'redux-immutable'
import News from './newsReducer'
import Videos from './videosReducer'
import Widgets from './widgetReducer'
import Finance from './financeReducer'
import Emi from './emiReducer'
import Toast from './toastReducer'

var RootReducer = combineReducers({
	  News,
    Videos,
		Widgets,
	  Finance,
		Emi,
		Toast
})

module.exports = RootReducer;
