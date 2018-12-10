import {
	SHOW_HEADER,
	HIDE_HEADER,
	OPEN_NAV_DRAWER,
	CLOSE_NAV_DRAWER,
	SHOW_HEADER_FOOTER,
	HIDE_HEADER_FOOTER
} from '../actionTypes'

let initialHeaderState = {
	isVisible: true,
	isNavDrawerActive: false
}

export const header = (state = initialHeaderState, action) => {
	switch (action.type) {
		case SHOW_HEADER_FOOTER:
		case SHOW_HEADER:
			return {
				...state,
				isVisible: true
			}
		case HIDE_HEADER_FOOTER:
		case HIDE_HEADER:
			return {
				...state,
				isVisible: false
			}

		case OPEN_NAV_DRAWER:
			return {
                ...state,
				isNavDrawerActive: true
			}

		case CLOSE_NAV_DRAWER:
			return {
				...state,
				isNavDrawerActive: false
			}
		default:
			return state
	}
}
