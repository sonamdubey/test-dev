import { SET_BUYER_DATA } from "../../../src/LeadForm/Actions/FormActionTypes";
import { SET_INFO_LOG } from "../../../src/LeadForm/Actions/LogActionTypes";
import { setWhatsAppFormLead } from "../../../src/LeadForm/ActionCreators/WhatsAppFormState";
import * as LeadSubmission from "../../../src/LeadForm/utils/LeadSubmission";
import * as ObjectMapping from "../../../src/LeadForm/utils/ObjectMapping";
import { platform } from "../../../src/enum/Platform";
import { SET_THANKYOU_SCREEN } from "../../../src/LeadForm/Actions/ScreenActionTypes";

jest.mock("../../../src/LeadForm/utils/LeadSubmission", () => {
  return {
    submitLead: jest.fn()
  };
});

jest.mock("../../../src/LeadForm/utils/ObjectMapping", () => {
  return {
    leadMappingBuyerStoreToApi: jest.fn(() => {
      return "lead";
    })
  };
});

//mocking StoreData module
jest.mock("../../../src/LeadForm/utils/StoreData", () => {
  return {
    getStateFromStore: jest.fn(() => ({}))
  };
});

//mocking setThankYouScreen
jest.mock("../../../src/LeadForm/ActionCreators/ScreenState", () => {
  return {
    setThankYouScreen: jest.fn(() => ({ type: "SET_THANKYOU_SCREEN" }))
  };
});

let store, storeState, FormScreenState;

describe("Testing WhatsAppFormState Actions", () => {
  FormScreenState = {
    buyerInfo: {
      name: "Ritesh",
      mobile: "8108378233",
      email: ""
    },
    citySelectItems: [],
    dealerSelectItems: [
      {
        id: 7109,
        name: "Agol - Renault Karnavati",
        area: "380058",
        distance: 0
      }
    ],
    showLocation: true,
    showSeller: false,
    MLAList: []
  };

  beforeEach(() => {
    //mocking store state
    storeState = {
      leadClickSource: {
        propId: 1,
        page: {
          platform: {
            id: platform.DESKTOP.id,
            name: platform.DESKTOP.name
          },
          page: {
            name: ""
          }
        },
        isCitySet: false
      },
      interactionId: 1,
      NC: {
        campaign: {
          featuredCarData: {
            makeName: "Honda",
            modelName: "City"
          }
        }
      },
      location: {
        cityName: "Indore"
      }
    };

    //mocking store
    store = mockStore(storeState);
  });

  test("Lead Data mapped - setWhatsAppFormLead called", () => {
    store.dispatch(setWhatsAppFormLead(FormScreenState));
    expect(ObjectMapping.leadMappingBuyerStoreToApi).toHaveBeenCalledWith(
      storeState,
      FormScreenState
    );
  });

  test("submitLead executed - setWhatsAppFormLead Called", () => {
    store.dispatch(setWhatsAppFormLead(FormScreenState));
    expect(LeadSubmission.submitLead).toHaveBeenCalledWith(
      "lead",
      "WhatsAppFormOpen-UnSuccessful_Submit"
    );
  });

  test("setThankYouScreen executed  - setWhatsAppFormLead Called", () => {
    store.dispatch(setWhatsAppFormLead(FormScreenState));
    const actions = store.getActions();
    const setScreenAction = {
      type: SET_THANKYOU_SCREEN
    };

    expect(actions).toContainEqual(setScreenAction);
  });

  test("SetBuyer Executed - setWhatsAppFormLead Called", () => {
    store.dispatch(setWhatsAppFormLead(FormScreenState));
    const actions = store.getActions();
    const setBuyerAction = {
      type: SET_BUYER_DATA,
      buyerInfo: FormScreenState.buyerInfo
    };

    expect(actions).toContainEqual(setBuyerAction);
  });

  test("Logged 'Leaving WhatsAppFormScreen' - setWhatsAppFormLead Called", () => {
    store.dispatch(setWhatsAppFormLead(FormScreenState));
    const actions = store.getActions();
    const setLogAction = {
      type: SET_INFO_LOG,
      logData: {
        message:
          "Leaving WhatsAppFormScreen (Current Store Provided in currentState)",
        level: "INFO",
        currentState: {},
        interactionId: 1
      }
    };

    expect(actions).toContainEqual(setLogAction);
  });
});
