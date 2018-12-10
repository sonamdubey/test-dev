import { OPEN_POPUP, CLOSE_POPUP, SHOW_CROSS_ICON, HIDE_CROSS_ICON } from '../actionTypes';

export function openPopup() {
  return {
    type: OPEN_POPUP
  }
}

export function closePopup() {
  return {
    type: CLOSE_POPUP
  }
}
export function showCrossIcon() {
  return {
    type: SHOW_CROSS_ICON
  }
}
export function hideCrossIcon() {
  return {
    type: HIDE_CROSS_ICON
  }
}
