import { setOthers } from "../../../src/LeadForm/ActionCreators/Others";
import { SET_OTHERS } from "../../../src/LeadForm/Actions/FormActionTypes";
import { sourceType } from "../../../src/LeadForm/Enum/SourceType";

let store;

store = mockStore({});

describe("Testing Others action creator", () => {
  it("setOthers action fired", () => {
    let others = {
      testData: {
        value: 12,
        type: sourceType.ALL
      }
    };
    const action = { type: SET_OTHERS, others };
    store.dispatch(setOthers(others));
    const actions = store.getActions();
    expect(actions).toContainEqual(action);
  });
});
