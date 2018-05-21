import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { selectBikePopup } from '../actionTypes/SelectBikePopup'

var initialState = fromJS({
  isActive: false,
  Selection: {
    makeName: "Honda",
    modelName: "CB Hornet 160R",
    modelImage: "https://imgd.aeplcdn.com//310x174//bw/models/honda-cb-hornet-160r.jpg",
    modelId: 693,
    rating: 4.5,
    selectedVersionIndex: 0,
    version: [
      {
        value: 4792,
        label: "STD"
      },
      {
        value: 4481,
        label: "Special Edition - CBS [2017]"
      },
      {
        value: 4793,
        label: "CBS"
      },
      {
        value: 4782,
        label: "ABS - Std"
      },
      {
        value: 4783,
        label: "ABS - Dlx"
      }
    ]
  }
})

export function SelectBikePopup(state, action) {
  try {
    if ( state == undefined || !state.size) {
      return initialState;
    }

    switch (action.type) {
      case selectBikePopup.OPEN_BIKE_POPUP:
        return state.setIn(['isActive'], true);

      case selectBikePopup.CLOSE_BIKE_POPUP:
        return state.setIn(['isActive'], false);

      case selectBikePopup.SELECT_MODEL:
        return initialState

      default:
        return state
    }
  }
  catch(err) {
    console.log(err)
    return state;
  }
}
