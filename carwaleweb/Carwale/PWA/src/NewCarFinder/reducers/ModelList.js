import {
	REQUEST_MODEL_LIST,
	RECEIVE_MODEL_LIST,
    APPEND_MODEL_LIST
} from '../actionTypes'

let initialModelListState = {
	mainListing: {
		isFetching: false,
		data: [],
		totalModels:0,
		nextPageUrl: null
	},
	shortListing: {
		isFetching: false,
		data: [],
		totalModels:0,
		nextPageUrl: null
	}
}

export const listingData = (state = initialModelListState, action) => {
	let updatedState
	switch (action.type) {
		case REQUEST_MODEL_LIST:
			updatedState = action.resetList ? initialModelListState[action.pageName] : state[action.pageName]
			updatedState.isFetching = true
			return {
				...state,
				[action.pageName]: updatedState
			}

		case RECEIVE_MODEL_LIST:
			updatedState = {
				...state[action.pageName],
				isFetching: false,
				data: action.data,
				totalModels: action.totalModels,
				nextPageUrl: action.nextPageUrl,
				cityName: action.cityName,
				cityId: action.cityId
			}
			return {
				...state,
				[action.pageName]: updatedState
			}

		case APPEND_MODEL_LIST:
			updatedState = {
				...state[action.pageName],
				isFetching: false,
				data: state[action.pageName].data.concat(action.data),
				totalModels: action.totalModels,
				nextPageUrl: action.nextPageUrl
			}
			return {
				...state,
				[action.pageName]: updatedState
			}

		default:
			return state
	}
}
