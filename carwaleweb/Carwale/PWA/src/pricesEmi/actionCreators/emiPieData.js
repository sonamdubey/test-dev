import {
	UPDATE_PIE_VALUE
} from '../actionTypes'

const updatePieValue = (pieInterestPayable, pieTotalPrincipalAmount, pieloanAmount) => {
	return {
		type: UPDATE_PIE_VALUE,
		pieInterestPayable,
		pieTotalPrincipalAmount,
		pieloanAmount
	}
}

export const updatePieValueData = ({pieInterestPayable, pieTotalPrincipalAmount, pieloanAmount}) => (dispatch, getState) => {
		dispatch(updatePieValue(pieInterestPayable, pieTotalPrincipalAmount, pieloanAmount))
		getState().newEmiPrices.vehiclePie
}
