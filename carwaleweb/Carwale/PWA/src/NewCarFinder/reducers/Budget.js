import { combineReducers } from 'redux'

import {
	UPDATE_BUDGET_SLIDER_VALUE,
	UPDATE_BUDGET_INPUT_VALUE
} from '../actionTypes'

import {
	fireNonInteractiveTracking
} from '../../utils/Analytics'

import snapPoints from '../utils/rheostat/constants/budgetSnapPoints'
import { BUDGET_SLIDER_MIN, BUDGET_SLIDER_MAX } from '../constants/index'
import {CATEGORY_NAME} from '../constants/index'
import { trackCustomData } from '../../utils/cwTrackingPwa'

let initialSliderState = {
	min: BUDGET_SLIDER_MIN,
	max: BUDGET_SLIDER_MAX,
	values: [0],
	userChange:false,
	snapPoints
}

const slider = (state = initialSliderState, action) => {
	switch (action.type) {
		case UPDATE_BUDGET_SLIDER_VALUE:
			if(!action.userChange){
				fireNonInteractiveTracking(CATEGORY_NAME,"NCF_BudgetFilter","Suggested_"+action.values[0])
				trackCustomData(CATEGORY_NAME,"BudgetFilterSuggestion","value="+action.values[0],false)
			}
			return {
				...state,
				values: action.values,
				min: action.min || state.min,
				max: action.max || state.max,
				userChange: action.userChange
			}

		default:
			return state
	}
}


// budget input field
let initialInputState = {
	value: 0
}

const inputBox = (state = initialInputState, action) => {
	switch (action.type) {
		case UPDATE_BUDGET_INPUT_VALUE:
			return {
				...state,
				value: action.value
			}

		default:
			return state
	}
}

export const budget = combineReducers({
	slider,
	inputBox
})
