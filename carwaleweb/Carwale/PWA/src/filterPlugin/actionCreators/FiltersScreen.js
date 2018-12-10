import {
	OPEN_FILTER_SCREEN,
	CLOSE_FILTER_SCREEN,
	SET_FILTERS
} from '../actionTypes'

export const openFilterScreen = (accordionItemType) => {
	return {
		type: OPEN_FILTER_SCREEN,
		activeAccordion: accordionItemType
	}
}

export const closeFilterScreen = () => {
	return {
		type: CLOSE_FILTER_SCREEN
	}
}

export const setFilters = (filters) => {
	return {
		type: SET_FILTERS,
		filters
	}
}
