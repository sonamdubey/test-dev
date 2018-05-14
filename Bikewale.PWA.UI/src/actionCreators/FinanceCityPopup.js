import { financeCityPopup } from '../actionTypes/FinanceCityPopup'

const fetchCityData = () => {
  return {
    type: financeCityPopup.FETCH_CITY_SUCCESS
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
