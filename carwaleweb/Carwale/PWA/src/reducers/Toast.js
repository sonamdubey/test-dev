import {
	SET_TOAST,
	REMOVE_TOAST
} from '../actionTypes'

let initialToastState = {
	isVisible: false,
	message: '',
	duration: 3000,
	toastTimerId: null,
	style: null,
}

export const toast = (state = initialToastState, action) => {
	switch (action.type) {
		case SET_TOAST:
			return {
				...state,
				isVisible: true,
				message: action.message,
				toastTimerId: action.toastTimerId,
				style: action.toastStyle,
			}

		case REMOVE_TOAST:
			return {
				...state,
				isVisible: false,
				toastTimerId: null,
			}

		default:
			return state
	}
}
