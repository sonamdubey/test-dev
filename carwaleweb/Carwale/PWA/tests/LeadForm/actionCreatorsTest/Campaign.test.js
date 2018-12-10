import {
  setCampaign,
  setCampaignModelKey
} from "../../../src/LeadForm/ActionCreators/Campaign";
import {
  SET_CAMPAIGN,
  SET_CAMPAIGN_MODEL_KEY
} from "../../../src/LeadForm/Actions/FormActionTypes";

let store;

beforeEach(() => {
  store = mockStore({});
});

describe("Testing Campaign action creator", () => {
  it("SetCampaignModelKey action fired", () => {
    let campaign = {
      campaign: {},
      modelId: 123
    };
    const action = { type: SET_CAMPAIGN_MODEL_KEY, campaign };
    store.dispatch(setCampaignModelKey({}, 123));
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  it("fire a set campaign action", () => {
    const action = {
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
    };
    store.dispatch(setCampaign(action.campaign));
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });
});
