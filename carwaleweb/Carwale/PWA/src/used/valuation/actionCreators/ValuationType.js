import {
	SELECT_VALUATION_TYPE
} from '../actionTypes'

const valuationTypeSelection = value => {
	return {
		type: SELECT_VALUATION_TYPE,
		value
	}
}

export const selectValuationType = value => (dispatch) => {
	dispatch(valuationTypeSelection(value))
}
