import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

let initialEmiState = {
	pieloanAmount: "",
	pieInterestPayable: "",
	pieTotalPrincipalAmount: "",
	pieEmiAmount: "",
}

export const vehiclePie = (state = initialEmiState, action) => {
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