import { SET_CITY_CHANGE } from "../Actions/IsCityChanged";

let initialState = false;

export const isCityChanged = (state = initialState, action) => {
  switch (action.type) {
    case SET_CITY_CHANGE:
      return action.isCityChanged;
    default:
      return state;
  }
};
