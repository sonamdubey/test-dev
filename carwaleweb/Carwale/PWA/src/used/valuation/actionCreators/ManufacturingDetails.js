import {
	SELECT_MANUFACTURING_DETAILS
} from '../actionTypes'

import {clearCarSelection} from '../actionCreators/Car'
import {DEFAULT_MANUFACTURING_YEAR } from '../constants/index';
const manufacturingDetailsSelection = date => {
	return {
		type: SELECT_MANUFACTURING_DETAILS,
		date
	}
}


export const selectManufacturingDetails = date => (dispatch,getState) => {
	const state = getState().usedCar.valuation.manufacturingDetails;
	dispatch(manufacturingDetailsSelection(date));
	const newDate = date.split('-');
	if(newDate.length && newDate[0] &&  state.year !== DEFAULT_MANUFACTURING_YEAR && state.year !== Number.parseInt(newDate[0])){
		dispatch(clearCarSelection());
	}
}
