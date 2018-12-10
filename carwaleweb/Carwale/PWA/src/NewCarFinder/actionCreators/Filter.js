import FilterScreenApi from '../apis/FilterScreenOrder'
import {
    SET_CURRENT_SCREEN, SET_ALL_FILTER_SCREEN
} from '../actionTypes'

export const setCurrentScreen= (screenId) => {
	return {
        type: SET_CURRENT_SCREEN,
        screenId
	}
}

export const setFilterScreenData = (data) => {
	return {
		type: SET_ALL_FILTER_SCREEN,
		data
	}
}
export const fetchFilterScreen = () => (dispatch, getState) => {
	FilterScreenApi
		.get()
		.then(data => { setTimeout(() => dispatch(setFilterScreenData(data)), 300) })
		.catch( error => {
			//TODO: how should error affect the state ?
			console.log(error)
		})
}
