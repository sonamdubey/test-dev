import { SET_PROP_ID } from "../Actions/PropIdActionType";

export function setPropId(propId) {
  return {
    type: SET_PROP_ID,
    propId
  };
}
