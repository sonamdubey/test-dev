import { SET_PROP_ID } from "../Actions/PropIdActionType";

let initialPropId = -1;

export const propId = (state = initialPropId, action) => {
  switch (action.type) {
    case SET_PROP_ID:
      return action.propId;

    default:
      return state;
  }
};
