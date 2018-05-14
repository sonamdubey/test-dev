import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { financeCityPopup } from '../actionTypes/FinanceCityPopup'

var initialState = fromJS({
  isActive: false,
  Selection: {
    cityId: 1,
    cityName: "Mumbai",
    userChange: false
  },
  Popular: []
    /*{
      cityId: 1,
      cityName: "Mumbai",
      maskingName: "mumbai",
      icon: "https://imgd.aeplcdn.com/0x0/bw/static/icons/city/mumbai.svg"
    },
    {
      cityId: 128,
      cityName: "Ahmedabad",
      maskingName: "ahmedabad",
      icon: "https://imgd.aeplcdn.com/0x0/bw/static/icons/city/ahmedabad.svg"
    },
    {
      cityId: 2,
      cityName: "Bangalore",
      maskingName: "bangalore",
      icon: "https://imgd.aeplcdn.com/0x0/bw/static/icons/city/bangalore.svg"
    },
    {
      cityId: 176,
      cityName: "Chennai",
      maskingName: "chennai",
      icon: "https://imgd.aeplcdn.com/0x0/bw/static/icons/city/chennai.svg"
    },
    {
      cityId: 105,
      cityName: "Hyderabad",
      maskingName: "hyderabad",
      icon: "https://imgd.aeplcdn.com/0x0/bw/static/icons/city/hyderabad.svg"
    },
    {
      cityId: 12,
      cityName: "Pune",
      maskingName: "pune",
      icon: "http://imgd.aeplcdn.com/0x0/bw/static/icons/city/pune.svg"
    }
  ]*/,
  Other: []
   /* {
      cityId: 333,
      cityName: "Abohar",
      maskingName: "abohar"
    },
    {
      cityId: 625,
      cityName: "Abu",
      maskingName: "abu"
    },
    {
      cityId: 99,
      cityName: "Adilabad",
      maskingName: "adilabad"
    },
    {
      cityId: 1346,
      cityName: "Adimali",
      maskingName: "adimali"
    },
    {
      cityId: 490,
      cityName: "Adoni",
      maskingName: "adoni"
    },
    {
      cityId: 1222,
      cityName: "Agar Malwa",
      maskingName: "agarmalwa"
    },
    {
      cityId: 266,
      cityName: "Agartala",
      maskingName: "agartala"
    }
  ]*/
})

export function FinanceCityPopup(state = initialState, action) {
    console.log(state)
  try {
      if (!state)
      return initialState;

    switch (action.type) {
        case financeCityPopup.FETCH_CITY_SUCCESS:
            return state.setIn(['Popular'], fromJS(action.payload.City.slice(0,6))).setIn(['Other'], fromJS(action.payload.City.slice(6))).setIn(['Selection'],fromJS({
                cityId: 1,
                cityName: "Mumbai",
                userChange: false
            }));

      case financeCityPopup.OPEN_CITY_POPUP:
        return state.setIn(['isActive'], true);

      case financeCityPopup.CLOSE_CITY_POPUP:
        return state.setIn(['isActive'], false);

      case financeCityPopup.SET_CITY:
        const actionPayload = action.payload;

        const cityId = actionPayload && actionPayload.cityId ? actionPayload.cityId : -1;
        const cityName = actionPayload && actionPayload.cityName ? actionPayload.cityName : "";
        const userChange = actionPayload && actionPayload.userChange ? actionPayload.userChange : false;

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
