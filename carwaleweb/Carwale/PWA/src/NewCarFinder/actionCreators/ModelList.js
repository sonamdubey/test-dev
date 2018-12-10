import {
	REQUEST_MODEL_LIST,
	RECEIVE_MODEL_LIST,
	APPEND_MODEL_LIST
} from '../actionTypes'

import NewCarSearchApi from '../apis/NewCarSearch'
import {
	initToast
} from '../../actionCreators/Toast';
import {
	error
} from 'util';

const requestModelList = (pageName, resetList = false) => {
	return {
		type: REQUEST_MODEL_LIST,
		resetList,
		pageName
	}
}

const setModelList = (data, pageName) => {
	return {
		type: RECEIVE_MODEL_LIST,
		data: data.models,
		totalModels: data.totalModels,
		nextPageUrl: data.nextPageUrl ? data.nextPageUrl.split('?')[1] : data.nextPageUrl,
		cityName: data.city ? data.city.name : null,
		cityId: data.city ? data.city.id : null,
		pageName
	}
}

const appendModelList = (data, pageName) => {
	return {
		type: APPEND_MODEL_LIST,
		data: data.models,
		totalModels: data.totalModels,
		nextPageUrl: data.nextPageUrl ? data.nextPageUrl.split('?')[1] : null,
		pageName
	}
}

export const fetchModelList = (options, pageName) => (dispatch, getState) => {
	dispatch(requestModelList(pageName, true))
	NewCarSearchApi
		.getPage(options)
		.then(data => {
			dispatch(setModelList(data, pageName))
			let message
			if (data.totalModels > 1) {
				message = `${data.totalModels} cars found matching your criteria.`
			} else if (data.totalModels === 1) {
				message = `${data.models[0].modelName} is the only car matching your criteria.`
			}
			if (message && pageName == "mainListing") {
				dispatch(initToast({
					message
				}))
			}
		})
		.catch(error => {
			console.log(error)
			dispatch(setModelList({
				models: [],
				totalModels: 0,
				nextPageUrl: null
			}, pageName))
		})
}

export const fetchNextPage = (options, pageName) => (dispatch, getState) => {
	const {
		nextPageUrl
	} = getState().newCarFinder.listingData[pageName]
	if (nextPageUrl) {
		dispatch(requestModelList(pageName, false))
		NewCarSearchApi
			.get(nextPageUrl)
			.then(data => {
				dispatch(appendModelList(data, pageName))
			})
			.catch(error => {
				console.log(error)
				dispatch(appendModelList({
					models: [],
					totalModels: 0,
					nextPageUrl: null
				}, pageName))
			})
	}
}
