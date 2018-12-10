import {
	KILOMETERS_CHANGED,
	VALIDATE_FORM
} from '../actionTypes'

import {
	MIN_KILOMETER_DRIVEN,
	MAX_KILOMETER_DRIVEN
} from '../constants'
import {deserialzeQueryStringToObject} from '../../../utils/Common';

let initialKilometerState = {
	min: MIN_KILOMETER_DRIVEN,
	max: MAX_KILOMETER_DRIVEN,
	value: 0,
	isValid: true,
	errorText: ''
}

const validate = (data) => {
	let errorText = "";
	let isValid = true;
	if (!data.value) {
		isValid = false;
		errorText = "Please select kilometers";
	}
	else if (!(data.value >= data.min && data.value <= data.max)) {
		isValid = false;
		errorText = "Should be below 10 lakh";
	}
	return { errorText: errorText, isValid: isValid }
}

const getInitialKilometerState = () => {
	let {kms} = deserialzeQueryStringToObject(window.location.search);
	if (kms) {
		initialKilometerState.value = parseInt(kms);
	}
	return initialKilometerState;
}
export const kmsDriven = (state = getInitialKilometerState(), action) => {
	switch (action.type) {
		case KILOMETERS_CHANGED:
			return {
				...state,
				value: action.value,
				isValid: true
			}
		case VALIDATE_FORM:
			let status = validate(state);
			return {
				...state,
				isValid: status.isValid,
				errorText: status.errorText
			}
		default:
			return state
	}
}
