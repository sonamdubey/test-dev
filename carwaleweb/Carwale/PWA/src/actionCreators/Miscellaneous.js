import {
    SHOW_HEADER_FOOTER,
    HIDE_HEADER_FOOTER
} from '../actionTypes'

export const showHeaderFooter = data => {
	return {
		type: SHOW_HEADER_FOOTER
	}
}

export const hideHeaderFooter = () => {
	return {
		type: HIDE_HEADER_FOOTER
	}
}
