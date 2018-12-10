import {
	HIDE_SHORTLIST_POPUP,
	SHOW_SHORTLIST_POPUP,
	ADD_MODEL_TO_SHORTLIST,
	REMOVE_MODEL_FROM_SHORTLIST,
	BLACKLIST_CAR,
	RESET_SHORTLIST_CARS
} from '../actionTypes/shortlist'

const initialShortlistState = {
	active: false,
	count: 0,
	max: 5,
	modelIds: [],
	removedModelIds: [],
}

export const shortlistCars = (state = initialShortlistState, action) => {
	let newCount
	switch (action.type) {

		case SHOW_SHORTLIST_POPUP:
			return {
				...state,
				active: true
			}

		case HIDE_SHORTLIST_POPUP:
			return {
				...state,
				active: false
			}

		case ADD_MODEL_TO_SHORTLIST:
			newCount = state.count + 1;
			if (newCount <= state.max && !(state.modelIds.findIndex(x => x == action.modelId) >= 0)) {
				const modelIds = state.modelIds.concat([action.modelId])
				return {
					...state,
					count: newCount,
					modelIds
				}
			}
			return state

		case REMOVE_MODEL_FROM_SHORTLIST:
			newCount = state.count - 1
			const modelIds = state.modelIds.filter(x => x != action.modelId)
			if (modelIds.length <= 0) {
				return {
					...state,
					count: newCount,
					modelIds,
					active: false
				}
			}
			return {
				...state,
				count: newCount,
				modelIds
			}

		case BLACKLIST_CAR:
			const removedModelIds = state.removedModelIds.concat([action.modelId])
			return {
				...state,
				removedModelIds
			}

		case RESET_SHORTLIST_CARS:
			return {
				...state,
				modelIds: action.modelIds,
				count: action.count
			}
		default:
			return state
	}
}
