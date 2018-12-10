import { SET_LEAD_SOURCE_CITY } from "../Actions/IsCitySetActionType";

let initialState = true;

export const isCitySet = (state = initialState, action) => {
  switch (action.type) {
    case SET_LEAD_SOURCE_CITY:
      return action.isCitySet;

    default:
      return state;
  }
};
