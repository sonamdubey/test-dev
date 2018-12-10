import {
  SET_CAMPAIGN,
  SET_CAMPAIGN_MODEL_KEY
} from "../Actions/FormActionTypes";

/**
 * Call the Api to post lead data
 * and return corresponding action to reducers
 */
export function setCampaign(campaign) {
  return {
    type: SET_CAMPAIGN,
    campaign
  };
}

export const setCampaignModelKey = (campaign, modelId) => {
  return {
    type: SET_CAMPAIGN_MODEL_KEY,
    campaign: { campaign, modelId }
  };
};
