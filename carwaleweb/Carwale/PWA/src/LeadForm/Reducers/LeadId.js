import { SET_LEAD_ID, CLEAR_LEAD_ID } from "../Actions/FormActionTypes";

let initialLeadIdState = [];

export const leadId = (state = initialLeadIdState, action) => {
  switch (action.type) {
    //When setLeadId Action Trigerred
    case SET_LEAD_ID:
      return [...state, action.leadId];

    case CLEAR_LEAD_ID:
      return [];

    default:
      return state;
  }
};
