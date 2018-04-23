import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { selectBikePopupAction } from '../actionTypes/actionTypes'

var initialState = fromJS({
  isActive: false
})

export function SelectBikePopup(state, action) {
  try {
    if (!state)
      return initialState;

    switch (action.type) {
      case selectBikePopupAction.OPEN:
        return state.setIn(['isActive'], true);

      default:
        return state
    }
  }
  catch(err) {
    console.log(err)
    return state;
  }
}
