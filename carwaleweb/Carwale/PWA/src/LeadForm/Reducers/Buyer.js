import { SET_EMAIL, SET_BUYER_DATA } from "../Actions/FormActionTypes";
import { storage } from "../../utils/Storage";

const getName = storage.getValue("LeadForm.BuyerInfo.Name");
const getMobile = storage.getValue("LeadForm.BuyerInfo.Mobile");
const getEmail = storage.getValue("LeadForm.BuyerInfo.Email");

const initialBuyerState = {
  name: getName == undefined ? "" : getName,
  mobile: getMobile == undefined ? "" : getMobile,
  email: getEmail == undefined ? "" : getEmail
};

export const buyerInfo = (state = initialBuyerState, action) => {
  switch (action.type) {
    //When setBuyer Action Triggered
    case SET_BUYER_DATA: {
      if (action.buyerInfo == null || action.buyerInfo == undefined)
        throw "Buyer {BuyerInfo} action.BuyerInfo input is null or undefined";

      storage.setValue("LeadForm.BuyerInfo.Name", action.buyerInfo.name);
      storage.setValue("LeadForm.BuyerInfo.Mobile", action.buyerInfo.mobile);
      return {
        ...state,
        name: action.buyerInfo.name,
        mobile: action.buyerInfo.mobile
      };
    }

    //When SetEmail Action Triggered
    case SET_EMAIL: {
      if (action.email == null || action.email == undefined)
        throw "Buyer {BuyerInfo} action.email input is null or undefined";

      storage.setValue("LeadForm.BuyerInfo.Email", action.email);
      return {
        ...state,
        email: action.email
      };
    }

    default:
      return state;
  }
};
