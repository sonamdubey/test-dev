import { selectBikePopupAction } from '../actionTypes/actionTypes'

module.exports = {
	openSelectBikePopup: function() {
		return function (dispatch) {
			dispatch({
				type: selectBikePopupAction.OPEN
			})
		}
	}
}
