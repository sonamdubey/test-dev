import { setInteractionId } from "../../../src/LeadForm/ActionCreators/InteractionId";
import { SET_INTERACTION_ID } from "../../../src/LeadForm/Actions/FormActionTypes";

describe("Testing InteractionId Actions", () => {
  let store;
  store = mockStore({});

  const interactionIdAction = {
    type: SET_INTERACTION_ID,
    interactionId: 2
  };

  test("setInteractionId Executed", () => {
    store.dispatch(setInteractionId(interactionIdAction.interactionId));
    const actions = store.getActions();
    expect(actions).toContainEqual(interactionIdAction);
  });
});
