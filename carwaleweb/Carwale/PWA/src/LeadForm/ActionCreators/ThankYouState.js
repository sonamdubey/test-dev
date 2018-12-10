import { SET_EMAIL } from "../Actions/FormActionTypes";
import { setInfoLog } from "./LogState";
import { leadMappingEmailStoreToApi } from "../utils/ObjectMapping";
import { submitLead } from "../utils/LeadSubmission";

export function setEmail(email) {
  return {
    type: SET_EMAIL,
    email
  };
}

export const setThankYouLead = email => {
  return (dispatch, getState) => {
    let state = getState();
    let lead = leadMappingEmailStoreToApi(state, email);
    dispatch(setEmail(email));
    submitLead(lead, false);

    dispatch(
      setInfoLog(
        "Leaving ThankYouScreen (Current Store Provided in currentState)"
      )
    );
  };
};
