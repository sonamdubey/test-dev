import {
	HIDE_SHORTLIST_POPUP,
	SHOW_SHORTLIST_POPUP,
	REMOVE_MODEL_FROM_SHORTLIST,
	ADD_MODEL_TO_SHORTLIST,
	RESET_SHORTLIST_CARS
} from '../actionTypes/shortlist'

export const showShortlistPopup = () => {
	return {
		type: SHOW_SHORTLIST_POPUP
	}
}

export const hideShortlistPopup = () => {
	return {
		type: HIDE_SHORTLIST_POPUP
	}
}

export const addModelToShortlist = (modelId) => {
	return {
		type: ADD_MODEL_TO_SHORTLIST,
		modelId
	}
}

export const removeModelFromShortlist = (modelId) => {
	return {
		type: REMOVE_MODEL_FROM_SHORTLIST,
		modelId
	}
}

export const blacklistCar = (modelId) => {
	return {
		type: BLACKLIST_CAR,
		modelId
	}
}

export const resetShortlistCars = (count,modelIds) => {
	return {
		type: RESET_SHORTLIST_CARS,
		count,
		modelIds
	}
}
