import { setInfoLog } from "./LogState";
import { setThankYouScreen } from "./ScreenState";
import { leadMappingCampaignStoreToApi } from "../utils/ObjectMapping";
import { submitLead } from "../utils/LeadSubmission";

export const setMLALead = (selectedMLA, modelDetail) => {
  return (dispatch, getState) => {
    let state = getState();
    let lead = leadMappingCampaignStoreToApi(state, selectedMLA, modelDetail);

    submitLead(lead);

    dispatch(
      setInfoLog("Leaving MLAScreen (Current Store Provided in currentState)")
    );

    dispatch(setThankYouScreen());
  };
};
