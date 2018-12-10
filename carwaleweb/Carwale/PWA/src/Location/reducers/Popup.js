import { OPEN_POPUP, CLOSE_POPUP, SHOW_CROSS_ICON, HIDE_CROSS_ICON } from '../actionTypes';
import {
	lockScroll,
	unlockScroll
} from '../../utils/ScrollLock'

const initialPopupState = false;
const initialShouldShowCrossIcon = true;

const isActive = (state = initialPopupState, action) => {
  switch (action.type) {
    case OPEN_POPUP:
      lockScroll()
      return true;

    case CLOSE_POPUP:
      unlockScroll()
      return false;

    default:
      return state;
  }
}
export const shouldShowCrossIcon = (state = initialShouldShowCrossIcon, action) => {
  switch (action.type) {
    case SHOW_CROSS_ICON: return true;
    case HIDE_CROSS_ICON: return false;
    default: return state;
  }
}

export default isActive;
