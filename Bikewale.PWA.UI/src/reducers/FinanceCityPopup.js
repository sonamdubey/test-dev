import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { financeCityPopup } from '../actionTypes/FinanceCityPopup'
import { getGlobalCity, setGlobalCity, IsGlobalCityPresent } from '../utils/popUpUtils'

var globalCity = getGlobalCity();
var globalCityName = (globalCity && globalCity.name && globalCity.name.length > 0) ? globalCity.name : '';
var globalCityId = (globalCity && globalCity.id > 0) ? globalCity.id : '';
var initialState = fromJS({
  isActive: false,
  Selection: {
    cityId: globalCityId,
    cityName: globalCityName,
    userChange: false
  },
  Popular: [],
  Other: [],
  IsFetching: false
});

export function FinanceCityPopup(state = initialState, action) {
  try {
      if (!state || window._SERVER_RENDERED_DATA)
      return initialState;
    const selectionObj = state.get('Selection').toJS();
    const currentCityId = selectionObj != null && selectionObj.cityId > 0 ? selectionObj.cityId : -1;
    switch (action.type) {
        case financeCityPopup.FETCH_CITY_SUCCESS:
        let selection = IsGlobalCityPresent(action.payload, currentCityId)?
        state.get('Selection')
        :
        fromJS({
          cityId: -1,
          cityName: "",
          userChange: false
        });
            return state.setIn(['Popular'], fromJS(action.payload.filter(item => item.popularityOrder < 7)))
            .setIn(['Other'], fromJS(action.payload.filter(item => item.popularityOrder > 6)))
            .setIn(['Selection'], selection).setIn(['IsFetching'], false);

      case financeCityPopup.OPEN_CITY_POPUP:
          return state.setIn(['isActive'], true);

        case financeCityPopup.CLOSE_CITY_POPUP:
        return state.setIn(['isActive'], false);

      case financeCityPopup.CITY_NEXT:
        return state.setIn(['isActive'], false);

      case financeCityPopup.SET_CITY:
        const actionPayload = action.payload;
        const cityId = actionPayload != null && actionPayload.cityId != null ? actionPayload.cityId : -1;
        const cityName = actionPayload != null && actionPayload.cityName != null ? actionPayload.cityName : "";
        const userChange = actionPayload != null && actionPayload.userChange != null ? actionPayload.userChange : false;
        setGlobalCity(cityId, cityName, currentCityId);
        return state.setIn(['Selection'], fromJS({
          cityId: cityId,
          cityName: cityName,
          userChange: userChange
        }));

      case financeCityPopup.FETCH_CITY_FAILURE:
        return initialState.setIn(['IsFetching'], false);
      
      case financeCityPopup.FETCH_CITY:
        return state.setIn(['IsFetching'], true);
      default:
        return state
    }
  }
  catch (err) {
    console.log(err)
    return state;
  }
}
