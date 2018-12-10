import { log } from "../../../src/LeadForm/Reducers/Log";
import {
  SET_ERROR_LOG,
  SET_INFO_LOG,
  SET_DEBUG_LOG
} from "../../../src/LeadForm/Actions/LogActionTypes";

const infoAction = () => ({
  type: SET_INFO_LOG,
  logData: {
    message: "Entering FormScreen",
    level: "INFO",
    currentState: {}
  }
});

const errorAction = () => ({
  type: SET_ERROR_LOG,
  logData: {
    message: "Error: API failed",
    level: "ERROR",
    currentState: {
      BuyerInfo: { name: "Ritesh", mobile: "8108378233" }
    }
  }
});

const debugAction = () => ({
  type: SET_DEBUG_LOG,
  logData: {
    message:
      "Leaving BuyerSellerLocationScreen (Current Store Provided in currentState)",
    level: "DEBUG",
    currentState: {},
    nextState: {}
  }
});

const state = () => [];

test("Reducer Log ExpectedPayload - set info log", () => {
  let reducerOutput = log(state(), infoAction());
  expect(reducerOutput[0].message).toEqual(infoAction().logData.message);
  expect(reducerOutput[0].level).toEqual(infoAction().logData.level);
  expect(reducerOutput[0].currentState).toEqual(
    infoAction().logData.currentState
  );
});

test("Reducer Log ExpectedPayload - set error log", () => {
  let reducerOutput = log(state(), errorAction());
  expect(reducerOutput[0].message).toEqual(errorAction().logData.message);
  expect(reducerOutput[0].level).toEqual(errorAction().logData.level);
  expect(reducerOutput[0].currentState).toEqual(
    errorAction().logData.currentState
  );
});

test("Reducer Log ExpectedPayload - set debug log", () => {
  let reducerOutput = log(state(), debugAction());
  expect(reducerOutput[0].message).toEqual(debugAction().logData.message);
  expect(reducerOutput[0].level).toEqual(debugAction().logData.level);
  expect(reducerOutput[0].currentState).toEqual(
    debugAction().logData.currentState
  );
  expect(reducerOutput[0].nextState).toEqual(debugAction().logData.nextState);
});

test("Reducer Log ExpectedPayload - any other action", () => {
  let reducerOutput3 = log(state(), {
    type: "SET_BUYER"
  });
  expect(reducerOutput3).toEqual(state());
});
