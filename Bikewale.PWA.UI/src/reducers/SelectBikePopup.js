import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { selectBikePopup } from '../actionTypes/SelectBikePopup'

var initialState = fromJS({
  isActive: false,
  Selection: {
    makeName: "Honda",
    modelName: "CB Hornet 160R",
    modelImage: "https://imgd.aeplcdn.com//310x174//bw/models/honda-cb-hornet-160r.jpg",
    rating: 4.5,
    versionId: 4792,
    version: [
      {
        id: 4792,
        name: "STD"
      },
      {
        id: 4481,
        name: "Special Edition - CBS [2017]"
      },
      {
        id: 4793,
        name: "CBS"
      },
      {
        id: 4782,
        name: "ABS - Std"
      },
      {
        id: 4783,
        name: "ABS - Dlx"
      }
    ]
  }
})

export function SelectBikePopup(state, action) {
  try {
    if (!state.size) {
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
