import { SET_INFO_LOG } from "../../../src/LeadForm/Actions/LogActionTypes";
import { setRecoLead } from "../../../src/LeadForm/ActionCreators/RecoState";
import * as LeadSubmission from "../../../src/LeadForm/utils/LeadSubmission";
import { SET_THANKYOU_SCREEN } from "../../../src/LeadForm/Actions/ScreenActionTypes";
import * as ObjectMapping from "../../../src/LeadForm/utils/ObjectMapping";
import { logLevel } from "../../../src/LeadForm/Enum/LogLevel";

//mocking lead submission api module
jest.mock("../../../src/LeadForm/utils/LeadSubmission", () => {
  return {
    submitLead: jest.fn()
  };
});

jest.mock("../../../src/LeadForm/utils/ObjectMapping", () => {
  return {
    leadMappingCarDetailStoreToApi: jest.fn(() => {
      return "lead";
    })
  };
});

jest.mock("../../../src/LeadForm/utils/StoreData", () => {
  return {
    getStateFromStore: jest.fn(() => ({}))
  };
});

let store, storeState;

describe("Testing RecoState Actions", () => {
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

  test("Lead Data mapped - setRecoLead called", () => {
    store.dispatch(setRecoLead("selectedReco"));
    expect(ObjectMapping.leadMappingCarDetailStoreToApi).toHaveBeenCalledWith(
      storeState,
      "selectedReco"
    );
  });

  test("submitLead executed - setRecoLead Called", () => {
    store.dispatch(setRecoLead({}));
    expect(LeadSubmission.submitLead).toHaveBeenCalledWith("lead");
  });

  test("setThankYouScreen executed - setRecoLead Called", () => {
    store.dispatch(setRecoLead({}));
    const actions = store.getActions();
    const setScreenAction = {
      type: SET_THANKYOU_SCREEN
    };

    expect(actions).toContainEqual(setScreenAction);
  });

  test("Log 'Leaving MLAScreen' - setRecoLead Called", () => {
    store.dispatch(setRecoLead({}));
    const actions = store.getActions();
    const setLogAction = {
      type: SET_INFO_LOG,
      logData: {
        message: "Leaving RecoScreen (Current Store Provided in currentState)",
        level: logLevel.INFO,
        currentState: {},
        interactionId: 1
      }
    };

    expect(actions).toContainEqual(setLogAction);
  });
});
