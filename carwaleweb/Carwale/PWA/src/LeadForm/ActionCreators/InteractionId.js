import { SET_INTERACTION_ID } from "../Actions/FormActionTypes";

export function setInteractionId(interactionId) {
  return {
    type: SET_INTERACTION_ID,
    interactionId
  };
}
