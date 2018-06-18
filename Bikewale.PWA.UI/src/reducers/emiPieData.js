import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

let initialState = {
	pieloanAmount: "",
	pieInterestPayable: "",
	pieTotalPrincipalAmount: "",
	pieEmiAmount: "",
}

export const vehiclePie = (state = initialState, action) => {
	if (!state)
	return initialState; 

	switch (action.type) {
		case emiCalculatorAction.UPDATE_PIE_VALUE:
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

export const VehiclePieData= combineReducers({
	vehiclePie
})