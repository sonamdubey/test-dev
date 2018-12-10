import {
	SELECT_CITY,
	SELECT_AREA,
	SELECT_LATLONG,
	FETCHING_LOCATION,
	SELECT_AREA_CITIES,
	AUTODETECT_LOCATION
} from '../actionTypes'
import { initToast } from '../actionCreators/Toast'

const get = (url) => {
	let fetchUrl = url
	return fetch(fetchUrl, { headers: { sourceId: "43" } })
		.then(response => {
			if (!response.ok) {
				throw response
			}
			return response.json();
		})
}

export const setCity = ({ cityId, cityName, cityMaskingName, isConfirmBtnClicked = false }) => {
	return {
		type: SELECT_CITY,
		cityId,
		cityName,
		cityMaskingName,
		isConfirmBtnClicked
	}
}

export const setArea = ({ areaId, areaName }) => {
	return {
		type: SELECT_AREA,
		areaId,
		areaName,
	}
}

export const setLatLong = ({ latitude, longitude, autoDetect }) => {
	return {
		type: SELECT_LATLONG,
		latitude,
		longitude,
		autoDetect
	}
}
export const selectAreaCities = (areaCities) => {
	return {
		type: SELECT_AREA_CITIES,
		areaCities
	}
}
export const setAutoDetectLocation = (payload) => {
	return {
		type: AUTODETECT_LOCATION,
		payload
	}
}
const processAutoDetect = (position, dispatch) => {
	dispatch(setIsFetching(true))
	get('/api/locations/nearest/?latitude=' + position.coords.latitude + '&longitude=' + position.coords.longitude)
		.then(response => {
			if (response && response.length) {
				dispatch(setAutoDetectLocation({ ...response[0], ...position, autoDetect: true }));//need both response and position coord
			}
			dispatch(setIsFetching(false))
		})
		.catch(error => {
			errorprocessAutoDetect(dispatch);
		})
}
const errorprocessAutoDetect = (dispatch) => {
	dispatch(setIsFetching(false))
	dispatch(setLatLong({ autoDetect: false }))
	dispatch(initToast({
		message: 'Your location was not found!'
	}))
}
const setIsFetching = (isFetching) => {
	return {
		type: FETCHING_LOCATION,
		isFetching
	}
}
export const detectLocation = (pos) => (dispatch) => {
	if (pos) {
		processAutoDetect(pos, dispatch);
		return;
	}
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(function (position) { processAutoDetect(position, dispatch) }, function (err) { errorprocessAutoDetect(dispatch) });
	} else {
		errorprocessAutoDetect(dispatch)
	}
}
