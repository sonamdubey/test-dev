import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { financeCityPopup } from '../actionTypes/FinanceCityPopup'
import { getGlobalCity, setGlobalCity } from '../utils/popUpUtils'

var globalCity = getGlobalCity();
var globalCityName = ( globalCity && globalCity.name.length>0 ) ? globalCity.name : '';
var globalCityId = ( globalCity && globalCity.id>0 ) ? globalCity.id : '';
var initialState = fromJS({
  isActive: false,
  Selection: {
    cityId: globalCityId,
    cityName: globalCityName,
    userChange: false
  },
  Popular: [],
  Other: []
})

export function FinanceCityPopup(state = initialState, action) {
  try {
      if (!state || window._SERVER_RENDERED_DATA)
      return initialState;

    switch (action.type) {
        case financeCityPopup.FETCH_CITY_SUCCESS:
            return state.setIn(['Popular'], fromJS(action.payload.City.slice(0,6))).setIn(['Other'], fromJS(action.payload.City.slice(6))).setIn(['Selection'],fromJS({
                cityId: globalCityId,
                cityName: globalCityName,
                userChange: false
            }));

      case financeCityPopup.OPEN_CITY_POPUP:
          return state.setIn(['isActive'], true).setIn(['Selection'],fromJS({
              cityId: globalCityId,
              cityName: globalCityName,
              userChange: false
          }));

        case financeCityPopup.CLOSE_CITY_POPUP:
            return state.setIn(['isActive'], false);

        case financeCityPopup.CITY_NEXT:
            return state.setIn(['isActive'], false);

      case financeCityPopup.SET_CITY:
        const actionPayload = action.payload;

        const cityId = actionPayload && actionPayload.cityId ? actionPayload.cityId : -1;
        const cityName = actionPayload && actionPayload.cityName ? actionPayload.cityName : "";
        const userChange = actionPayload && actionPayload.userChange ? actionPayload.userChange : false;
        setGlobalCity(cityId, cityName, globalCityId);

        return state.setIn(['Selection'], fromJS({
          cityId: cityId,
          cityName: cityName,
          userChange: userChange
        }))

      default:
        return state
    }
  }
  catch (err) {
    console.log(err)
    return state;
  }
}
