import { financeCityPopup } from '../actionTypes/FinanceCityPopup'

const getCity = (modelId) => {
  return (dispatch) => {
    var url = '/api/pwa/cities/model/' + modelId + '/';
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
      if (xhr.readyState == 4) {
        if (xhr.status == 200) {
          dispatch({ type: financeCityPopup.FETCH_CITY_SUCCESS, payload: { cities: JSON.parse(xhr.responseText), modelId: modelId } });
        }
        else {
          dispatch({ type: financeCityPopup.FETCH_CITY_FAILURE, payload: { modelId: modelId } });
        }
      }
    }
    xhr.open('GET', url)
    xhr.send();
    dispatch({ type: financeCityPopup.FETCH_CITY })
  }
}

const openCityPopup = () => {
    return {
    type: financeCityPopup.OPEN_CITY_POPUP
  }
}

const closeCityPopup = () => {
  return {
      type: financeCityPopup.CLOSE_CITY_POPUP
  }
}

const cityNext = () => {
    return{
        type: financeCityPopup.CITY_NEXT
    }
}

const setCity = (payload) => {
  return {
    type: financeCityPopup.SET_CITY,
    payload
  }
}

export const fetchCity = (modelId) => (dispatch) => {
  dispatch(getCity(modelId))
}

export const resetCityFailure = () => (dispatch) => {
  dispatch({type: financeCityPopup.RESET_CITY_FAILURE})
}

export const openSelectCityPopup = () => (dispatch) => {
  dispatch(openCityPopup())
}

export const closeSelectCityPopup = () => (dispatch) => {
  dispatch(closeCityPopup())
}

export const selectCityNext = () => (dispatch) => {
   dispatch(cityNext())
}

export const selectCity = (payload) => (dispatch) => {
  dispatch(setCity(payload))
}
