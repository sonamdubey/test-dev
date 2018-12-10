import 'isomorphic-fetch'

import {
	REQUEST_MAKE,
	RECEIVE_MAKE,
	MAKE_SELECTION,
	SET_MAKE_STORED_FILTERS
} from '../actionTypes'

import MakeApi from '../apis/Make'

import {
	getCityId
} from '../../selectors/LocationSelectors'

import {
	getMakeFilter,
	getBudgetFilter,
	getBodyTypeFilter,
	getFuelTypeFilter
} from '../selectors/NCFSelectors'

export const requestMake = () => {
	return {
		type: REQUEST_MAKE
	}
}

export const setMakeData = data => {
	return {
		type: RECEIVE_MAKE,
		data
	}
}

export const selectMake = id => {
	return {
		type: MAKE_SELECTION,
		id
	}
}

export const fetchMake = isFetching => (dispatch, getState) => {

	dispatch(requestMake(isFetching))
	const state = getState()
	const options = { ...getCityId(state), ...getBudgetFilter(state), ...getBodyTypeFilter(state), ...getFuelTypeFilter(state) }

	MakeApi
		.get(options)
		.then(data => {setTimeout(() => dispatch(setMakeData(data)), 300)})
		.catch(error => {
			//TODO: how should error affect the state ?
			console.log(error)
		})
}

export const setMakeStoredFilters = storedFilters => {
	return {
		type: SET_MAKE_STORED_FILTERS,
		storedFilters
	}
}
