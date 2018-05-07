import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/emiActionTypes'

var initialState = fromJS({
  isAnimate: false,
})

export function PieAnimation(state, action) {
  try {
    if (!state)
      return initialState;

    switch (action.type) {
      
      case emiCalculatorAction.START_PIE_ANIMATION:
        return state.setIn(['isAnimate'], true);

      case emiCalculatorAction.STOP_PIE_ANIMATION:
        return state.setIn(['isAnimate'], false);

      default:
        return state
    }
  }
  catch (err) {
    console.log(err)
    return state;
  }
}
