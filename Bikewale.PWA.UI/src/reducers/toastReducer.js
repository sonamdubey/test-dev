import {
	toast
} from '../actionTypes/toast'
import { fromJS } from 'immutable'
import { combineReducers } from 'redux-immutable';

let initialToastState = fromJS({
	isVisible: false,
	message: '',
	duration: 3000,
	toastTimerId: null,
	style: { bottom: 50 }
})

const Toast = (state = initialToastState, action) => {
	switch (action.type) {
		case toast.SET_TOAST:
			return fromJS({
				isVisible: true,
				message: action.message,
				toastTimerId: action.toastTimerId,
				style: action.toastStyle,
				duration: action.duration
			});

		case toast.REMOVE_TOAST:
			return state.setIn(['isVisible'], false).setIn(['toastTimerId'], null);

		default:
			return state
	}
}

module.exports = combineReducers({ Toast });