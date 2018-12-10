import { SET_INFO_LOG } from "../../../src/LeadForm/Actions/LogActionTypes";
import { setMLALead } from "../../../src/LeadForm/ActionCreators/MLAState";
import * as LeadSubmission from "../../../src/LeadForm/utils/LeadSubmission";
import { SET_THANKYOU_SCREEN } from "../../../src/LeadForm/Actions/ScreenActionTypes";
import * as ObjectMapping from "../../../src/LeadForm/utils/ObjectMapping";
import { logLevel } from "../../../src/LeadForm/Enum/LogLevel";

//mocking lead submission module
jest.mock("../../../src/LeadForm/utils/LeadSubmission", () => {
  return {
    submitLead: jest.fn()
  };
});

jest.mock("../../../src/LeadForm/utils/ObjectMapping", () => {
  return {
    leadMappingCampaignStoreToApi: jest.fn(() => {
      return "lead";
    })
  };
});

jest.mock("../../../src/LeadForm/utils/StoreData", () => {
  return {
    getStateFromStore: jest.fn(() => ({}))
  };
});

let store;
let storeState;

describe("Testing MLAState Actions", () => {
  beforeEach(() => {
    storeState = {
      page: {
        page: {
          name: ""
        },
        platform: {
          name: ""
        }
      },
      interactionId: 1
    };

    //mocking store
    store = mockStore(storeState);
  });

  test("Lead Data mapped - setMLALead called", () => {
    store.dispatch(setMLALead("selectedMLA", "modelDetails"));
    expect(ObjectMapping.leadMappingCampaignStoreToApi).toHaveBeenCalledWith(
      storeState,
      "selectedMLA",
      "modelDetails"
    );
  });

  test("submitLead executed - setMLALead Called", () => {
    store.dispatch(setMLALead({}));
    expect(LeadSubmission.submitLead).toHaveBeenCalledWith("lead");
  });

  test("set ThankYouScreen - setMLALead Called", () => {
    store.dispatch(setMLALead({}));
    const actions = store.getActions();
    const setScreenAction = {
      type: SET_THANKYOU_SCREEN
    };

    expect(actions).toContainEqual(setScreenAction);
  });

  test("Log 'Leaving MLAScreen' - setMLALead Called", () => {
    store.dispatch(setMLALead({}));
    const actions = store.getActions();
    const setLogAction = {
      type: SET_INFO_LOG,
      logData: {
        message: "Leaving MLAScreen (Current Store Provided in currentState)",
        level: logLevel.INFO,
        currentState: {},
        interactionId: 1
      }
    };

    expect(actions).toContainEqual(setLogAction);
  });
});
