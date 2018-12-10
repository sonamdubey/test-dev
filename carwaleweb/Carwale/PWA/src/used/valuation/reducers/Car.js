import {
	combineReducers
} from 'redux'
import {
	SHOW_CAR_POPUP,
	SET_MAKE_DATA,
	REQUEST_MAKE_DATA,
	SELECT_MAKE,
	REQUEST_MODEL_DATA,
	SET_MODEL_DATA,
	SELECT_MODEL,
	CHANGE_CAR_STATE,
	REQUEST_VERSION_DATA,
	SET_VERSION_DATA,
	SELECT_VERSION,
	APPLY_CAR_FILTER,
	VALIDATE_FORM,
	CAR_VALIDATION_STATE,
	CLEAR_CAR_SELECTION_DATA,
	RESET_CAR_SELECTED_DATA,
	RESET_INITIAL_STATE
} from '../actionTypes'
import {
	validateCarDetails
} from '../utils/validations'
import { isUndefined } from 'util';
import { SIGBREAK } from 'constants';
// initial type to be selected
let initialState = {
	type: "make",
	status: 1,
	active: false
}
// initial state before selecting any car
let initialSelectedState = {
	isValid: true,
	errorText: "",
	make: {
		id: "",
		name: ""
	},
	model: {
		id: "",
		name: "",
		rootName: ""
	},
	version: {
		id: "",
		name: ""
	}
}

// car data
let initialDataState = {
	make: {
		title: "Make",
		isFetching: false,
		isFilterApplied: false,
		filteredData: [],
		popular: [],
		all: []
	},
	model: {
		title: "Model",
		isFetching: false,
		isFilterApplied: false,
		filteredData: [],
		data: []
	},
	version: {
		title: "Version",
		isFetching: false,
		isFilterApplied: false,
		filteredData: [],
		data: []
	}
}

const state = (state = initialState, action) => {
	switch (action.type) {
		case SHOW_CAR_POPUP:
			return {
				...state,
				active: action.value.active,
				status: action.value.status,
				type: action.value.type
			}
			// used to hide and display car selection popup
		case CHANGE_CAR_STATE:
			return {
				...state,
				status: typeof action.value.status !== 'undefined' ? action.value.status : state.status,
				type: typeof action.value.type !== 'undefined' ? action.value.type : state.type,
				active: typeof action.value.active !== 'undefined' ? action.value.active : state.active
			}
		default:
			return state;
	}
}
// store selected make, model and version by the user
const selected = (state = initialSelectedState, action) => {
	let newData = {};
	if (action.value) {
		newData = {
			id: action.value.id,
			name: action.value.name
		};
	}
	switch (action.type) {
		case SELECT_MAKE:
			return {
				...state,
				isValid: true,
				errorText: '',
				make: newData,
				// reset model and version, since make is changed by the user
				model: {
					id: "",
					name: ""
				},
				version: {
					id: "",
					version: ""
				}
			}
		case SELECT_MODEL:
			newData["rootName"] = action.value.rootName
			return {
				...state,
				isValid: true,
				errorText: '',
				model: newData,
				// reset version, since model has been changed by the user
				version: {
					id: "",
					name: ""
				}
			}
		case SELECT_VERSION:
			return {
				...state,
				isValid: true,
				errorText: '',
				version: newData
			}
		case VALIDATE_FORM:
			let status = validateCarDetails(state);
			return {
				...state,
				isValid: status.isValid,
				errorText: status.errorText
			}
		case CAR_VALIDATION_STATE:
			return {
				...state,
				isValid: action.value.isValid,
				errorText: action.value.errorText
			}
		default:
			return state
	}
}

const data = (state = initialDataState, action) => {
	let requestData = [];
	switch (action.type) {
		case REQUEST_MAKE_DATA:
			return {
				...state,
				make: {
					...state.make,
					isFetching: true
				}
			}
		case REQUEST_MODEL_DATA:
			return {
				...state,
				model: {
					...state.model,
					isFetching: true
				}
			}
		case REQUEST_VERSION_DATA:
			return {
				...state,
				version: {
					...state.version,
					isFetching: true
				}
			}
		case SET_MAKE_DATA:
			requestData = action.value.data;
			let popularMakes = [],
				allMakes = [];
			if (requestData.popularMakes && requestData.popularMakes.length) {
				requestData.popularMakes.map((item) => {
					popularMakes.push({
						id: item.makeId,
						name: item.makeName
					})
				})
			}
			if (requestData.otherMakes && requestData.otherMakes.length) {
				requestData.otherMakes.map((item) => {
					allMakes.push({
						id: item.makeId,
						name: item.makeName
					});
				});
			}
			return {
				...state,
				make: {
					...state.make,
					isFetching: false,
					all: allMakes,
					popular: popularMakes
				}
			}
		case SET_MODEL_DATA:
			requestData = action.value.data;
			let models = [];
			if (requestData && requestData.length) {
				requestData.map((item) => {
					models.push({
						id: item.ModelId,
						name: item.ModelName,
						rootName: item.RootName
					});
				})
			}
			return {
				...state,
				model: {
					...state.model,
					data: models,
					isFetching: false,
					filteredData: [],
					isFilterApplied: false
				}
			}
		case SET_VERSION_DATA:
			requestData = action.value.data;
			let versions = [];
			if (requestData && requestData.length) {
				requestData.map((item) => {
					versions.push({
						id: item.ID,
						name: item.Name
					});
				})
			}
			return {
				...state,
				version: {
					...state.version,
					data: versions,
					isFetching: false,
					filteredData: [],
					isFilterApplied: false
				}
			}
		case APPLY_CAR_FILTER:
			const dataKey = action.value.type
			let dataObj = state[dataKey];
			dataObj.isFilterApplied = action.value.isFilterApplied
			if (action.value.isFilterApplied) {
				dataObj.filteredData = getFilteredData(state, action.value);
			}
			return {
				...state,
				[dataKey]: {...dataObj}
			}
		default:
			return state
	}
}
const getFilteredData = (state, value) => {
	let filteredData = [];
	let arrToFilter = [];
	switch (value.type) {
		case "make":
			arrToFilter = state[value.type].popular.concat(state[value.type].all);
			break;
		case "model":
		case "version":
			arrToFilter = state[value.type].data;
			break;
		default:
			return;
	}
	let pattern = new RegExp(value.data, 'i');
	filteredData = arrToFilter.filter(function (curr) {
		return pattern.test(curr.name);
	});
	return filteredData;
}

export const car = (state, action) => {
	if(action.type == CLEAR_CAR_SELECTION_DATA ||
		action.type == RESET_CAR_SELECTED_DATA ||
		action.type == RESET_INITIAL_STATE){
		state = undefined
	}
	return carRoot(state, action)
}

const carRoot = combineReducers({
	state,
	selected,
	data
})
