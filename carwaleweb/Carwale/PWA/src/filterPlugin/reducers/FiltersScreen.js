import {
	OPEN_FILTER_SCREEN,
	CLOSE_FILTER_SCREEN,
	SET_FILTERS
} from '../actionTypes'

const initialFilterScreenState = {
	active: false,
	activeAccordion: null,
	filters: {
		budget: [],
		bodyType: [],
		fuelType: [],
		make: [],
		transmissionType: [],
		seats: []
	}
}

export const filtersScreen = (state = initialFilterScreenState, action) => {
	switch (action.type) {
		case OPEN_FILTER_SCREEN:
			return {
				...state,
				active: true,
				activeAccordion: action.activeAccordion ? action.activeAccordion : null
			}

		case CLOSE_FILTER_SCREEN:
			return {
				...state,
				active: false,
				activeAccordion: null
			}

		case SET_FILTERS:
			return {
				...state,
				filters : action.filters
			}

		default:
			return state
	}
}
