import { financeCityPopupAction } from '../actionTypes/actionTypes'

module.exports = {
  fetchCity: function() {
    return function(dispatch) {
      dispatch({
        type: financeCityPopupAction.FETCH_CITY_SUCCESS
      })
    }
  },

  openSelectCityPopup: function() {
    return function(dispatch) {
      dispatch({
        type: financeCityPopupAction.OPEN_POPUP
      })
    }
  },

  setCity: function(payload) {
    return function(dispatch) {
      dispatch({
        type: financeCityPopupAction.SET_CITY,
        payload
      })
    }
  }
}
