import { financeCityPopup } from '../actionTypes/FinanceCityPopup'

const fetchCityData = () => {
    return (dispatch) => {
        var method = 'GET';
        var url = '/api/pwa/cities/?requestType=18';
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function() {
            if(xhr.readyState == 4 && xhr.status == 200) {
                if(xhr.status == 200)
                    dispatch({type:financeCityPopup.FETCH_CITY_SUCCESS,payload:JSON.parse(xhr.responseText)})
                else
                    dispatch({type:financeCityPopup.FETCH_CITY_FAILURE})
            }

        }
        xhr.open('GET',url)
        xhr.send();
  }
}

const openCityPopup = () => {
    return {
    type: financeCityPopup.OPEN_CITY_POPUP
  }
}

const closeCityPopup = (payload) => {
  return {
      type: financeCityPopup.CLOSE_CITY_POPUP,
      payload
  }
}

const setCity = (payload) => {
  return {
    type: financeCityPopup.SET_CITY,
    payload
  }
}

export const fetchCity = () => (dispatch) => {
  dispatch(fetchCityData())
}

export const openSelectCityPopup = () => (dispatch) => {
  dispatch(openCityPopup())
}

export const closeSelectCityPopup = () => (dispatch) => {
  dispatch(closeCityPopup())
}

export const selectCity = (payload) => (dispatch) => {
  dispatch(setCity(payload))
}
