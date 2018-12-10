import {
  setEmail,
  setThankYouLead
} from "../../../src/LeadForm/ActionCreators/ThankYouState";
import { SET_EMAIL } from "../../../src/LeadForm/Actions/FormActionTypes";
import * as ObjectMapping from "../../../src/LeadForm/utils/ObjectMapping";
import * as LeadSubmission from "../../../src/LeadForm/utils/LeadSubmission";

//mocking lead submission api module
jest.mock("../../../src/LeadForm/utils/LeadSubmission", () => {
  return {
    submitLead: jest.fn()
  };
});

describe("Testing ThankyouState Actions", () => {
  let store;

  beforeEach(() => {
    store = mockStore({});

    //mocking object mapping module
    ObjectMapping.leadMappingEmailStoreToApi = jest.fn(() => {
      return "lead";
    });
  });

  const action = {
    type: SET_EMAIL,
    email: "rj@carwale.com"
  };

  test("leadMappingEmailStoreToApi called and Returns Mapped lead object - setThankYouLead called", () => {
    store.dispatch(setThankYouLead("rj@carwale.com"));
    expect(ObjectMapping.leadMappingEmailStoreToApi).toHaveBeenCalledWith(
      {},
      "rj@carwale.com"
    );
  });

  test("submitLead called with lead data and 'false' as arguments - setThankYouLead Called", () => {
    store.dispatch(setThankYouLead({}));
    expect(LeadSubmission.submitLead).toHaveBeenCalledWith("lead", false);
  });

  test("Set email action dispatched", () => {
    store.dispatch(setEmail(action.email));
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });
});
