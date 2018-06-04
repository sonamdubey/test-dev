import { fromJS } from 'immutable'

import { financeCityPopup } from '../actionTypes/FinanceCityPopup'
import { getGlobalCity, setGlobalCity } from '../utils/popUpUtils'

var globalCity = getGlobalCity();
var globalCityName = (globalCity && globalCity.name && globalCity.name.length > 0) ? globalCity.name : '';
var globalCityId = (globalCity && globalCity.id > 0) ? globalCity.id : -1;
var initialState = fromJS({
  isActive: false,
  Selection: {
    cityId: globalCityId,
    cityName: globalCityName,
    userChange: false
  },
  currentGlobalCityName: globalCityName,
  currentGlobalCityId: globalCityId,
  Popular: [],
  Other: [],
  IsFetching: false,
  RelatedModelId: -1,
  CityFetchError: false
});

export function FinanceCityPopup(state = initialState, action) {
  try {
    if (state == undefined || (state != undefined && state.size == 0))
      return initialState;
    const selectionObj = state.get('Selection').toJS();
    const currentCityId = selectionObj != null && selectionObj.cityId > 0 ? selectionObj.cityId : -1;
    switch (action.type) {
      case financeCityPopup.FETCH_CITY_SUCCESS:
        return state.setIn(['Popular'], fromJS(action.payload.cities.filter(item => item.popularityOrder < 7)))
          .setIn(['Other'], fromJS(action.payload.cities.filter(item => item.popularityOrder > 6)))
          .setIn(['IsFetching'], false).setIn(['RelatedModelId'], action.payload.modelId);

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
        const currentGlobalCityName = cityId > -1 ? cityName : state.get('currentGlobalCityName');
        const currentGlobalCityId = cityId > -1 ? cityId : state.get('currentGlobalCityId');  
        if (cityId > -1) {
          setGlobalCity(cityId, cityName, currentCityId);
        }
        return state.setIn(['Selection'], fromJS({
          cityId: cityId,
          cityName: cityName,
          userChange: userChange
        })).setIn(['currentGlobalCityId'], currentGlobalCityId).setIn(['currentGlobalCityName'], currentGlobalCityName);

      case financeCityPopup.FETCH_CITY_FAILURE:
        return initialState.setIn(['IsFetching'], false).setIn(['RelatedModelId'], action.payload.modelId).setIn(['CityFetchError'], true);
      
      case financeCityPopup.FETCH_CITY:
        return state.setIn(['IsFetching'], true);
      
      case financeCityPopup.RESET_CITY_FAILURE:
        return state.setIn(['CityFetchError'], false);
      default:
        return state
    }
  }
  catch (err) {
    console.log(err)
    return state;
  }
}
