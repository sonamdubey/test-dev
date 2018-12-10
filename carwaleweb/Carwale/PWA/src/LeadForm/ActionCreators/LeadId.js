import { SET_LEAD_ID, CLEAR_LEAD_ID } from "../Actions/FormActionTypes";

export function setLeadId(leadId) {
  if (leadId == null || leadId == undefined)
    throw "setLeadId leadId input is null or undefined";
  return {
    type: SET_LEAD_ID,
    leadId
  };
}

export function clearLeadId() {
  return {
    type: CLEAR_LEAD_ID
  };
}
