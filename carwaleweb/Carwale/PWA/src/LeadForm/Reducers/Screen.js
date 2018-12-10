import {
  SET_MLA_SCREEN,
  SET_RECOMMENDATION_SCREEN,
  SET_THANKYOU_SCREEN,
  SET_SORRY_SCREEN,
  SET_FORM_SCREEN,
  SET_LOCATION_SCREEN,
  SET_WHATSAPP_FORM_SCREEN
} from "../Actions/ScreenActionTypes";
import { screenType } from "../Enum/ScreenEnum";

let initialScreenState = {
  screen: screenType.Hide
};

export const screen = (state = initialScreenState, action) => {
  switch (action.type) {
    case SET_LOCATION_SCREEN:
      return {
        screen: screenType.Location
      };
    //When SetMLAScreen Action Triggered
    case SET_MLA_SCREEN:
      return {
        screen: screenType.MLA
      };

    //When SetRecoScreen Action Triggered
    case SET_RECOMMENDATION_SCREEN:
      return {
        screen: screenType.Reco
      };

    case SET_FORM_SCREEN:
      return {
        screen: screenType.Form
      };

    case SET_WHATSAPP_FORM_SCREEN:
      return {
        screen: screenType.WhatsAppForm
      };

    //When SetThankYouScreen Action Triggered
    case SET_THANKYOU_SCREEN:
      return {
        screen: screenType.ThankYou
      };

    //When SetSorryScreen Action Triggered
    case SET_SORRY_SCREEN:
      return {
        screen: screenType.Sorry
      };

    default:
      return state;
  }
};
