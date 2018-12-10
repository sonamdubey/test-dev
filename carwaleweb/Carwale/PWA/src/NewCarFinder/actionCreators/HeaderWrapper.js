import {
    MAKE_HEADER_NORMAL,
    MAKE_HEADER_STICKY
} from '../actionTypes'

import {
    HIDE_SHORTLIST_ICON,
    SHOW_SHORTLIST_ICON
} from '../actionTypes/shortlist'

export const showShortlistIcon = () => {
	return {
		type: SHOW_SHORTLIST_ICON
	}
}

export const hideShortlistIcon = () => {
	return {
		type: HIDE_SHORTLIST_ICON
	}
}

export const makeHeaderSticky = () => {
	return {
		type: MAKE_HEADER_STICKY
	}
}

export const makeHeaderNormal = () => {
	return {
		type: MAKE_HEADER_NORMAL
	}
}
