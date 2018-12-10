import { SET_INTERACTION_ID } from "../Actions/FormActionTypes";

let initialInteractionId = 0;

export const interactionId = (state = initialInteractionId, action) => {
  switch (action.type) {
    case SET_INTERACTION_ID:
      return action.interactionId;

    default:
      return state;
  }
};
