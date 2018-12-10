import {
  SET_INFO_LOG,
  SET_ERROR_LOG
} from "../../../src/LeadForm/Actions/LogActionTypes";
import { submitLead } from "../../../src/LeadForm/utils/LeadSubmission";
import LeadSubmissionApi from "../../../src/LeadForm/Api/LeadSubmission";
import store from "../../../src/LeadForm/store";
import * as LogState from "../../../src/LeadForm/ActionCreators/LogState";
import * as Analytics from "../../../src/utils/Analytics";
import * as LeadId from "../../../src/LeadForm/ActionCreators/LeadId";
import { SET_LEAD_ID } from "../../../src/LeadForm/Actions/FormActionTypes";

jest.mock("../../../src/LeadForm/ActionCreators/LeadId", () => {
  return {
    setLeadId: jest.fn(() => {
      return { type: "SET_LEAD_ID", LeadId: "" };
    })
  };
});

jest.mock("../../../src/utils/Analytics", () => {
  return {
    fireInteractiveTracking: jest.fn()
  };
});

jest.mock("../../../src/LeadForm/Api/LeadSubmission");

jest.mock("../../../src/LeadForm/store", () => {
  return mockStore({
    screenData: {
      screen: 1
    },
    leadClickSource: {
      propId: 1,
      page: {
        platform: {
          id: 1,
          name: "platformName"
        },
        page: {
          name: "pageName"
        }
      }
    },
    interactionId: 1
  });
});

jest.mock("../../../src/LeadForm/ActionCreators/LogState", () => {
  return {
    setInfoLog: jest.fn(() => {
      return { type: "SET_INFO_LOG", logData: {} };
    }),
    setErrorLog: jest.fn(() => {
      return { type: "SET_ERROR_LOG", logData: {} };
    })
  };
});

beforeEach(() => {
  if (LeadSubmissionApi.set) {
    LeadSubmissionApi.set.mockClear();
  }

  LeadSubmissionApi.set = jest
    .fn()
    .mockImplementation(Lead => Promise.resolve("-jhjh2234kjb"));
});

describe("Testing LeadSubmission", () => {
  test("Info Log dispatched - submitLead called, LeadSubmissionApi success", () => {
    submitLead({}, true, "action", "label");

    process.nextTick(() => {
      let actions = store.getActions();
      //   try {
      expect(actions).toContainEqual({
        type: SET_INFO_LOG,
        logData: {}
      });
      expect(LogState.setInfoLog).toHaveBeenCalledWith(
        "Form lead submitted with encryptedLeadId, -jhjh2234kjb",
        1
      );
      //   } catch (e) {
      //     console.log(e);
      //   }
    });
  });

  test("setLeadId dispatched - submitLead called, LeadSubmissionApi success", () => {
    submitLead({}, true, "action", "label");

    process.nextTick(() => {
      //   try {
      let actions = store.getActions();
      expect(actions).toContainEqual({
        type: SET_LEAD_ID,
        LeadId: ""
      });
      expect(LeadId.setLeadId).toHaveBeenCalledWith("-jhjh2234kjb");
      //   } catch (e) {
      //     console.log(e);
      //   }
    });
  });

  test("Error Log dispatch - submitLead called, LeadSubmissionApi Failed", () => {
    LeadSubmissionApi.set = jest
      .fn()
      .mockImplementation(Lead => Promise.reject("Cannot submit lead"));

    submitLead({}, true, "action", "label");

    process.nextTick(() => {
      let actions = store.getActions();
      //   try {
      expect(actions).toContainEqual({
        type: SET_ERROR_LOG,
        logData: {}
      });
      expect(LogState.setErrorLog).toHaveBeenCalledWith(
        "Error: Cannot submit lead. Found in LeadSubmission API",
        1
      );
      //   } catch (e) {
      //     console.log(e);
      //   }
    });
  });

  test("GA Tracking fired - submitLead called, LeadSubmissionApi Failed", () => {
    LeadSubmissionApi.set = jest
      .fn()
      .mockImplementation(Lead => Promise.reject("Cannot submit lead"));

    submitLead({}, "action");

    process.nextTick(() => {
      //   try {
      expect(Analytics.fireInteractiveTracking).toHaveBeenCalledWith(
        "LeadForm-platformName-pageName",
        "action",
        ""
      );
      //   } catch (e) {
      //     console.log(e);
      //   }
    });
  });
});
