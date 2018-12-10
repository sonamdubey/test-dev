import {
	ADD_MODEL_TO_COMPARE,
	REMOVE_MODEL_FROM_COMPARE
} from '../actionTypes'

export const addModelToCompare = (data) => {
	return {
		type: ADD_MODEL_TO_COMPARE,
		data
	}
}

export const removeModelFromCompare = (versionId) => {
	return {
		type: REMOVE_MODEL_FROM_COMPARE,
		versionId
	}
}
