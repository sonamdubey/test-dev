import { campaign } from "../../../src/LeadForm/Reducers/campaign";
import {
  SET_CAMPAIGN,
  SET_CAMPAIGN_MODEL_KEY
} from "../../../src/LeadForm/Actions/FormActionTypes";

describe("set reducer campaign action", () => {
  const actionSetCampaign = () => ({
    type: SET_CAMPAIGN,
    campaign: {
      id: 1569,
      contactName: "Regent Honda Thane",
      mutualLeads: true,
      cvlDetails: { isCvl: false },
      isEmailRequired: false,
      isTestDriveCampaign: false,
      leadPanel: 2,
      predictionData: null,
      type: 0,
      dealerAdminId: 0
    }
  });

  const actionSetCampaignModel = () => ({
    type: SET_CAMPAIGN_MODEL_KEY,
    campaign: {
      campaign: {
        id: 1569,
        contactName: "Regent Honda Thane",
        mutualLeads: true,
        cvlDetails: { isCvl: false },
        isEmailRequired: false,
        isTestDriveCampaign: false,
        leadPanel: 2,
        predictionData: null,
        type: 0,
        dealerAdminId: 0
      },
      modelId: 114
    }
  });

  const state = {};

  it("set campaign", () => {
    let campaignReducerOutput = campaign(state, actionSetCampaign());
    expect(campaignReducerOutput).toEqual(actionSetCampaign().campaign);
  });

  it("set campaignModel", () => {
    let campaignReducerOutput = campaign(state, actionSetCampaignModel());
    expect(campaignReducerOutput).toEqual(
      actionSetCampaignModel().campaign.campaign
    );
  });
});
