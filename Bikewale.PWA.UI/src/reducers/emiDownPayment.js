import { combineReducers } from 'redux-immutable'
import { fromJS, toJS } from 'immutable'

import { emiCalculatorAction } from '../actionTypes/emiActionTypes'
import {createSnapPointsWithBoundaryValues} from '../utils/rheostat/function/DiffSnapPoints'

const initialSliderState = fromJS({
	min: 0,
	max: Number.MAX_SAFE_INTEGER,
	values: [0],
	onRoadPrice: 0,
	userChange: false,
	sliderTitleRight: 'On-Road Price'
})

const slider = (state = initialSliderState, action) => {
	if (!state)
		return initialSliderState;

	switch (action.type) {
		case emiCalculatorAction.UPDATE_DOWNPAYMENT_SLIDER_VALUE:
			return state.setIn(['values'], fromJS(action.values)).setIn(['userChange'], action.userChange);
		case emiCalculatorAction.OPEN_EMICALCULATOR:
			let payload = action.payload
			let downPayment = null;
			let currentDp = state.getIn(['values']).toJS()[0]
			if(currentDp > 0){
				let minDp = .1 * payload.onRoadPrice
				let maxDp = .4 * payload.onRoadPrice
				downPayment = currentDp >= minDp && currentDp <= maxDp ? currentDp : currentDp < minDp ? minDp : maxDp
			}
			else{
				downPayment = payload.values[0]
			}
			return fromJS({
				...state.toJS(),
				snapPoints: createSnapPointsWithBoundaryValues({ startPoint: payload.min, endPoint: payload.max, divisions: 100 }),
				values: [parseInt(downPayment)],
				min: payload.min,
				max: payload.max,
				onRoadPrice: payload.onRoadPrice,
				userChange: false
			});
		default:
			return state;
	}
}

export const VehicleDownPayment = combineReducers({
	slider
})
