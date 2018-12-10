import {
  SET_ERROR_LOG,
  SET_INFO_LOG,
  SET_DEBUG_LOG,
  CLEAR_LOG
} from "../../../src/LeadForm/Actions/LogActionTypes";
import {
  setInfoLog,
  setErrorLog,
  setDebugLog,
  clearLog
} from "../../../src/LeadForm/ActionCreators/LogState";
import * as storeData from "../../../src/LeadForm/utils/StoreData";

jest.mock("../../../src/LeadForm/utils/StoreData");

storeData.getStateFromStore = jest.fn(() => {
  return {};
});

describe("Testing LogState Actions ", () => {
  let store;
  store = mockStore({});

  const infoLogAction = {
    type: SET_INFO_LOG,
    logData: {
      message: "Entering BuyerScreen",
      level: "INFO",
      currentState: {}
    }
  };

  const errorLogAction = {
    type: SET_ERROR_LOG,
    logData: {
      message: "Error: API failed",
      level: "ERROR",
      currentState: {}
    }
  };

  const testCurrentState = {
    buyerInfo: "",
    assignedDealerId: "",
    cityId: ""
  };

  const testNextState = {
    buyerInfo: "",
    assignedDealerId: "",
    cityId: ""
  };

  const debugLogAction = {
    type: SET_DEBUG_LOG,
    logData: {
      message: "Name or Mobile changed",
      level: "DEBUG",
      currentState: testCurrentState,
      nextState: testNextState
    }
  };

  test("setInfolog Executed", () => {
    store.dispatch(setInfoLog("Entering BuyerScreen"));
    const actions = store.getActions();
    expect(actions).toContainEqual(infoLogAction);
  });

  test("setErrorLog Executed", () => {
    store.dispatch(setErrorLog("Error: API failed"));
    const actions = store.getActions();
    expect(actions).toContainEqual(errorLogAction);
  });

  test("setDebugLog Executed", () => {
    store.dispatch(
      setDebugLog("Name or Mobile changed", testCurrentState, testNextState)
    );
    const actions = store.getActions();
    expect(actions).toContainEqual(debugLogAction);
  });

  test("ClearLog Executed", () => {
    store.dispatch(clearLog());
    const actions = store.getActions();
    expect(actions).toContainEqual({ type: CLEAR_LOG });
  });

  test("ClearLog Executed", () => {
    store.dispatch(clearLog());
    const actions = store.getActions();
    expect(actions).toContainEqual({ type: CLEAR_LOG });
  });
});
