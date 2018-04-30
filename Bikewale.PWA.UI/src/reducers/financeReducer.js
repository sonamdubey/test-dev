import { combineReducers } from 'redux-immutable'

import { SelectBikePopup } from './SelectBikePopup'
import { FinanceCityPopup } from './FinanceCityPopup'

var Finance = combineReducers({
	SelectBikePopup: SelectBikePopup,
	FinanceCityPopup: FinanceCityPopup
});

module.exports = Finance;
