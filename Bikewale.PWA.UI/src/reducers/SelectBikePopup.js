import { combineReducers } from 'redux-immutable'
import { fromJS, toJS } from 'immutable'

import { selectBikePopup } from '../actionTypes/SelectBikePopup'

var initialState = {
  isActive: false,
  Selection: {
    makeName: "",
    modelName: "",
    hostUrl: "",
    originalImagePath: "",
    modelId: -1,
    rating: 0,
    selectedVersionIndex: -1,
    versionList: []
  },
  MakeModelList: []
};

export function SelectBikePopup(state, action) {
  try {
    if (state == undefined || !state.size) {
      return fromJS(initialState);
    }

    switch (action.type) {
      case selectBikePopup.OPEN_BIKE_POPUP:
        return state.setIn(['isActive'], true);

      case selectBikePopup.CLOSE_BIKE_POPUP:
        return state.setIn(['isActive'], false);

      case selectBikePopup.SELECT_MODEL:
        if (action.payload != null) {
          return state.setIn(['Selection'], fromJS({ ...(state.get('Selection').toJS()), ...action.payload}))
        }
        else {
          return state;
        }

      case selectBikePopup.FETCH_BIKELIST_SUCCESS:
        if (action.payload != null) {
          return state.setIn(['MakeModelList'], fromJS(action.payload));
        }
        else {
          return state;
        }

      case selectBikePopup.FETCH_BIKELIST_FAILURE:
        return state.setIn(['MakeModelList'], []);

      case selectBikePopup.FETCH_VERSIONLIST_SUCCESS:
        if (action.payload != null) {
          return state.setIn(['Selection'], fromJS({ ...(state.get('Selection').toJS()), versionList: action.payload.versions}));
        }
        else {
          return state;
        }

      case selectBikePopup.FETCH_VERSIONLIST_FAILURE:
        return state.setIn(['Selection'], fromJS({ ...(state.get('Selection').toJS()), versionList: []}));

      case selectBikePopup.FETCH_MODEL_DETAIL_SUCCESS:
        if (action.payload) {
          return state.setIn(['Selection'], fromJS({ ...(state.get('Selection').toJS()), makeName: action.payload.makeDetails.makeName,
            modelName: action.payload.modelName, hostUrl: action.payload.hostUrl, originalImagePath: action.payload.originalImagePath,
          rating: action.payload.reviewRate, selectedVersionIndex: 0, modelId: action.payload.modelId}))
        }
        else {
          return state;
        }
      case selectBikePopup.FETCH_MODEL_DETAIL_FAILURE:
        return state.setIn(['Selection'], fromJS(initialState));

      default:
        return state
    }
  }
  catch(err) {
    console.log(err)
    return state;
  }
}
