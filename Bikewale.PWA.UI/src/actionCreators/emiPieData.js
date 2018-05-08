import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

const updatePieValue = function(pieInterestPayable, pieTotalPrincipalAmount, pieloanAmount) {
    return function(dispatch) {
       dispatch({
        type: emiCalculatorAction.UPDATE_PIE_VALUE,
        pieInterestPayable,
        pieTotalPrincipalAmount,
        pieloanAmount
      })
    }
}