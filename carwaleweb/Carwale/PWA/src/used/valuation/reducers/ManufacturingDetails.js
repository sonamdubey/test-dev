import {
	SELECT_MANUFACTURING_DETAILS, VALIDATE_FORM
} from '../actionTypes'
import { MIN_MANUFACTURING_YEAR, MAX_MANUFACTURING_YEAR,DEFAULT_MANUFACTURING_YEAR } from '../constants/index';
import { validateManufacturingDetails } from '../utils/validations'
import {deserialzeQueryStringToObject} from '../../../utils/Common';

let initialManufacturingDetailsState = {
	month: 1,
	year: DEFAULT_MANUFACTURING_YEAR,
	isValid: true,
	minYear: MIN_MANUFACTURING_YEAR,
	maxYear: MAX_MANUFACTURING_YEAR
}

const getInitialManufacturingDetailsState = () => {
	let { month, year } = deserialzeQueryStringToObject(window.location.search)
	if (month && year) {
		initialManufacturingDetailsState.month = parseInt(month);
		initialManufacturingDetailsState.year = parseInt(year);
	}
	return initialManufacturingDetailsState;
}

export const manufacturingDetails = (state = getInitialManufacturingDetailsState(), action) => {
	switch (action.type) {
		case SELECT_MANUFACTURING_DETAILS:
			const date = action.date.split('-');
			return {
				...state,
				month: Number.parseInt(date[1]),
				year: Number.parseInt(date[0]),
				isValid: true
			}
		case VALIDATE_FORM:
			return {
				...state,
				isValid: validateManufacturingDetails(state)
			}
		default:
			return state
	}
}

