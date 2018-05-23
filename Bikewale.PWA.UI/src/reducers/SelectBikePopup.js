import { combineReducers } from 'redux-immutable'
import { fromJS, toJS } from 'immutable'
import {mapVersionsDataToDropdownList} from '../components/Finance/FinanceCommon'
import { selectBikePopup } from '../actionTypes/SelectBikePopup'

var initialState = fromJS({
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
  MakeModelList: [],
  IsFetchingModelDetail: false
});

export function SelectBikePopup(state = initialState, action) {
  try {
    if (state == undefined || (state != undefined && state.size == 0)) {
      return initialState;
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
        if (action.payload != null && action.payload.versions.length > 0) {
          return state.setIn(['Selection'], fromJS({ ...(state.get('Selection').toJS()), versionList: mapVersionsDataToDropdownList(action.payload.versions), selectedVersionIndex: 0}));
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
          rating: action.payload.reviewRate, modelId: action.payload.modelId})).setIn(['IsFetchingModelDetail'],false);
        }
        else {
          return state;
        }
      case selectBikePopup.FETCH_MODEL_DETAIL_FAILURE:
        return state.setIn(['IsFetchingModelDetail'], false);
      
      case selectBikePopup.FETCH_MODEL_DETAIL:
        return state.setIn(['IsFetchingModelDetail'], true);

      case selectBikePopup.SET_BIKE_VERSION:
        return state.setIn(['Selection'], fromJS({ ...(state.get('Selection').toJS()), selectedVersionIndex: action.payload.versionId }))

      default:
        return state
    }
  }
  catch(err) {
    console.log(err)
    return state;
  }
}
