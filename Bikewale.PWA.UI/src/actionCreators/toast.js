import {
	toast,
} from '../actionTypes/toast';
import { toJS } from 'immutable';

export const setToast = data => {
	return {
		type: toast.SET_TOAST,
		message: data.message,
		toastTimerId: data.toastTimerId,
		toastStyle: data.toastStyle,
	}
}

export const removeToast = () => {
	return {
		type: toast.REMOVE_TOAST
	}
}

export const initToast = (data) => (dispatch, getState) => {
	let toast = getState().get('Toast').toJS()['Toast'];

	let toastStyle;

	// show toast message relative to click position
	if(data.event) {
		const event = data.event

		const offset = {
			top: window.pageYOffset || document.documentElement.scrollTop
		}

		const clickPositionY = event.pageY - offset.top + 20
		const availableHeight = window.innerHeight - clickPositionY

		if (availableHeight < 100) {
			toastStyle = {
				...toastStyle,
				top: 'auto',
				bottom: availableHeight + 50 // 50px offset height i.e average height of toast message
			}
		}
		else {
			toastStyle = {
				...toastStyle,
				top: clickPositionY + 'px',
				bottom: 'auto'
			}
		}
	}

	if(data.style) {
		toastStyle = { ...toastStyle, ...data.style}
	}


	// set toast message duration
	let toastDuration = toast && toast.duration ? toast.duration : 3000;

	if(data.duration) {
		toastDuration = data.duration
	}
    if(toast != undefined)
        clearTimeout(toast.toastTimerId) // clear previous toast
    else {

    }

	let toastTimerId = setTimeout(() => {
		dispatch(removeToast())
	}, toastDuration)

	dispatch(setToast({
		...data,
		toastTimerId,
		toastStyle
	}))
}

export const clearToast = () => (dispatch, getState) => {
	clearTimeout(getState().toast.toastTimerId)
	dispatch(removeToast())
}