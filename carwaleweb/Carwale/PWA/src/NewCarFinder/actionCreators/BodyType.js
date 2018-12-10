import {
	REQUEST_BODY_TYPE,
	RECEIVE_BODY_TYPE,
	BODY_TYPE_SELECTION,
	SET_BODY_STORED_FILTERS
} from '../actionTypes'
import {
	getNCFFilterParams
} from '../../filterPlugin/selectors/FilterPluginSelectors'
import BodyTypeApi from '../apis/BodyType'

import {
	getCityId
} from '../../selectors/LocationSelectors'

import {
	getBudgetFilter
} from '../selectors/NCFSelectors'

export const requestBodyType = () => {
	return {
		type: REQUEST_BODY_TYPE
	}
}

export const setBodyTypeData = data => {
	return {
		type: RECEIVE_BODY_TYPE,
		data
	}
}

export const selectBodyType = id => {
	return {
		type: BODY_TYPE_SELECTION,
		id
	}
}

export const fetchBodyType = () => (dispatch, getState) => {
	dispatch(requestBodyType())
	let state = getState()
	const currentScreen = state.newCarFinder.filter.filterScreens.currentScreen
	const prevFilters = state.newCarFinder.filter.filterScreens.screenOrder.slice(0, currentScreen).reduce((acc, screen) => acc.concat([screen.name]), [])
	const options = getNCFFilterParams(state, prevFilters)
	if(state.newCarFinder.shortlistCars.removedModelIds != undefined){
		options.removedModelIds = state.newCarFinder.shortlistCars.removedModelIds
	}
	BodyTypeApi
		.get(options)
		.then(data => { setTimeout(() => dispatch(setBodyTypeData(data)), 300) })
		.catch( error => {
			//TODO: how should error affect the state ?
			console.log(error)
		})
}

export const setBodyStoredFilters = storedFilters => {
	return {
		type: SET_BODY_STORED_FILTERS,
		storedFilters
	}
}
