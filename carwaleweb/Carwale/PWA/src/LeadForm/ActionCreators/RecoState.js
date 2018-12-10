import { setInfoLog } from "./LogState";
import { setThankYouScreen } from "./ScreenState";
import { leadMappingCarDetailStoreToApi } from "../utils/ObjectMapping";
import { submitLead } from "../utils/LeadSubmission";

export const setRecoLead = selectedReco => {
  return (dispatch, getState) => {
    let state = getState();
    let lead = leadMappingCarDetailStoreToApi(state, selectedReco);

    submitLead(lead);

    dispatch(
      setInfoLog("Leaving RecoScreen (Current Store Provided in currentState)")
    );

    dispatch(setThankYouScreen());
  };
};
