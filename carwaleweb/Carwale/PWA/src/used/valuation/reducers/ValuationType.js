import { BUY_CAR_ID } from '../constants'
import {
	SELECT_VALUATION_TYPE
} from '../actionTypes'
let initialValuationTypeState = BUY_CAR_ID

export const type = (state = initialValuationTypeState, action) => {
	switch(action.type) {
		case SELECT_VALUATION_TYPE:
			return Number.parseInt(action.value)
		default:
			return state
	}
}
