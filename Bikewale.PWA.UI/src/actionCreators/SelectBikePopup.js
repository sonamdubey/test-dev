import { selectBikePopup } from '../actionTypes/SelectBikePopup'

const openBikePopup = () => {
  return {
    type: selectBikePopup.OPEN_BIKE_POPUP
  }
}

const closeBikePopup = () => {
  return {
    type: selectBikePopup.CLOSE_BIKE_POPUP
  }
}

const setModel = () => {
  return {
    type: selectBikePopup.SELECT_MODEL
  }
}

export const openSelectBikePopup = () => (dispatch) => {
  dispatch(openBikePopup())
}

export const closeSelectBikePopup = () => (dispatch) => {
  dispatch(closeBikePopup())
}

export const selectModel = () => (dispatch) => {
  dispatch(setModel())
}
