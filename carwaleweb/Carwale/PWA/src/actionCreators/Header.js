import {
	SHOW_HEADER,
	HIDE_HEADER,
	OPEN_NAV_DRAWER,
	CLOSE_NAV_DRAWER
} from '../actionTypes'

export const showHeader = data => {
	return {
		type: SHOW_HEADER
	}
}

export const hideHeader = () => {
	return {
		type: HIDE_HEADER
	}
}

export const openNavDrawer = () => {
	return {
		type: OPEN_NAV_DRAWER
	}
}

export const closeNavDrawer = () => {
	return {
		type: CLOSE_NAV_DRAWER
	}
}

