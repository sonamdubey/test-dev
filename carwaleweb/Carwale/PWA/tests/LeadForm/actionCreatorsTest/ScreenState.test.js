import {
  SET_MLA_SCREEN,
  SET_RECOMMENDATION_SCREEN,
  SET_THANKYOU_SCREEN,
  SET_SORRY_SCREEN,
  SET_HIDE,
  SET_FORM_SCREEN,
  SET_LOCATION_SCREEN
} from "../../../src/LeadForm/Actions/ScreenActionTypes";
import {
  setScreen,
  setSorryScreen,
  setMLAScreen,
  setRecoScreen,
  setThankYouScreen,
  setHide,
  setLocationScreen,
  setFormScreen,
  setMLAData,
  setRecoData
} from "../../../src/LeadForm/ActionCreators/ScreenState";
import {
  SET_RECOMMENDATION_DATA,
  SET_MLA_DATA
} from "../../../src/LeadForm/Actions/FormActionTypes";
import recommendationApi from "../../../src/LeadForm/Api/Recommendation";
import {
  CLEAR_LOG,
  SET_INFO_LOG,
  SET_ERROR_LOG
} from "../../../src/LeadForm/Actions/LogActionTypes";
import * as Analytics from "../../../src/utils/Analytics";
import { screenType } from "../../../src/LeadForm/Enum/ScreenEnum";
import * as ObjectMapping from "../../../src/LeadForm/utils/ObjectMapping";
import { platform } from "../../../src/enum/Platform";
import bhriguLogSubmissionApi from "../../../src/LeadForm/Api/BhriguLogSubmission";
import { RefreshPage } from "../../../src/utils/UrlFactory";

//mocking Recommendation api module
jest.mock("../../../src/LeadForm/Api/Recommendation");

jest.mock("../../../src/LeadForm/Api/BhriguLogSubmission");

jest.mock("../../../src/utils/UrlFactory", () => {
  return {
    RefreshPage: jest.fn()
  };
});

jest.mock("../../../src/LeadForm/ActionCreators/LogState", () => {
  return {
    setInfoLog: jest.fn((message, interactionId) => {
      return {
        type: "SET_INFO_LOG",
        logData: { message, currentState: {}, level: "INFO", interactionId }
      };
    }),
    setErrorLog: jest.fn((message, interactionId) => {
      return {
        type: "SET_ERROR_LOG",
        logData: { message, currentState: {}, level: "ERROR", interactionId }
      };
    }),
    clearLog: jest.fn(() => {
      return { type: "CLEAR_LOG" };
    })
  };
});

jest.mock("../../../src/utils/CancelablePromise", () => {
  return {
    makeCancelable: jest.fn(promise => ({
      promise,
      cancel: jest.fn(() => {
        return true;
      })
    }))
  };
});

//mocking Analytics module
jest.mock("../../../src/utils/Analytics");

//mocking ObjectMapping module
jest.mock("../../../src/LeadForm/utils/ObjectMapping");

describe("Testing ScreenState Actions", () => {
  let store;
  let storeState;
  beforeEach(() => {
    storeState = {
      NC: {
        leadForm: {
          buyerInfo: {
            mobile: "9999999999"
          },
          MLASellers: {
            list: []
          }
        },
        campaign: {
          campaign: {
            mutualLeads: true
          },
          featuredCarData: {
            modelId: 1
          }
        }
      },
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
      location: {
        cityname: ""
      },
      screenData: {
        screen: screenType.Form
      },
      interactionId: 1,
      log: [{}],
      isCityChanged: true
    };

    store = mockStore(storeState);

    if (recommendationApi.get) {
      recommendationApi.get.mockClear();
    }
    //mocking recommendation API
    recommendationApi.get = jest
      .fn()
      .mockImplementation(() => Promise.resolve([]));

    if (bhriguLogSubmissionApi.set) {
      bhriguLogSubmissionApi.set.mockClear();
    }
    //mocking bhriguLogSubmissionApi
    bhriguLogSubmissionApi.set = jest
      .fn()
      .mockImplementation(logData => Promise.resolve(""));

    //mocking analytics
    Analytics.fireNonInteractiveTracking = jest.fn(() => ({}));

    //mocking object mapping module
    ObjectMapping.recoMappingApiToStore = jest.fn(() => ({}));
    ObjectMapping.mapBhriguLogs = jest.fn(logData => []);
  });

  test("SetMLAScreen action dispatched", () => {
    const action = {
      type: SET_MLA_SCREEN
    };
    store.dispatch(setMLAScreen());
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("SetMLAData action dispatched", () => {
    const action = {
      type: SET_MLA_DATA,
      MLAList: {}
    };
    store.dispatch(setMLAData({}));
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("SetRecoScreen action dispatched", () => {
    const action = {
      type: SET_RECOMMENDATION_SCREEN
    };
    store.dispatch(setRecoScreen());
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("SetRecoData action dispatched", () => {
    const action = {
      type: SET_RECOMMENDATION_DATA,
      recoList: {}
    };
    store.dispatch(setRecoData({}));
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("SetLocationScreen action dispatched", () => {
    const action = {
      type: SET_LOCATION_SCREEN
    };
    store.dispatch(setLocationScreen({}));
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("SetFormScreen action dispatched", () => {
    const action = {
      type: SET_FORM_SCREEN
    };
    store.dispatch(setFormScreen({}));
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("SetThankyouScreen action dispatched", () => {
    const action = {
      type: SET_THANKYOU_SCREEN
    };
    store.dispatch(setThankYouScreen({}));
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("SetSorryScreen action dispatched", () => {
    const action = {
      type: SET_SORRY_SCREEN
    };
    store.dispatch(setSorryScreen({}));
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("SetInfoLog action dispatched- setHide executed", () => {
    store.dispatch(setHide());
    const actions = store.getActions();
    expect(actions[0].type).toEqual("SET_INFO_LOG");
    expect(actions[0].logData.message).toEqual(
      "Leaving LeadForm (Current Store Provided in currentState)"
    );
    expect(actions[0].logData.currentState).toEqual({});
  });

  test("ClearLog action dispatched- setHide executed", () => {
    const action = {
      type: CLEAR_LOG
    };
    store.dispatch(setHide());
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("BhriguLogs Mapped - setHide executed", () => {
    store.dispatch(setHide());
    expect(ObjectMapping.mapBhriguLogs).toHaveBeenCalledWith([{}]);
  });

  test("Refresh page - global city changed, setHide executed", () => {
    store.dispatch(setHide());
    expect(RefreshPage).toHaveBeenCalled();
  });

  test("SetHide action dispatched- setHide executed", () => {
    const action = {
      type: SET_HIDE
    };
    store.dispatch(setHide());
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("Recommendation api success - set log action dispatched", () => {
    const action = {
      type: SET_INFO_LOG,
      logData: {
        message: "Fetched Reco data from Reco API for Reco Screen",
        level: "INFO",
        currentState: {},
        interactionId: 1
      }
    };
    store.dispatch(setScreen());
    const actions = store.getActions();
    process.nextTick(() => {
      // try {
      expect(actions).toContainEqual(action);
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Recommendation api failure - set log action dispatched", () => {
    //mocking recommendation API
    recommendationApi.get.mockClear();
    recommendationApi.get = jest
      .fn()
      .mockImplementation(() => Promise.reject("error"));

    const action = {
      type: SET_ERROR_LOG,
      logData: {
        message: `Error: error. Found in Reco API`,
        level: "ERROR",
        currentState: {},
        interactionId: 1
      }
    };
    store.dispatch(setScreen());
    const actions = store.getActions();
    process.nextTick(() => {
      // try {
      expect(actions).toContainEqual(action);
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("SetScreen MLA data available - set MLA screen dispatched", () => {
    const action = {
      type: SET_MLA_SCREEN
    };

    storeState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        leadForm: {
          ...storeState.NC.leadForm,
          MLASellers: {
            ...storeState.NC.leadForm.MLASellers,
            list: ["mla data"]
          }
        }
      }
    };

    store = mockStore(storeState);
    store.dispatch(setScreen());
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });

  test("SetScreen MLA data not available - fire tracking", () => {
    store.dispatch(setScreen());
    expect(Analytics.fireNonInteractiveTracking).toHaveBeenCalled();
  });

  test("SetScreen No Reco data - fire tracking", () => {
    store.dispatch(setScreen());
    process.nextTick(() => {
      // try {
      expect(Analytics.fireNonInteractiveTracking).toHaveBeenCalled();
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("SetScreen No Reco data and current screen is not MLA - set thankyou screen", () => {
    const action = {
      type: SET_THANKYOU_SCREEN
    };
    storeState = {
      ...storeState,
      screenData: {
        ...storeState.screenData,
        screen: screenType.Reco
      }
    };

    store = mockStore(storeState);

    store.dispatch(setScreen());
    let actions = store.getActions();
    process.nextTick(() => {
      // try {
      expect(actions).toContainEqual(action);
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });
});
