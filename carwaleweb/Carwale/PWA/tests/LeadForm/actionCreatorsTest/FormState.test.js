import { SET_BUYER_DATA } from "../../../src/LeadForm/Actions/FormActionTypes";
import { SET_INFO_LOG } from "../../../src/LeadForm/Actions/LogActionTypes";
import { setFormLead } from "../../../src/LeadForm/ActionCreators/FormState";
import * as LeadSubmission from "../../../src/LeadForm/utils/LeadSubmission";
import * as ObjectMapping from "../../../src/LeadForm/utils/ObjectMapping";
import { platform } from "../../../src/enum/Platform";

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
    getStateFromStore: jest.fn(() => {
      return {};
    })
  };
});

//mocking setScreen
jest.mock("../../../src/LeadForm/ActionCreators/ScreenState", () => {
  return {
    setScreen: jest.fn(() => ({ type: "SET_SCREEN" }))
  };
});

//mocking setEmail
jest.mock("../../../src/LeadForm/ActionCreators/ThankYouState", () => {
  return {
    setEmail: jest.fn(() => ({ type: "SET_EMAIL" }))
  };
});

//mocking setLocation
let store, storeState, FormScreenState;
jest.mock("../../../src/Location/actionCreators/index", () => {
  return {
    SetLocation: () => ({ type: "SET_LOCATION" })
  };
});


describe("Testing FormState Actions", () => {
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
    cityId: 0,
    cityName: "",
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
      location: {
        cityId: 0,
        cityName: ""
      }
    };

    //mocking store
    store = mockStore(storeState);
  });

  test("Lead Data mapped - setFormLead called", () => {
    store.dispatch(setFormLead(FormScreenState));
    expect(ObjectMapping.leadMappingBuyerStoreToApi).toHaveBeenCalledWith(
      storeState,
      FormScreenState
    );
  });

  test("submitLead executed - setFormLead Called", () => {
    store.dispatch(setFormLead(FormScreenState));
    expect(LeadSubmission.submitLead).toHaveBeenCalledWith(
      "lead",
      "FormOpen-UnSuccessful_Submit"
    );
  });

  test("SetScreen Executed - setFormLead Called", () => {
    store.dispatch(setFormLead(FormScreenState));
    const actions = store.getActions();
    const setScreenAction = {
      type: "SET_SCREEN"
    };

    expect(actions).toContainEqual(setScreenAction);
  });

  test("SetBuyer Executed - setFormLead called", () => {
    store.dispatch(setFormLead(FormScreenState));
    const actions = store.getActions();
    const setBuyerAction = {
      type: SET_BUYER_DATA,
      buyerInfo: FormScreenState.buyerInfo
    };

    expect(actions).toContainEqual(setBuyerAction);
  });

  test("SetEmail Executed when email is set - setFormLead called", () => {
    FormScreenState = {
      ...FormScreenState,
      buyerInfo: { email: "abc@xyz.com" }
    };
    store.dispatch(setFormLead(FormScreenState));
    const actions = store.getActions();
    const setEmailAction = {
      type: "SET_EMAIL"
    };

    expect(actions).toContainEqual(setEmailAction);
  });

  test("SetEmail not Executed when email is not set - setFormLead called", () => {
    FormScreenState = { ...FormScreenState, buyerInfo: { email: "" } };
    store.dispatch(setFormLead(FormScreenState));
    const actions = store.getActions();
    const setEmailAction = {
      type: "SET_EMAIL"
    };

    expect(actions).not.toContainEqual(setEmailAction);
  });

  test("SetLocation Executed when showLocation is set", () => {
    store.dispatch(setFormLead(FormScreenState));
    const actions = store.getActions();
    const SetLocation = {
      type: "SET_LOCATION",
      location: {
        cityId: 0,
        cityName: ""
      }
    };
    expect(actions).toContainEqual(SetLocation);
  });

  test("Logged 'Leaving FormScreen' - setFormLead called", () => {
    store.dispatch(
      setFormLead({ ...FormScreenState, showLocation: true, showSeller: true })
    );
    const actions = store.getActions();
    const setLogAction = {
      type: SET_INFO_LOG,
      logData: {
        message: "Leaving FormScreen (Current Store Provided in currentState)",
        level: "INFO",
        currentState: {},
        interactionId: 1
      }
    };

    expect(actions).toContainEqual(setLogAction);
  });
});
