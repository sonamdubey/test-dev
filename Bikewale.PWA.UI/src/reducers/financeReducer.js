import { combineReducers } from 'redux-immutable'

import { SelectBikePopup } from './selectBikePopupReducer'
import { FinanceCityPopup } from './financeCityPopupReducer'
import { VehicleDownPayment } from './emiDownPayment'
import { VehicleTenure } from './emiTenure'
import { VehicleInterest } from './emiInterest'

var Finance = combineReducers({
	SelectBikePopup: SelectBikePopup,
	FinanceCityPopup: FinanceCityPopup,
	VehicleDownPayment: VehicleDownPayment,
	VehicleTenure: VehicleTenure,
	VehicleInterest: VehicleInterest
});

module.exports = Finance;
