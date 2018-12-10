import { SET_BUYER_DATA } from "../Actions/FormActionTypes";
import { setInfoLog } from "./LogState";
import { setScreen } from "./ScreenState";
import { setEmail } from "./ThankYouState";
import { leadMappingBuyerStoreToApi } from "../utils/ObjectMapping";
import { submitLead } from "../utils/LeadSubmission";
import { SetLocation } from "./Location";

export function setBuyer(buyerInfo) {
  return {
    type: SET_BUYER_DATA,
    buyerInfo
  };
}

export const setFormLead = formScreenState => {
  const { buyerInfo } = formScreenState;

  return (dispatch, getState) => {
    let state = getState();
    let lead = leadMappingBuyerStoreToApi(state, formScreenState);
    let action = "FormOpen-UnSuccessful_Submit";

    submitLead(lead, action);
    dispatch(setBuyer(buyerInfo));

    if (formScreenState.showLocation) {
      let location = {
        cityId: formScreenState.cityId,
        cityName: formScreenState.cityName
      };
      dispatch(SetLocation(location));
    }

    if (buyerInfo.email != "") {
      dispatch(setEmail(buyerInfo.email));
    }

    dispatch(
      setInfoLog("Leaving FormScreen (Current Store Provided in currentState)")
    );

    dispatch(setScreen());
  };
};
