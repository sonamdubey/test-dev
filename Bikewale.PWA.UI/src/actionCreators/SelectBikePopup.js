import { selectBikePopup } from '../actionTypes/SelectBikePopup'

const openBikePopup = () => {
  return {
    type: selectBikePopup.OPEN_BIKE_POPUP
  }
}

const closeBikePopup = () => {
  return {
    type: selectBikePopup.CLOSE_BIKE_POPUP
  }
}

const getSelectedBikeDetail = (dispatch, modelId) => {
  if (modelId > 0) {
    let xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
      if (xhr.readyState == 4) {
        if (xhr.status == 200)
          dispatch({ type: selectBikePopup.FETCH_MODEL_DETAIL_SUCCESS, payload: JSON.parse(xhr.responseText) });
        else
          dispatch({ type: selectBikePopup.FETCH_MODEL_DETAIL_FAILURE });
      }
    }
    xhr.open('GET', '/api/model/?modelId=' + modelId);
    xhr.send();
  }
}

const getBikeVersionList = (dispatch, modelId, cityId) => {
  var xhr = new XMLHttpRequest();
  xhr.onreadystatechange = function () {
    if (xhr.readyState == 4) {
      if (xhr.status == 200)
        dispatch({ type: selectBikePopup.FETCH_VERSIONLIST_SUCCESS, payload: JSON.parse(xhr.responseText) });
      else
        dispatch({ type: selectBikePopup.FETCH_VERSIONLIST_FAILURE });
    }
  }
  xhr.open('GET', '/api/pwa/PQVersionList/?modelid=' + modelId + '&cityId=' + cityId);
  xhr.send();
}

const setModel = (modelId) => {
  return {
    type: selectBikePopup.SELECT_MODEL,
    payload:{ modelId : modelId} 
  }
}

const getMakeModelList = (dispatch) => {
  let xhr = new XMLHttpRequest();
  xhr.onreadystatechange = function () {
    if (xhr.readyState == 4) {
      if (xhr.status == 200)
        dispatch({ type: selectBikePopup.FETCH_BIKELIST_SUCCESS, payload: JSON.parse(xhr.responseText) });
      else
        dispatch({ type: selectBikePopup.FETCH_BIKELIST_FAILURE });
    }
  }
  xhr.open('GET', '/api/pwa/model/all/v2/2/');
  xhr.send();
}

export const openSelectBikePopup = () => (dispatch) => {
  dispatch(openBikePopup())
}

export const closeSelectBikePopup = () => (dispatch) => {
  dispatch(closeBikePopup())
}

export const fetchSelectedBikeDetail = (modelId) => (dispatch) => {
  return getSelectedBikeDetail(dispatch, modelId);
}

export const selectModel = (modelId) => (dispatch) => {
  dispatch(setModel(modelId));
}

export const fetchMakeModelList = () => (dispatch) => {
  return getMakeModelList(dispatch);
}

export const fetchBikeVersionList = (modelId, cityId) => (dispatch) => {
  return getBikeVersionList(dispatch, modelId, cityId);
}

