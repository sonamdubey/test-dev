import {
	MAKE_HEADER_NORMAL,
	MAKE_HEADER_STICKY
} from '../actionTypes'

import {
    SHOW_SHORTLIST_ICON,
	HIDE_SHORTLIST_ICON
} from '../actionTypes/shortlist'

let initialHeaderWrapperState = {
	isShortlistIconVisible: false,
	isSticky: false
}

export const headerWrapper = (state = initialHeaderWrapperState, action) => {
	switch (action.type) {
		case SHOW_SHORTLIST_ICON:
			return {
				...state,
				isShortlistIconVisible: true
			}

		case HIDE_SHORTLIST_ICON:
			return {
				...state,
				isShortlistIconVisible: false
			}
		case MAKE_HEADER_STICKY:
			return {
				...state,
				isSticky: true
			}
		case MAKE_HEADER_NORMAL:
			return {
				...state,
				isSticky: false
			}
		default:
			return state
	}
}
