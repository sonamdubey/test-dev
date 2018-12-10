import { SET_HIDE, SET_SHOW } from "../Actions/ScreenActionTypes";

let initialShowState = false;

export const isLeadFormVisible = (state = initialShowState, action) => {
  switch (action.type) {
    case SET_HIDE:
      return false;
    case SET_SHOW:
      return true;

    default:
      return state;
  }
};
