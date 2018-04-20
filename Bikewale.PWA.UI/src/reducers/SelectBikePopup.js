import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { selectBikePopupAction } from '../actionTypes/actionTypes'

var initialState = fromJS({
	open: false
})

function SelectBikePopup(state, action) {
	try {
		if (!state)
			return initialState;

		switch (action.type) {
			case selectBikePopupAction.OPEN:
				return fromJS({
					open: true
				})

			default:
				return state
		}
	}
	catch(err) {
		console.log(err)
		return state;
	}
}

var Finance = combineReducers({
	SelectBikePopup: SelectBikePopup
});

module.exports = Finance;
