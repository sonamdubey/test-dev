import React from "react";
import SorryScreen from "../../../src/LeadForm/Components/SorryScreen";
import store from "../../../src/LeadForm/store";
import { submitBhriguLogs } from "../../../src/LeadForm/utils/BhriguSubmission";
import { CLEAR_LOG } from "../../../src/LeadForm/Actions/LogActionTypes";

jest.mock("../../../src/LeadForm/utils/BhriguSubmission", () => {
  return { submitBhriguLogs: jest.fn() };
});

jest.mock("../../../src/LeadForm/ActionCreators/ScreenState", () => {
  return {
    clearLog: jest.fn(() => ({
      type: "CLEAR_LOG"
    }))
  };
});

jest.mock("../../../src/LeadForm/store", () => {
  return mockStore({
    log: [{}]
  });
});

let props = { setLog: jest.fn() };
let wrapper;
beforeEach(() => {
  wrapper = shallowWithStore(<SorryScreen {...props} />, store);
});

describe("Testing SorryScreen Component - DOM", () => {
  test("Component SorryScreen Snapshot RenderedTree", () => {
    expect(wrapper).toMatchSnapshot();
  });
});

describe("Testing SorryScreen Component - ComponentDidMount", () => {
  test("'setlog' of props called - sorry screen mounted", () => {
    expect(props.setLog).toHaveBeenCalled();
  });

  test("SubmitBhriguLogs called - sorry screen mounted", () => {
    expect(submitBhriguLogs).toHaveBeenCalledWith([{}]);
  });

  test("Logs Cleared - sorry screen mounted", () => {
    let clearLog = { type: CLEAR_LOG };
    let actions = store.getActions();
    expect(actions).toContainEqual(clearLog);
  });
});
