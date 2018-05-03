import { combineReducers } from 'redux-immutable'

import { SelectBikePopup } from './selectBikePopupReducer'
import { FinanceCityPopup } from './financeCityPopupReducer'

var Finance = combineReducers({
	SelectBikePopup: SelectBikePopup,
	FinanceCityPopup: FinanceCityPopup
});

module.exports = Finance;
