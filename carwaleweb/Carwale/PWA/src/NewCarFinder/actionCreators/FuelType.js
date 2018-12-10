import 'isomorphic-fetch'

import {
	REQUEST_FUEL_TYPE,
	RECEIVE_FUEL_TYPE,
	FUEL_TYPE_SELECTION,
	SET_FUEL_STORED_FILTERS
} from '../actionTypes'

import FuelTypeApi from '../apis/FuelType'

import {
	getCityId
} from '../../selectors/LocationSelectors'

import {
	getBudgetFilter,
	getBodyTypeFilter
} from '../selectors/NCFSelectors'

export const requestFuelType = () => {
	return {
		type: REQUEST_FUEL_TYPE
	}
}

export const setFuelTypeData = data => {
	return {
		type: RECEIVE_FUEL_TYPE,
		data
	}
}

export const selectFuelType = id => {
	return {
		type: FUEL_TYPE_SELECTION,
		id
	}
}

export const fetchFuelType = isFetching => (dispatch, getState) => {
	dispatch(requestFuelType(isFetching))
	const state = getState()
	const options = {...getCityId(state), ...getBudgetFilter(state), ...getBodyTypeFilter(state)}
	FuelTypeApi
		.get(options)
		.then( data => { setTimeout(() => dispatch(setFuelTypeData(data)), 300)})
		.catch( error => {
			//TODO: how should error affect the state ?
			console.log(error)
		})
}


export const setFuelStoredFilters = storedFilters => {
	return {
		type: SET_FUEL_STORED_FILTERS,
		storedFilters
	}
}
