import { combineReducers } from 'redux'

import {
	UPDATE_PIE_VALUE
} from '../actionTypes'

let initialEmiState = {
	pieloanAmount: "",
	pieInterestPayable: "",
	pieTotalPrincipalAmount: "",
	pieEmiAmount: "",
}
export const vehiclePie = (state = initialEmiState, action) =>{
	switch (action.type) {
		case UPDATE_PIE_VALUE:
			return {
				...state,
				pieloanAmount: action.pieloanAmount,
				pieInterestPayable: action.pieInterestPayable || state.pieInterestPayable,
				pieTotalPrincipalAmount: action.pieTotalPrincipalAmount || state.pieTotalPrincipalAmount,
				pieEmiAmount: action.pieEmiAmount || state.pieEmiAmount,
			}
		default:
			return state
	}
}
