import {
  SELECT_CITY,
  SELECT_AREA,
  SELECT_LATLONG,
  FETCHING_LOCATION,
  SELECT_AREA_CITIES,
  AUTODETECT_LOCATION,
  SET_LOCATION,
  PREFILL_LOCATION
} from "../actionTypes";
import Cookies from "js-cookie";

let cookieDefaults = {
	'_CustCityMaster': 'Select City',
	'_CustAreaId': -1,
	'_CustCityIdMaster': -1,
	'_CustAreaName': 'Select Area',
	'_CustZoneIdMaster': '',
	'_CustZoneMaster': 'Select Zone'
};

let latLongDefaults = {
	'_CustLatitude': -100,
	'_CustLongitude': -200
}

let existingCookies = {
	'_CustCityMaster': Cookies.get('_CustCityMaster') || 'Select City',
	'_CustAreaId': parseInt(Cookies.get('_CustAreaId')) || -1,
	'_CustCityIdMaster': parseInt(Cookies.get('_CustCityIdMaster')) || -1,
	'_CustAreaName': Cookies.get('_CustAreaName') || 'Select Area',
	'_CustZoneIdMaster': parseInt(Cookies.get('_CustZoneIdMaster')) || '',
	'_CustZoneMaster': Cookies.get('_CustZoneMaster') || 'Select Zone',
	'_CustLatitude': Cookies.get('_CustLatitude') || latLongDefaults["_CustLatitude"],
	'_CustLongitude': Cookies.get('_CustLongitude') || latLongDefaults["_CustLongitude"]
}

let initialCityState = {
  cityId: parseInt(Cookies.get("_CustCityIdMaster")) || -1,
  cityName: Cookies.get("_CustCityMaster") || "",
  userConfirmed: false,
	autoDetect: false,
	isFetching: false,
	areaId: parseInt(Cookies.get('_CustAreaId')) || -1,
	areaName: Cookies.get('_CustAreaName') || '',
	latitude: Cookies.get('_CustLatitude') || latLongDefaults["_CustLatitude"],
	longitude: Cookies.get('_CustLongitude') || latLongDefaults["_CustLongitude"],
	areaCities: []
}
let setCookiesFromObject = (cookieObj) => {
	if (cookieObj) {
		Object.keys(cookieObj).forEach(key => Cookies.set(key, cookieObj[key], { expires: 180, domain: COOKIE_DOMAIN }))
	}
}
export const location = (state = initialCityState, action) => {
	let newCookies;

	switch (action.type) {

		case SELECT_CITY:
			newCookies = state.cityId === action.cityId ? {
				...existingCookies,
			} : {
					...cookieDefaults,
					...latLongDefaults,
					'_CustCityIdMaster': action.cityId,
					'_CustCityMaster': action.cityName
				}
			setCookiesFromObject(newCookies);

			return {
				...state,
				cityId: action.cityId,
				cityName: action.cityName,
				cityMaskingName: action.cityMaskingName,
				userConfirmed: true,
				areaId: -1,
				areaName: 'Select Area',
				latitude: -100,
				longitude: -200
			}

		case SELECT_AREA:
			newCookies = state.areaId === action.areaId ? "" :
				{
					...latLongDefaults,
					'_CustAreaId': action.areaId,
					'_CustAreaName': action.areaName
				}
			setCookiesFromObject(newCookies);
			return {
				...state,
				areaId: action.areaId,
				areaName: action.areaName,
				latitude: -100,
				longitude: -200
			}
		case SELECT_LATLONG:
			newCookies = {
				'_CustLatitude': action.latitude,
				'_CustLongitude': action.longitude
			}
			setCookiesFromObject(newCookies);
			return {
				...state,
				autoDetect: action.autoDetect ? action.autoDetect : state.autoDetect,
				latitude: action.latitude,
				longitude: action.longitude
			}
		case FETCHING_LOCATION:
			return {
				...state,
				isFetching: action.isFetching
			}

		case SELECT_AREA_CITIES:
			return {
				...state,
				areaCities: action.areaCities
			}

		case AUTODETECT_LOCATION:
			newCookies = {
				'_CustCityIdMaster': action.payload.cityId,
				'_CustCityMaster': action.payload.cityName,
				'_CustAreaId': action.payload.areaId,
				'_CustAreaName': action.payload.areaName,
				'_CustLatitude': action.payload.latitude,
				'_CustLongitude': action.payload.longitude,
				'_CustZoneIdMaster': action.payload.zoneId,
				'_CustZoneMaster': action.payload.zoneName
			}
			setCookiesFromObject(newCookies);
			return {
				...state,
				cityId: action.payload.cityId,
				cityName: action.payload.cityName,
				cityMaskingName: action.payload.cityMaskingName,
				areaId: action.payload.areaId,
				areaName: action.payload.areaName,
				latitude: action.payload.latitude,
				longitude: action.payload.longitude,
				autoDetect: action.payload.autoDetect,
			}
		case PREFILL_LOCATION:
			return {
				cityId: action.location.cityId,
				cityName: action.location.cityName,
				areaId: action.location.areaId ? action.location.areaId : -1,
				areaName: action.location.areaName ? action.location.areaName : "Select Area",
				userConfirmed: false
			};
		case SET_LOCATION:
			existingCookies =
				existingCookies._CustCityIdMaster === action.location.cityId
					? {
						...existingCookies
					}
					: {
						...cookieDefaults,
						_CustCityIdMaster: action.location.cityId,
						_CustCityMaster: action.location.cityName
					};
			if (action.location.areaId > 0) {
				existingCookies = {
					...existingCookies,
					_CustAreaId: action.location.areaId,
					_CustAreaName: action.location.areaName
				};
			}
			Object.keys(existingCookies).forEach(key =>
				Cookies.set(key, existingCookies[key], {
					expires: 180,
					domain: COOKIE_DOMAIN
				})
			);
			return {
				...state,
				cityId: action.location.cityId,
				cityName: action.location.cityName,
				areaId: action.location.areaId ? action.location.areaId : -1,
				areaName: action.location.areaName
					? action.location.areaName
					: "Select Area"
			};
		default:
			return state;
	}
};
