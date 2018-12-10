import {
  SET_CAMPAIGN,
  SET_CAMPAIGN_MODEL_KEY
} from "../Actions/FormActionTypes";

let initialState = {};

export const campaign = (state = initialState, action) => {
  switch (action.type) {
    //When setBuyer Action Triggered
    case SET_CAMPAIGN: {
      return action.campaign;
    }
    case SET_CAMPAIGN_MODEL_KEY: {
      let key = "LEADFORM_" + action.campaign.modelId;
      sessionStorage.setItem(key, JSON.stringify(action.campaign.campaign));
      return action.campaign.campaign;
    }
    default:
      return state;
  }
};
