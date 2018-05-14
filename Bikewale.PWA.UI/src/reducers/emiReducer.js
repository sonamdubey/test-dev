import { combineReducers } from 'redux-immutable'

import { VehicleDownPayment } from './emiDownPayment'
import { VehicleTenure } from './emiTenure'
import { VehicleInterest } from './emiInterest'
import { VehiclePieData } from './emiPieData'
import { PieAnimation } from './emiPieAnimation'

var Emi = combineReducers({
	VehicleDownPayment: VehicleDownPayment,
	VehicleTenure: VehicleTenure,
	VehicleInterest: VehicleInterest,
	VehiclePieData: VehiclePieData,
	PieAnimation: PieAnimation
});

module.exports = Emi;
