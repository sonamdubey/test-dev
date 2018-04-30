import { combineReducers } from 'redux-immutable'

import { SelectBikePopup } from './SelectBikePopup'
import { FinanceCityPopup } from './FinanceCityPopup'
import { SimilarBikesEMI } from './SimilarBikesEMI'

var Finance = combineReducers({
	SelectBikePopup: SelectBikePopup,
	FinanceCityPopup: FinanceCityPopup,
	SimilarBikesEMI: SimilarBikesEMI
});

module.exports = Finance;
