import {
	SHOW_FOOTER,
	HIDE_FOOTER
} from '../actionTypes'

export const showFooter = data => {
	return {
		type: SHOW_FOOTER
	}
}

export const hideFooter = () => {
	return {
		type: HIDE_FOOTER
	}
}
