import { others } from "../../../src/LeadForm/Reducers/Others";
import { SET_OTHERS } from "../../../src/LeadForm/Actions/FormActionTypes";
import { sourceType } from "../../../src/LeadForm/Enum/SourceType";

const othersAction = () => ({
  type: SET_OTHERS,
  others: {
    testData: {
      value: 12,
      type: sourceType.ALL
    }
  }
});

const state = () => [];

test("Reducer Others ExpectedPayload - set others data", () => {
  let reducerOutput = others(state(), othersAction());
  expect(reducerOutput).toEqual(othersAction().others);
});
