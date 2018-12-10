import { SET_OTHERS } from "../Actions/FormActionTypes";

let initialOtherState = {};

export const others = (state = initialOtherState, action) => {
  switch (action.type) {
    //When setOthers Action Trigerred
    case SET_OTHERS:
      return action.others;

    default:
      return state;
  }
};
