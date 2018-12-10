import { SET_OTHERS } from "../Actions/FormActionTypes";

export function setOthers(others) {
  return {
    type: SET_OTHERS,
    others
  };
}
