import {
    SHOW_EMI_POPUP,
    HIDE_EMI_POPUP
} from '../actionTypes'

const showEmiPopup = () => {
	return {
		type: SHOW_EMI_POPUP,
	}
}

const hideEmiPopup = () => {
	return {
		type: HIDE_EMI_POPUP,
	}
}

export const showEmiPopupState = () => (dispatch, state) => {
    dispatch(showEmiPopup());
}
export const hideEmiPopupState = () => (dispatch, state) => {
    dispatch(hideEmiPopup());
	if (window.EmiCalculator.setEMIModelResult) {
		window.EmiCalculator.setEMIModelResult();
	}
}
