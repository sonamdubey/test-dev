import { combineReducers } from 'redux-immutable'

import { VehicleDownPayment } from './emiDownPayment'
import { VehicleTenure } from './emiTenure'
import { VehicleInterest } from './emiInterest'

var Emi = combineReducers({
	VehicleDownPayment: VehicleDownPayment,
	VehicleTenure: VehicleTenure,
	VehicleInterest: VehicleInterest
});

module.exports = Emi;
