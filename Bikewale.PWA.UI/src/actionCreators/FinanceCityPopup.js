import { financeCityPopup } from '../actionTypes/FinanceCityPopup'

const getCityData = (modelId) => {
    return (dispatch) => {
        var method = 'GET';
        var url = '/api/pwa/cities/model/'+ modelId + '/';
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
  dispatch(getCityData(modelId))
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
