import { setInfoLog } from "./LogState";
import { setThankYouScreen } from "./ScreenState";
import { leadMappingBuyerStoreToApi } from "../utils/ObjectMapping";
import { submitLead } from "../utils/LeadSubmission";
import { setBuyer } from "./FormState";

export const setWhatsAppFormLead = whatsAppFormScreenState => {
  const { buyerInfo } = whatsAppFormScreenState;

  return (dispatch, getState) => {
    let state = getState();
    let { makeName, modelName } = state.NC.campaign.featuredCarData;
    let dealerWhatsappNumber = state.NC.campaign.dealerWhatsappNumber;
    let cityName = state.location.cityName;
    let lead = leadMappingBuyerStoreToApi(state, whatsAppFormScreenState);
    let action = "WhatsAppFormOpen-UnSuccessful_Submit";

    submitLead(lead, action);

    dispatch(setBuyer(buyerInfo));
    dispatch(
      setInfoLog(
        "Leaving WhatsAppFormScreen (Current Store Provided in currentState)"
      )
    );

    let textMessage = `Hi, I want to enquire about ${makeName} ${modelName} in ${cityName}. Please call me on ${
      buyerInfo.mobile
    }`;
    window.location = `https://api.whatsapp.com/send?phone=91${dealerWhatsappNumber}&text=${textMessage}`;
    dispatch(setThankYouScreen());
  };
};
