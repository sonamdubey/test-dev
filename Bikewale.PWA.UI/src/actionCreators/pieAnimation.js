import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

const startPieAnimation = () => {
  return {
    type: emiCalculatorAction.START_PIE_ANIMATION
  }
}

const stopPieAnimation = () => {
  return {
    type: emiCalculatorAction.STOP_PIE_ANIMATION
  }
}

export const startAnimation = () => (dispatch) => {
  dispatch(startPieAnimation(setTimeout(function(){
    dispatch(stopPieAnimation())
  },500)))
}
