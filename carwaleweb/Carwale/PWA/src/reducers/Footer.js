import {
	SHOW_FOOTER,
	HIDE_FOOTER,
	SHOW_HEADER_FOOTER,
	HIDE_HEADER_FOOTER
} from '../actionTypes'

let initialFooterState = {
	isVisible: true
}

export const footer = (state = initialFooterState, action) => {
	switch (action.type) {
		case SHOW_HEADER_FOOTER:
		case SHOW_FOOTER:
			return {
				...state,
				isVisible: true
			}
		case HIDE_HEADER_FOOTER:
		case HIDE_FOOTER:
			return {
				...state,
				isVisible: false
			}

		default:
			return state
	}
}
